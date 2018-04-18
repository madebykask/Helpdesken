using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Exceptions;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using DH.Helpdesk.Domain.GDPR;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.Services
{
    public enum GDPROperations
    {
        DataPrivacy
    };

    public interface IGDPROperationsService
    {
        IDictionary<int, string> GetOperationAuditCustomers();
        IList<GdprOperationsAuditOverview> ListGdprOperationsAuditItems(int? customerId, bool successOnly = true);
        int CreateDataPrivacyOperationAudit(int userId, int customerId, string url);
        GDPROperationsAudit GetDataPrivacyOperationAuditData(int id);
        bool RemoveDataPrivacyFromCase(DataPrivacyParameters p);
    }
    
    public interface IGDPRDataPrivacyAccessService
    {
        GDPRDataPrivacyAccess GetByUserId(int userId);
    }

    public interface IGDPRFavoritesService
    {
        IDictionary<int, string> ListFavorites();
        GdprFavoriteModel GetFavorite(int id);
        int SaveFavorite(GdprFavoriteModel model, int? userId);
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

        public int SaveFavorite(GdprFavoriteModel model, int? userId)
        {
            int res;
            using (var uow = _unitOfWorkFactory.Create())
            {
                var repo = uow.GetRepository<GDPRDataPrivacyFavorite>();
                var entity = repo.GetById(model.Id);
                if (entity == null)
                {
                    entity = new GDPRDataPrivacyFavorite
                    {
                        CreatedByUser_Id = userId
                    };

                    repo.Add(entity);
                }

                _gdprFavoritesEntityMapper.Map(model, entity);

                entity.ChangedByUser_Id = userId;
                entity.ChangedDate = DateTime.Now;

                uow.Save();

                res = entity.Id;
            }

            return res;
        }

        public GDPROperationsAudit GetDataPrivacyOperationAuditData(int id)
        {
            return _gdprOperationsAuditRespository.Get(id);
        }

        public int CreateDataPrivacyOperationAudit(int userId, int customerId, string url)
        {
            var auditData = CreateOperationAuditData(userId, customerId, url);
            return auditData.Id;
        }

        public bool RemoveDataPrivacyFromCase(DataPrivacyParameters p)
        {
            //update operation audit data
            var auditData = _gdprOperationsAuditRespository.GetById(p.OperationAuditId);
            auditData.Parameters = SerializeOperationParameters(p);
            auditData.Status = GDPROperationStatus.Running;
            UpdateAuditOperationData(auditData);

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
                    var extendedCaseValuesRep = uow.GetRepository<ExtendedCaseValueEntity>();
                    var formFieldValueRep = uow.GetRepository<FormFieldValue>();
                    var formFieldValueHistoryRep = uow.GetRepository<FormFieldValueHistory>();
                    var caseExtendedCaseRep = uow.GetRepository<Case_ExtendedCaseEntity>();
                    var caseSectionExtendedCaseRep = uow.GetRepository<Case_CaseSection_ExtendedCase>();

                    var casesQueryable = rep.GetAll()
                        .IncludePath(c => c.CaseHistories)
                        .IncludePath(c => c.CaseExtendedCaseDatas.Select(ec => ec.ExtendedCaseData.ExtendedCaseValues))
                        .IncludePath(c => c.CaseExtendedCaseDatas.Select(ec => ec.ExtendedCaseForm))
                        .IncludePath(c => c.CaseSectionExtendedCaseDatas.Select(esc => esc.ExtendedCaseData.ExtendedCaseValues))
                        .Where(x => x.Customer_Id == p.SelectedCustomerId);

                    if (p.RemoveCaseAttachments)
                    {
                        casesQueryable = casesQueryable.IncludePath(x => x.CaseFiles);
                    }

                    if (p.RemoveLogAttachments || p.FieldsNames.Contains("tblLog.Text_External") ||
                        p.FieldsNames.Contains("tblLog.Text_Internal") || p.FieldsNames.Contains(GlobalEnums.TranslationCaseFields.ClosingReason.ToString()))
                    {
                        casesQueryable = casesQueryable.IncludePath(x => x.Logs.Select(f => f.LogFiles));
                    }

                    if (p.FieldsNames.Contains(GlobalEnums.TranslationCaseFields.AddFollowersBtn.ToString()))
                    {
                        casesQueryable = casesQueryable.IncludePath(x => x.CaseFollowers);
                    }

                    if (p.ReplaceEmails)
                    {
                        casesQueryable = casesQueryable.IncludePath(x => x.Mail2Tickets);
                        casesQueryable = casesQueryable.IncludePath(x => x.CaseHistories.Select(y => y.Emaillogs));
                    }

                    if (p.ClosedOnly)
                        casesQueryable = casesQueryable.Where(x => x.FinishingDate.HasValue);

                    if (p.RegisterDateFrom.HasValue)
                    {
                        p.RegisterDateFrom = p.RegisterDateFrom.Value.Date;
                        casesQueryable = casesQueryable.Where(x => x.RegTime >= p.RegisterDateFrom.Value);
                    }

                    if (p.RegisterDateTo.HasValue)
                    {
                        //fix date
                        p.RegisterDateTo = p.RegisterDateTo.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
                        casesQueryable = casesQueryable.Where(x => x.RegTime <= p.RegisterDateTo.Value);
                    }

                    var cases = casesQueryable.ToList();

                    if (cases.Any())
                    {
                        casesIds = cases.Select(c => c.Id).ToList();

                        p.ReplaceDataWith = p.ReplaceDataWith ?? string.Empty;

                        foreach (var c in cases)
                        {
                            ProcessReplaceCasesData(c, caseFiles, logFiles, followers, p);
                            ProcessReplaceCasesHistoryData(c, emailLogs, p);
                            ProcessExtededCaseData(c, caseExtendedCaseRep, caseSectionExtendedCaseRep, extendedCaseValuesRep, p);
                        }

                        ProccessFormPlusCaseData(cases.Select(c => c.Id).ToList(), formFieldValueHistoryRep, formFieldValueRep);

                        uow.Save();
                    }
                }
            }
            catch (Exception e)
            {
                auditData.Success = false;
                auditData.Status = GDPROperationStatus.Complete;
                auditData.Error = $"Unknown error. { e.Message }";
                UpdateAuditOperationData(auditData);
                throw;
            }


            //save affected cases Ids
            auditData.Result = string.Join(",", casesIds);
            auditData.Status = GDPROperationStatus.Complete;
            auditData.Success = true;
            UpdateAuditOperationData(auditData);

            return true;
        }


        #region Replace case data

        private void ProcessReplaceCasesData(
            Case c,
            IRepository<CaseFile> caseFiles,
            IRepository<LogFile> logFiles,
            IRepository<CaseExtraFollower> followers,
            DataPrivacyParameters parameters)
        {

            var replaceDataWith = parameters.ReplaceDataWith;
            var replaceDatesWith = parameters.ReplaceDatesWith;


            foreach (var fieldName in parameters.FieldsNames)
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
                    c.RegUserId = ReplaceWith(c.RegUserId, replaceDataWith);
                    c.RegUserDomain = ReplaceWith(c.RegUserDomain, replaceDataWith);
                    c.RegUserName = ReplaceWith(c.RegUserName, replaceDataWith);
                    c.IpAddress = ReplaceWith(c.IpAddress, replaceDataWith);

                    //clean logs
                    foreach (var log in c.Logs.ToList())
                    {
                        log.RegUser = replaceDataWith;
                        log.User_Id = null;
                    }
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString())
                {
                    c.RegistrationSourceCustomer_Id = null;
                }
                else if (fieldName == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                {
                    c.AgreedDate = replaceDatesWith;
                }
                //else if (fieldName == GlobalEnums.TranslationCaseFields.Cost.ToString())
                //{
                //    c.Cost = 0;
                //    c.OtherCost = 0;
                //    c.Currency = replaceDataWith;
                //}
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

                if (parameters.RemoveCaseAttachments && c.CaseFiles.Any())
                {
                    foreach (var caseFile in c.CaseFiles.ToList())
                    {
                        caseFiles.Delete(caseFile);
                    }
                }

                if (parameters.RemoveLogAttachments && c.Logs.Any())
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

            }
        }

        private void ProcessReplaceCasesHistoryData(Case c, IRepository<EmailLog> emailLogs, DataPrivacyParameters parameters)
        {
            if (c.CaseHistories.Any())
            {
                var replaceDataWith = parameters.ReplaceDataWith;
                var replaceDatesWith = parameters.ReplaceDatesWith;

                foreach (var caseHistory in c.CaseHistories.ToList())
                {
                    foreach (var fieldName in parameters.FieldsNames)
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
                            caseHistory.RegUserId = ReplaceWith(caseHistory.RegUserId, replaceDataWith);
                            caseHistory.RegUserDomain = ReplaceWith(caseHistory.RegUserDomain, replaceDataWith);
                            caseHistory.IpAddress = ReplaceWith(caseHistory.IpAddress, replaceDataWith);
                            caseHistory.CreatedByUser = ReplaceWith(caseHistory.CreatedByUser, replaceDataWith);
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
                        }
                    }

                    if (parameters.ReplaceEmails)
                    {
                        var toReplace = c.CaseHistories.SelectMany(ch => ch.Emaillogs).ToList();//emailLogs.GetAll().Where(x => x.CaseHistory_Id == caseHistory.Id).ToList();
                        if (toReplace.Any())
                        {
                            toReplace.ForEach(e => e.EmailAddress = e.Bcc = e.Cc = replaceDataWith);
                        }

                        if (c.Mail2Tickets.Any())
                        {
                            foreach (var mail2Ticket in c.Mail2Tickets)
                            {
                                mail2Ticket.EMailAddress = replaceDataWith;
                                mail2Ticket.CreatedDate = replaceDatesWith ?? DateTime.Now; ;
                            }
                        }
                    }
                }
            }
        }

        private string ReplaceWith(string curValue, string resetValue)
        {
            if (string.IsNullOrEmpty(curValue))
            {
                return curValue;
            }
            return resetValue;
        }

        private void ProcessExtededCaseData(Case c, IRepository<Case_ExtendedCaseEntity> caseExtendedCaseRep,
            IRepository<Case_CaseSection_ExtendedCase> caseSectionExtendedCaseRep,
            IRepository<ExtendedCaseValueEntity> extendedCaseValuesRep,
            DataPrivacyParameters parameters)
        {
            var replaceDataWith = parameters.ReplaceDataWith;
            var replaceDatesWith = parameters.ReplaceDatesWith;

            if (c.CaseExtendedCaseDatas != null && c.CaseExtendedCaseDatas.Any())
            {
                foreach (var caseData in c.CaseExtendedCaseDatas.ToList())
                {
                    CleanExtendedCaseData(extendedCaseValuesRep, caseData.ExtendedCaseData, replaceDataWith, replaceDatesWith);
                    if (caseData.ExtendedCaseForm != null)
                    {
                        caseData.ExtendedCaseForm.CreatedBy = replaceDataWith;
                        caseData.ExtendedCaseForm.CreatedOn = replaceDatesWith ?? DateTime.Now;
                        caseData.ExtendedCaseForm.UpdatedBy = replaceDataWith;
                        caseData.ExtendedCaseForm.UpdatedOn = replaceDatesWith;
                    }
                    caseExtendedCaseRep.Delete(caseData);
                }
                c.CaseExtendedCaseDatas.Clear();
                //caseExtendedCaseRep.DeleteWhere(ce => ce.Case_Id == c.Id);
            }

            if (c.CaseSectionExtendedCaseDatas != null && c.CaseSectionExtendedCaseDatas.Any())
            {
                foreach (var caseData in c.CaseSectionExtendedCaseDatas.ToList())
                {
                    CleanExtendedCaseData(extendedCaseValuesRep, caseData.ExtendedCaseData, replaceDataWith, replaceDatesWith);
                    caseSectionExtendedCaseRep.Delete(caseData);
                }
                c.CaseSectionExtendedCaseDatas.Clear();
                //caseSectionExtendedCaseRep.DeleteWhere(cs => cs.Case_Id == c.Id);
            }

        }

        private static void CleanExtendedCaseData(IRepository<ExtendedCaseValueEntity> extendedCaseValuesRep, ExtendedCaseDataEntity caseData,
            string replaceDataWith, DateTime? replaceDatesWith)
        {
            if (caseData != null)
            {
                if (caseData.ExtendedCaseValues != null && caseData.ExtendedCaseValues.Any())
                {
                    foreach (var value in caseData.ExtendedCaseValues.ToList())
                    {
                        extendedCaseValuesRep.Delete(value);
                    }
                    caseData.ExtendedCaseValues.Clear();
                }
                caseData.CreatedBy = replaceDataWith;
                caseData.CreatedOn = replaceDatesWith ?? DateTime.Now;
                caseData.UpdatedBy = replaceDataWith;
                caseData.UpdatedOn = replaceDatesWith;
            }
        }

        private void ProccessFormPlusCaseData(IList<int> casesIds, IRepository<FormFieldValueHistory> formFieldValueHistoryRep, IRepository<FormFieldValue> formFieldValueRep)
        {
            if (!casesIds.Any()) return;

            formFieldValueRep.DeleteWhere(x => casesIds.Contains(x.Case_Id));
            formFieldValueHistoryRep.DeleteWhere(x => casesIds.Contains(x.Case_Id));
        }

        private string SerializeOperationParameters(DataPrivacyParameters p)
        {
            return _jsonSerializeService.Serialize(p);
        }

        private DataPrivacyParameters DeserializeOperationParameters(string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var parameters = _jsonSerializeService.Deserialize<DataPrivacyParameters>(data);
            return parameters;
        }

        #endregion

        #region Audit Methods

        public IDictionary<int, string> GetOperationAuditCustomers()
        {
            var res = _gdprOperationsAuditRespository.GetOperationsAuditCustomers();
            return res;
        }

        public IList<GdprOperationsAuditOverview> ListGdprOperationsAuditItems(int? customerId, bool successOnly = true)
        {
            var res = new List<GdprOperationsAuditOverview>();

            var cusId = customerId ?? 0;
            var query = _gdprOperationsAuditRespository.GetMany(x => cusId == 0 || x.Customer_Id == cusId);

            if (successOnly)
            {
                query = query.Where(x => x.Success);
            }

            var items = query.Select(x => new
            {
                CustomerId = x.Customer_Id,
                Data = x.Parameters,
                CreatedDate = x.CreatedDate
            }).OrderByDescending(x => x.CreatedDate).ToList();

            foreach (var item in items)
            {
                var operationParams = DeserializeOperationParameters(item.Data);
                if (operationParams != null)
                {
                    var auditData = new GdprOperationsAuditOverview
                    {
                        CustomerId = item.CustomerId ?? 0,
                        RegDateFrom = operationParams.RegisterDateFrom,
                        RegDateTo = operationParams.RegisterDateTo,
                        ClosedOnly = operationParams.ClosedOnly,
                        ReplaceEmails = operationParams.ReplaceEmails,
                        Fields = operationParams.FieldsNames,
                        RemoveCaseAttachments = operationParams.RemoveCaseAttachments,
                        RemoveLogAttachments = operationParams.RemoveLogAttachments,
                        ExecutedDate = item.CreatedDate
                    };

                    res.Add(auditData);
                }
            }
            return res;
        }

        public void UpdateAuditOperationData(GDPROperationsAudit auditData)
        {
            _gdprOperationsAuditRespository.Update(auditData);
            _gdprOperationsAuditRespository.Commit();
        }

        private GDPROperationsAudit CreateOperationAuditData(int userId, int customerId, string url)
        {
            var auditData = new GDPROperationsAudit()
            {
                User_Id = userId,
                Customer_Id = customerId,
                Operation = GDPROperations.DataPrivacy.ToString(),
                Parameters = "",
                Url = url,
                Application = ApplicationType.Helpdesk.ToString(),
                CreatedDate = DateTime.UtcNow,
                Status = GDPROperationStatus.None
            };

            _gdprOperationsAuditRespository.Add(auditData);
            _gdprOperationsAuditRespository.Commit();

            return auditData;
        }
            
        #endregion
    }
}
   