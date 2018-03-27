using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Exceptions;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.BusinessLogic.Gdpr;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.Services
{
    public enum GDPROperations
    {
        DataPrivacy
    };

    public interface IGDPROperationsService
    {
        bool RemoveDataPrivacyFromCase(DataPrivacyParameters p, int userId, string url);
    }

    public interface IGDPRDataPrivacyAccessService
    {
        GDPRDataPrivacyAccess GetByUserId(int userId);
    }

    public interface IGDPRFavoritesService
    {
        IDictionary<int, string> ListFavorites();
        GdprFavoriteModel GetFavorite(int id);
        int SaveFavorite(GdprFavoriteModel model);
        void DeleteFavorite(int favoriteId);
    }

    public class GDPRService : IGDPROperationsService, IGDPRDataPrivacyAccessService, IGDPRFavoritesService
    {
        private readonly IGDPRDataPrivacyAccessRepository _privacyAccessRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IGDPROperationsAuditRespository _gdprOperationsAuditRespository;
        private readonly IJsonSerializeService _jsonSerializeService;
        private readonly IGDPRDataPrivacyFavoriteRepository _gdprFavoritesRepository;
        private readonly IBusinessModelToEntityMapper<GdprFavoriteModel, GDPRDataPrivacyFavorite> _gdprFavoritesEntityMapper;
        private readonly IEntityToBusinessModelMapper<GDPRDataPrivacyFavorite, GdprFavoriteModel> _gdprFavoritesModelMapper;

        #region ctor()

        public GDPRService(IGDPRDataPrivacyAccessRepository privacyAccessRepository,
                           IUnitOfWorkFactory unitOfWorkFactory,
                           IGDPROperationsAuditRespository operationsAuditRespository,
                           IGDPRDataPrivacyFavoriteRepository favoritesRepository,
                           IBusinessModelToEntityMapper<GdprFavoriteModel, GDPRDataPrivacyFavorite> gdprFavoritesEntityMapper,
                           IEntityToBusinessModelMapper<GDPRDataPrivacyFavorite, GdprFavoriteModel> gdprFavoritesModelMapper,
                           IJsonSerializeService jsonSerializeService)
        {
            _gdprFavoritesEntityMapper = gdprFavoritesEntityMapper;
            _gdprFavoritesModelMapper = gdprFavoritesModelMapper;
            _gdprFavoritesRepository = favoritesRepository;
            _jsonSerializeService = jsonSerializeService;
            _unitOfWorkFactory = unitOfWorkFactory;
            _privacyAccessRepository = privacyAccessRepository;
            _gdprOperationsAuditRespository = operationsAuditRespository;
        }

        #endregion

        #region IGDPRDataPrivacyAccessService

        public GDPRDataPrivacyAccess GetByUserId(int userId)
        {
            return _privacyAccessRepository.GetByUserId(userId);
        }

        #endregion

        public IDictionary<int, string> ListFavorites()
        {
            var favorites = _gdprFavoritesRepository.ListFavorites();
            return favorites;
        }

        public GdprFavoriteModel GetFavorite(int id)
        {
            var entity = _gdprFavoritesRepository.GetById(id);
            var model = _gdprFavoritesModelMapper.Map(entity);
            return model;
        }

        public void DeleteFavorite(int favoriteId)
        {
            _gdprFavoritesRepository.Delete(f => f.Id == favoriteId);
            _gdprFavoritesRepository.Commit();
        }

        public int SaveFavorite(GdprFavoriteModel model)
        {
            int res;
            using (var uow = _unitOfWorkFactory.Create())
            {
                var repo = uow.GetRepository<GDPRDataPrivacyFavorite>();
                var entity = repo.GetById(model.Id);
                if (entity == null)
                {
                    entity = new GDPRDataPrivacyFavorite();
                    repo.Add(entity);
                }

                _gdprFavoritesEntityMapper.Map(model, entity);
                uow.Save();

                res = entity.Id;
            }

            return res;
        }

        public bool RemoveDataPrivacyFromCase(DataPrivacyParameters p, int userId, string url)
        {
            var auditData = SaveOperationAuditData(p, userId, url);
            var casesIds = new List<int>();

            try
            {
                using (var uow = _unitOfWorkFactory.Create())
                {
                    var rep = uow.GetRepository<Case>();
                    var caseFiles = uow.GetRepository<CaseFile>();
                    var followers = uow.GetRepository<CaseExtraFollower>();
                    var logFiles = uow.GetRepository<LogFile>();
                    var emailLogs = uow.GetRepository<EmailLog>();

                    var casesQueryable = rep.GetAll().Where(x => x.Customer_Id == p.SelectedCustomerId);

                    if (p.ClosedOnly)
                        casesQueryable = casesQueryable.Where(x => x.FinishingDate.HasValue);

                    if (p.RegisterDateFrom.HasValue)
                    {
                        var dateFrom = p.RegisterDateFrom.Value.Date;
                        casesQueryable = casesQueryable.Where(x => x.RegTime >= dateFrom);
                    }

                    if (p.RegisterDateTo.HasValue)
                    {
                        var dateTo = p.RegisterDateTo.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        casesQueryable = casesQueryable.Where(x => x.RegTime <= dateTo);
                    }

                    var cases = casesQueryable.ToList();

                    if (cases.Any())
                    {
                        casesIds = cases.Select(c => c.Id).ToList();
                        
                        ProcessReplaceCasesData(cases, caseFiles, logFiles, emailLogs, followers, p);
                        uow.Save();
                    }
                }
            }
            catch (Exception e)
            {
                auditData.Success = false;
                auditData.Error = $"Unknown error. { e.Message }";
                SaveAuditOperationResult(auditData);
                throw;
            }

            //save affected cases Ids
            auditData.Result = string.Join(",", casesIds);
            auditData.Success = true;
            SaveAuditOperationResult(auditData);

            return true;
        }

        #region Replace case data

        private void ProcessReplaceCasesData(
            IList<Case> cases,
            IRepository<CaseFile> caseFiles,
            IRepository<LogFile> logFiles,
            IRepository<EmailLog> emailLogs,
            IRepository<CaseExtraFollower> followers,
            DataPrivacyParameters p)
        {
            var replaceDataWith = p.ReplaceDataWith ?? string.Empty;
            var replaceDatesWith = p.ReplaceDatesWith;

            foreach (var c in cases)
            {
                foreach (var fieldName in p.FieldsNames)
                {
                    if (fieldName == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.Place.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.UserCode.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.Caption.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.Description.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.Miscellaneous.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.Available.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString() ||
                        fieldName == GlobalEnums.TranslationCaseFields.FinishingDescription.ToString()
                        )
                    {
                        var property = c.GetType().GetProperty(fieldName);
                        if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                        property.SetValue(c, replaceDataWith);

                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Region_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Department_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.OU_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.System_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Impact_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Category_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Priority_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.Status_Id.ToString() ||
                             fieldName == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()

                        )
                    {
                        var property = c.GetType().GetProperty(fieldName);
                        if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                        property.SetValue(c, null);

                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
                    {
                        c.PersonsName = replaceDataWith; //first name last name?
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString())
                    {
                        c.PersonsEmail = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString())
                    {
                        c.PersonsPhone = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString())
                    {
                        c.PersonsCellphone = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.ReportedBy = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.Person_Name = replaceDataWith; //first last name?
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.Person_Email = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.Person_Phone = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.Person_Cellphone = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.Region_Id = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.Department_Id = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.OU_Id = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.CostCentre = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.Place = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString())
                    {
                        if (c.IsAbout != null)
                            c.IsAbout.UserCode = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString())
                    {
                        c.InventoryType = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.User_Id.ToString())
                    {
                        c.User_Id = null;
                        c.RegUserId = replaceDataWith;
                        c.RegUserDomain = replaceDataWith;
                        c.RegUserName = replaceDataWith;
                        c.IpAddress = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString())
                    {
                        c.RegistrationSourceCustomer_Id = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                    {
                        c.AgreedDate = replaceDatesWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Cost.ToString())
                    {
                        c.Cost = 0;
                        c.OtherCost = 0;
                        c.Currency = replaceDataWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Project.ToString())
                    {
                        c.Project_Id = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Problem.ToString())
                    {
                        c.Problem_Id = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
                    {
                        c.CausingPartId = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.Change.ToString())
                    {
                        c.Change_Id = null;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.PlanDate.ToString())
                    {
                        c.PlanDate = replaceDatesWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.WatchDate.ToString())
                    {
                        c.WatchDate = replaceDatesWith;
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.SolutionRate.ToString())
                    {
                        c.SolutionRate = string.Empty;
                    }
                    else if (fieldName == "tblLog.Text_External")
                    {
                        foreach (var log in c.Logs.ToList())
                        {
                            log.Text_External = replaceDataWith;
                        }
                    }
                    else if (fieldName == "tblLog.Text_Internal")
                    {
                        foreach (var log in c.Logs.ToList())
                        {
                            log.Text_Internal = replaceDataWith;
                        }
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.ClosingReason.ToString())
                    {
                        foreach (var log in c.Logs.ToList())
                        {
                            log.FinishingType = null;
                        }
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.FinishingDate.ToString())
                    {
                        c.FinishingDate = replaceDatesWith;
                    }
                    else if (fieldName == AdditionalDataPrivacyFields.SelfService_RegUser.ToString())
                    {
                        c.Logs.ForEach(l => l.RegUser = replaceDataWith);
                    }
                    else if (fieldName == GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString())
                    {
                        if (c.CaseFollowers.Any())
                        {
                            foreach (var follower in c.CaseFollowers.ToList())
                            {
                                followers.Delete(follower);
                            }
                        }
                    }
                    
                }

                if (p.RemoveCaseAttachments && c.CaseFiles.Any())
                {
                    foreach (var caseFile in c.CaseFiles.ToList())
                    {
                        caseFiles.Delete(caseFile);
                    }
                }

                if (p.RemoveLogAttachments && c.Logs.Any())
                {
                    foreach (var log in c.Logs)
                    {
                        if (log.LogFiles.Any())
                        {
                            foreach (var lFile in log.LogFiles.ToList())
                            {
                                logFiles.Delete(lFile);
                            }
                        }
                    }
                }

                ProcessReplaceCasesHistoryData(c, emailLogs, p);
            }
        }

        private void ProcessReplaceCasesHistoryData(Case c, IRepository<EmailLog> emailLogs, DataPrivacyParameters dataPrivacyParameters)
        {
            if (c.CaseHistories.Any())
            {
                var replaceDataWith = dataPrivacyParameters.ReplaceDataWith ?? string.Empty;
                var replaceDatesWith = dataPrivacyParameters.ReplaceDatesWith;
                var deleteEmailLogs = false;

                foreach (var caseHistory in c.CaseHistories.ToList())
                {
                    foreach (var fieldName in dataPrivacyParameters.FieldsNames)
                    {
                        if (fieldName == GlobalEnums.TranslationCaseFields.ReportedBy.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.CostCentre.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Place.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.UserCode.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Caption.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Description.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Miscellaneous.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.Available.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.FinishingDescription.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString() ||
                            fieldName == GlobalEnums.TranslationCaseFields.ClosingReason.ToString()
                            )
                        {
                            var property = caseHistory.GetType().GetProperty(fieldName);
                            if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                            property.SetValue(caseHistory, replaceDataWith);

                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Region_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Department_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.OU_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.System_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Impact_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Category_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Priority_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.Status_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString() ||
                                 fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()
                            )
                        {
                            var property = caseHistory.GetType().GetProperty(fieldName);
                            if (property == null) throw new PropertyNotFoundException("Property not found", fieldName);

                            property.SetValue(caseHistory, null);

                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
                        {
                            caseHistory.PersonsName = replaceDataWith; //first name last name?
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString())
                        {
                            caseHistory.PersonsEmail = replaceDataWith;
                        }
                        if (fieldName == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString())
                        {
                            caseHistory.PersonsPhone = replaceDataWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString())
                        {
                            caseHistory.PersonsCellphone = replaceDataWith;
                        }
                        // No such fields in Case History
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString())
                        //{
                        //    //ch.IsAbout_Persons_EMail = replaceDataWith;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString())
                        //{
                        //    //ch.IsAbout_Persons_CellPhone = replaceDataWith;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString())
                        //{
                        //    //ch.IsAbout_Region_Id = null;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString())
                        //{
                        //    //ch.IsAbout_OU_Id = null;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString())
                        //{
                        //    //ch.IsAbout_CostCentre = replaceDataWith;
                        //}
                        //else if (fieldName == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString())
                        //{
                        //    //ñh.IsAbout_Place = replaceDataWith;
                        //}
                        else if (fieldName == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString())
                        {
                            caseHistory.InventoryType = replaceDataWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.User_Id.ToString())
                        {
                            caseHistory.User_Id = null;
                            caseHistory.RegUserId = replaceDataWith;
                            caseHistory.RegUserDomain = replaceDataWith;
                            caseHistory.IpAddress = replaceDataWith;
                            caseHistory.CreatedByUser = replaceDataWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString())
                        {
                            caseHistory.RegistrationSourceCustomer_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                        {
                            caseHistory.AgreedDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Cost.ToString())
                        {
                            caseHistory.Cost = 0;
                            caseHistory.OtherCost = 0;
                            caseHistory.Currency = replaceDataWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Project.ToString())
                        {
                            caseHistory.Project_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Problem.ToString())
                        {
                            caseHistory.Problem_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
                        {
                            caseHistory.CausingPartId = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.Change.ToString())
                        {
                            caseHistory.Change_Id = null;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.PlanDate.ToString())
                        {
                            caseHistory.PlanDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.WatchDate.ToString())
                        {
                            caseHistory.WatchDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.SolutionRate.ToString())
                        {
                            caseHistory.SolutionRate = string.Empty;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.FinishingDate.ToString())
                        {
                            caseHistory.FinishingDate = replaceDatesWith;
                        }
                        else if (fieldName == GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString())
                        {
                            caseHistory.CaseExtraFollowers = replaceDataWith;
                            deleteEmailLogs = true;
                        }
                        else if (fieldName == "tblLog.Text_External" 
                            || fieldName == "tblLog.Text_Internal")
                        {
                            deleteEmailLogs = true;
                        }
                    }

                    if (deleteEmailLogs)
                    {
                        var toReplace =  emailLogs.GetAll().Where(x => x.CaseHistory_Id == caseHistory.Id).ToList();
                        if (toReplace.Any())
                        {
                            toReplace.ForEach(e => e.EmailAddress = e.Bcc = e.Cc = replaceDataWith);
                        }
                    }
                }
            }
        }

        private string FormatOperationParameters(DataPrivacyParameters p)
        {
            return _jsonSerializeService.Serialize(p);
        }

        #endregion

        #region Audit Methods

        private GDPROperationsAudit SaveOperationAuditData(DataPrivacyParameters p, int userId, string url)
        {
            var auditData = new GDPROperationsAudit
            {
                User_Id = userId,
                Operation = GDPROperations.DataPrivacy.ToString(),
                Parameters = FormatOperationParameters(p),
                Url = url,
                Application = ApplicationType.Helpdesk.ToString(),
                CreatedDate  = DateTime.UtcNow
            };

            _gdprOperationsAuditRespository.Add(auditData);
            _gdprOperationsAuditRespository.Commit();

            return auditData;
        }

        public void SaveAuditOperationResult(GDPROperationsAudit auditData)
        {
            _gdprOperationsAuditRespository.Update(auditData);
            _gdprOperationsAuditRespository.Commit();
        }

        #endregion
    }
}
   