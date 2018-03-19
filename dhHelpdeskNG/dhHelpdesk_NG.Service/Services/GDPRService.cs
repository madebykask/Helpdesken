using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Serializers;
using DH.Helpdesk.Dal.NewInfrastructure;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain;
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
    }

    public class GDPRService : IGDPROperationsService, IGDPRDataPrivacyAccessService, IGDPRFavoritesService
    {
        private readonly IGDPRDataPrivacyAccessRepository _privacyAccessRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IGDPROperationsAuditRespository _gdprOperationsAuditRespository;
        private readonly IJsonSerializeService _jsonSerializeService;
        private readonly IGDPRDataPrivacyFavortieRepository _gdprDataPrivacyFavortieRepository;

        #region ctor()

        public GDPRService(IGDPRDataPrivacyAccessRepository privacyAccessRepository,
                           IUnitOfWorkFactory unitOfWorkFactory,
                           IGDPROperationsAuditRespository operationsAuditRespository,
                           IGDPRDataPrivacyFavortieRepository dataPrivacyFavortieRepository,
                           IJsonSerializeService jsonSerializeService)
        {
            _gdprDataPrivacyFavortieRepository = dataPrivacyFavortieRepository;
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
            var favorites = _gdprDataPrivacyFavortieRepository.ListFavorites();
            return favorites;
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
                    var logFiles = uow.GetRepository<LogFile>();
                    var caseHistories = uow.GetRepository<CaseHistory>();
                    var emailLogs = uow.GetRepository<EmailLog>();

                    var casesQueryable = rep.GetAll().Where(x => x.Customer_Id == p.SelectedCustomerId);

                    if (p.ClosedOnly)
                        casesQueryable = casesQueryable.Where(x => x.FinishingDate.HasValue);

                    if (p.RegisterDateFrom.HasValue)
                        casesQueryable = casesQueryable.Where(x => x.RegTime >= p.RegisterDateFrom.Value);

                    if (p.RegisterDateTo.HasValue)
                        casesQueryable = casesQueryable.Where(x => x.RegTime <= p.RegisterDateTo.Value);

                    var cases = casesQueryable.ToList();
                    if (cases.Any())
                    {
                        casesIds = cases.Select(c => c.Id).ToList();
                        
                        ProcessReplaceCasesData(cases, caseFiles, logFiles, caseHistories, emailLogs, p);
                        uow.Save();
                    }
                }
            }
            catch (Exception e)
            {
                auditData.Success = false;
                auditData.Error = $"Unknown error. " + e.Message;
                SaveAuditOperationResult(auditData);
                return false;
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
            IRepository<CaseHistory> caseHistories,
            IRepository<EmailLog> emailLogs,
            DataPrivacyParameters p)
        {
            var replaceDataWith = p.ReplaceDataWith ?? string.Empty;
            var replaceDatesWith = p.ReplaceDatesWith;

            foreach (var c in cases)
            {
                foreach (var fieldName in p.FieldsNames)
                {
                    switch (fieldName)
                    {
                        #region initiator

                        case "ReportedBy":
                            c.ReportedBy = replaceDataWith;
                            break;
                        case "Persons_Name":
                            c.PersonsName = replaceDataWith; //first name last name?
                            break;
                        case "Persons_EMail":
                            c.PersonsEmail = replaceDataWith;
                            break;
                        case "Persons_Phone":
                            c.PersonsPhone = replaceDataWith;
                            break;
                        case "Persons_CellPhone":
                            c.PersonsCellphone = replaceDataWith;
                            break;
                        case "Region_Id":
                            c.Region_Id = null;
                            break;
                        case "Department_Id":
                            c.Department_Id = null;
                            break;
                        case "OU_Id":
                            c.OU_Id = null;
                            break;
                        case "CostCentre":
                            c.CostCentre = replaceDataWith;
                            break;
                        case "Place":
                            c.Place = replaceDataWith;
                            break;
                        case "UserCode":
                            c.UserCode = replaceDataWith;
                            break;

                        #endregion

                        #region regarding
                        case "IsAbout_ReportedBy":
                            if (c.IsAbout != null)
                                c.IsAbout.ReportedBy = replaceDataWith;
                            break;
                        case "IsAbout_Persons_Name":
                            if (c.IsAbout != null)
                                c.IsAbout.Person_Name = replaceDataWith; //first last name?
                            break;
                        case "IsAbout_Persons_EMail":
                            if (c.IsAbout != null)
                                c.IsAbout.Person_Email = replaceDataWith;
                            break;
                        case "IsAbout_Persons_Phone":
                            if (c.IsAbout != null)
                                c.IsAbout.Person_Phone = replaceDataWith;
                            break;
                        case "IsAbout_Persons_CellPhone":
                            if (c.IsAbout != null)
                                c.IsAbout.Person_Cellphone = replaceDataWith;
                            break;
                        case "IsAbout_Region_Id":
                            if (c.IsAbout != null)
                                c.IsAbout.Region_Id = null;
                            break;
                        case "IsAbout_Department_Id":
                            if (c.IsAbout != null)
                                c.IsAbout.Department_Id = null;
                            break;
                        case "IsAbout_OU_Id":
                            if (c.IsAbout != null)
                                c.IsAbout.OU_Id = null;
                            break;
                        case "IsAbout_CostCentre":
                            if (c.IsAbout != null)
                                c.IsAbout.CostCentre = replaceDataWith;
                            break;
                        case "IsAbout_Place":
                            if (c.IsAbout != null)
                                c.IsAbout.Place = replaceDataWith;
                            break;
                        case "IsAbout_UserCode":
                            if (c.IsAbout != null)
                                c.IsAbout.UserCode = replaceDataWith;
                            break;
                        #endregion

                        #region computer info
                        case "InventoryNumber":
                            c.InventoryNumber = replaceDataWith;
                            break;
                        case "ComputerType_Id":
                            c.InventoryType = replaceDataWith;
                            break;
                        case "InventoryLocation":
                            c.InventoryLocation = replaceDataWith;
                            break;
                        #endregion

                        #region case info
                        case "User_Id":
                            c.User_Id = null;
                            c.RegUserId = replaceDataWith;
                            c.RegUserDomain = replaceDataWith;
                            c.RegUserName = replaceDataWith;
                            c.IpAddress = replaceDataWith;
                            break;
                        case "RegistrationSourceCustomer":
                            c.RegistrationSourceCustomer_Id = null;
                            break;
                        case "ProductArea_Id":
                            c.ProductArea_Id = null;
                            break;
                        case "System_Id":
                            c.System_Id = null;
                            break;
                        case "Urgency_Id":
                            c.Urgency_Id = null;
                            break;
                        case "Impact_Id":
                            c.Impact_Id = null;
                            break;
                        case "Category_Id":
                            c.Category_Id = null;
                            break;
                        case "Supplier_Id":
                            c.Supplier_Id = null;
                            break;
                        case "InvoiceNumber":
                            c.InvoiceNumber = replaceDataWith;
                            break;
                        case "ReferenceNumber":
                            c.ReferenceNumber = replaceDataWith;
                            break;
                        case "Caption":
                            c.Caption = replaceDataWith;
                            break;
                        case "Description":
                            c.Description = replaceDataWith;
                            break;
                        case "Miscellaneous":
                            c.Miscellaneous = replaceDataWith;
                            break;
                        case "AgreedDate":
                            c.AgreedDate = replaceDatesWith;
                            break;
                        case "Available":
                            c.Available = replaceDataWith;
                            break;
                        case "Cost":
                            c.Cost = 0;
                            c.OtherCost = 0;
                            c.Currency = replaceDataWith;
                            break;
                        #endregion

                        #region case management
                        case "WorkingGroup_Id":
                            c.WorkingGroup_Id = null;
                            break;
                        case "CaseResponsibleUser_Id":
                            c.CaseResponsibleUser_Id = null;
                            break;
                        case "Performer_User_Id":
                            c.Performer_User_Id = null;
                            break;
                        case "Priority_Id":
                            c.Priority_Id = null;
                            break;
                        case "Status_Id":
                            c.Status_Id = null;
                            break;
                        case "StateSecondary_Id":
                            c.StateSecondary_Id = null;
                            break;
                        case "Project":
                            c.Project_Id = null;
                            break;
                        case "Problem":
                            c.Problem_Id = null;
                            break;
                        case "CausingPart":
                            c.CausingPartId = null;
                            break;
                        case "Change":
                            c.Change_Id = null;
                            break;
                        case "PlanDate":
                            c.PlanDate = replaceDatesWith;
                            break;
                        case "WatchDate":
                            c.WatchDate = replaceDatesWith;
                            break;
                        //case "Verified":
                        //    c.Verified = replaceDataWith;
                        //    break;
                        case "VerifiedDescription":
                            c.VerifiedDescription = replaceDataWith;
                            break;
                        case "SolutionRate":
                            c.SolutionRate = string.Empty;
                            break;
                        #endregion

                        #region case log
                        case "tblLog.Text_External":
                            foreach (var log in c.Logs.ToList())
                            {
                                log.Text_External = replaceDataWith;
                            }
                            break;
                        case "tblLog.Text_Internal":
                            foreach (var log in c.Logs.ToList())
                            {
                                log.Text_Internal = replaceDataWith;
                            }
                            break;
                        case "FinishingDescription":
                            c.FinishingDescription = replaceDataWith;
                            break;
                        case "ClosingReason":
                            foreach (var log in c.Logs.ToList())
                            {
                                log.FinishingType = null;
                            }
                            break;
                        case "FinishingDate":
                            c.FinishingDate = replaceDatesWith;
                            break;
                        #endregion
                    }

                    if (fieldName == AdditionalDataPrivacyFields.SelfService_RegUser.ToString())
                    {
                        c.Logs.ForEach(l => l.RegUser = replaceDataWith);
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

                if (p.RemoveCaseHistory && c.CaseHistories.Any())
                {
                    foreach (var caseHistory in c.CaseHistories.ToList())
                    {
                        emailLogs.DeleteWhere(x => x.CaseHistory_Id == caseHistory.Id);
                        caseHistories.Delete(caseHistory);
                    }
                }

                //if (replaceRegisteredBy)
                //{
                //    c.User_Id = null;
                //    c.RegUserId = replaceDataWith;
                //    c.RegUserDomain = replaceDataWith;
                //    c.RegUserName = replaceDataWith;
                //    c.IpAddress = replaceDataWith;
                //}
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
   