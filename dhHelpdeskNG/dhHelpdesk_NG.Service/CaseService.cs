using System.Collections.Generic;
using System.Linq;
using System.Reflection; 
using System.Runtime.InteropServices;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.DTO.Utils; 
using System;

namespace dhHelpdesk_NG.Service
{
    public interface ICaseService
    {
        IList<Case> GetCases();
        IList<Case> GetCasesForStartPage(int customerId);
        Case InitCase(int customerId, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, Setting customerSetting, string adUser);
        Case GetCaseById(int id);
        IList<CaseHistory> GetCaseHistoryByCaseId(int caseId);
        int SaveCase(Case cases, User user, string adUser, out IDictionary<string, string> errors);
        int SaveCaseHistory(Case c, User user, string adUser, out IDictionary<string, string> errors);
        void Commit();
    }

    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly ICaseHistoryRepository _caseHistoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegionService _regionService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ISupplierService _supplierServicee;
        private readonly IPriorityService _priorityService;
        private readonly IStatusService _statusService;
        private readonly IWorkingGroupService _workingGroupService;

        public CaseService(
            ICaseRepository caseRepository,
            ICaseHistoryRepository caseHistoryRepository,
            IUnitOfWork unitOfWork,
            IRegionService regionService,
            ICaseTypeService caseTypeService, 
            ISupplierService supplierService, 
            IPriorityService priorityService,
            IStatusService statusService,
            IWorkingGroupService workingGroupService)
        {
            _unitOfWork = unitOfWork;
            _caseRepository = caseRepository;
            _caseRepository = caseRepository;
            _regionService = regionService;
            _caseTypeService = caseTypeService;
            _supplierServicee = supplierService;
            _priorityService = priorityService;
            _statusService = statusService;
            _workingGroupService = workingGroupService;
            _caseHistoryRepository = caseHistoryRepository; 
        }

        public Case GetCaseById(int id)
        {
            return _caseRepository.GetCaseById(id);
        }

        public Case InitCase(int customerId, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, Setting customerSetting, string adUser)
        {
            var c = new Case();

            c.Customer_Id = customerId;
            c.User_Id = userId;
            c.CaseResponsibleUser_Id = userId;
            c.IpAddress = ipAddress; 
            c.CaseGUID = Guid.NewGuid();
            c.RegLanguage_Id = languageId;
            c.RegistrationSource = (int)source;
            c.Deleted = 0;
            c.Region_Id = _regionService.GetDefaultId(customerId);
            c.CaseType_Id = _caseTypeService.GetDefaultId(customerId);
            c.Supplier_Id = _supplierServicee.GetDefaultId(customerId);
            c.Priority_Id = _priorityService.GetDefaultId(customerId);
            c.Status_Id = _statusService.GetDefaultId(customerId);
            c.WorkingGroup_Id = _workingGroupService.GetDefaultId(customerId);
            c.RegUserId =  adUser.GetUserFromAdPath();
            c.RegUserDomain = adUser.GetDomainFromAdPath();

            if (customerSetting != null)
            {
                if (customerSetting.SetUserToAdministrator == 1)
                    c.Performer_User_Id = userId;            
                else
                {
                    if (customerSetting.DefaultAdministrator.HasValue) 
                        c.Performer_User_Id = customerSetting.DefaultAdministrator.GetValueOrDefault();
                }
            }
            
            return c;
        }

        public IList<Case> GetCases()
        {
            return _caseRepository.GetAll().ToList();
        }

        public IList<Case> GetCasesForStartPage(int customerId) 
        {
            return _caseRepository.GetAll().Where(x => x.Customer_Id == customerId && x.Deleted == 0).ToList();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public int SaveCase(Case cases, User user, string adUser, out IDictionary<string, string> errors)
        {
            int ret = 0;

            if (cases == null)
                throw new ArgumentNullException("cases");

            Case c = ValidateCaseRequiredValues(cases); 

            errors = new Dictionary<string, string>();

            if (c.Id == 0)
            {
                c.RegTime = DateTime.UtcNow;
                _caseRepository.Add(c);
            }
            else
            {
                c.ChangeTime = DateTime.UtcNow;
                c.ChangeByUser_Id = user.Id;
                _caseRepository.Update(c);
            }

            if (errors.Count == 0)
                this.Commit();

            // save casehistory
            ret = SaveCaseHistory(c, user, adUser, out errors);  
            //return caseHistoryId
            return ret;
        }

        public int SaveCaseHistory(Case c, User user, string adUser, out IDictionary<string, string> errors)
        {
            if (c == null)
                throw new ArgumentNullException("caseHistory");

            errors = new Dictionary<string, string>();

            CaseHistory h = GenerateHistoryFromCase(c, user, adUser);
            _caseHistoryRepository.Add(h);

            if (errors.Count == 0)
                this.Commit();

            return h.Id;
        }

        public IList<CaseHistory> GetCaseHistoryByCaseId(int caseId)
        {
            return _caseHistoryRepository.GetCaseHistoryByCaseId(caseId).ToList(); 
        }

        private Case ValidateCaseRequiredValues(Case c)
        {
            Case ret = c;

            ret.PersonsCellphone = string.IsNullOrWhiteSpace(c.PersonsCellphone) ? string.Empty : c.PersonsCellphone;
            ret.PersonsEmail = string.IsNullOrWhiteSpace(c.PersonsEmail) ? string.Empty : c.PersonsEmail;
            ret.PersonsName = string.IsNullOrWhiteSpace(c.PersonsName) ? string.Empty : c.PersonsName;
            ret.PersonsPhone = string.IsNullOrWhiteSpace(c.PersonsPhone) ? string.Empty : c.PersonsPhone;
            ret.Place = string.IsNullOrWhiteSpace(c.Place) ? string.Empty : c.Place;
            ret.InventoryNumber = string.IsNullOrWhiteSpace(c.InventoryNumber) ? string.Empty : c.InventoryNumber;
            ret.InventoryType = string.IsNullOrWhiteSpace(c.InventoryType) ? string.Empty : c.InventoryType;
            ret.InventoryLocation = string.IsNullOrWhiteSpace(c.InventoryLocation) ? string.Empty : c.InventoryLocation;
            ret.InvoiceNumber = string.IsNullOrWhiteSpace(c.InvoiceNumber) ? string.Empty : c.InvoiceNumber;
            ret.Caption = string.IsNullOrWhiteSpace(c.Caption) ? string.Empty : c.Caption;
            ret.Description = string.IsNullOrWhiteSpace(c.Description) ? string.Empty : c.Description;
            ret.Miscellaneous = string.IsNullOrWhiteSpace(c.Miscellaneous) ? string.Empty : c.Miscellaneous;
            ret.Available = string.IsNullOrWhiteSpace(c.Available) ? string.Empty : c.Available;
            ret.MOSS_DocUrl = string.IsNullOrWhiteSpace(c.MOSS_DocUrl) ? string.Empty : c.MOSS_DocUrl;
            ret.MOSS_DocUrlText = string.IsNullOrWhiteSpace(c.MOSS_DocUrlText) ? string.Empty : c.MOSS_DocUrlText;

            return ret;
        }


        private CaseHistory GenerateHistoryFromCase(Case c, User user, string adUser)
        {
            CaseHistory h = new CaseHistory();

            h.AgreedDate = c.AgreedDate;
            h.ApprovedDate = c.AgreedDate;
            h.ApprovedBy_User_Id = c.ApprovedBy_User_Id; 
            h.Available = c.Available;
            h.Caption = c.Caption;
            h.Case_Id = c.Id;
            h.CaseHistoryGUID = Guid.NewGuid(); 
            h.CaseNumber = c.CaseNumber;
            h.CaseResponsibleUser_Id = c.CaseResponsibleUser_Id; 
            h.Category_Id = c.Category_Id; 
            h.Change_Id = c.Change_Id; 
            h.ContactBeforeAction = c.ContactBeforeAction;
            h.Cost = c.Cost;
            h.CreatedDate = DateTime.UtcNow;
            h.CreatedByUser = user.FirstName + ' ' + user.SurName; 
            h.Currency = c.Currency;
            h.Customer_Id = c.Customer_Id;
            h.Deleted = c.Deleted; 
            h.Department_Id = c.Department_Id;  
            h.Description = c.Description;
            h.ExternalTime = c.ExternalTime;
            h.FinishingDate = c.FinishingDate;
            h.FinishingDescription = c.FinishingDescription;
            h.FollowUpDate = c.FollowUpDate;
            h.Impact_Id = c.Impact_Id;
            h.InvoiceNumber = c.InvoiceNumber; 
            h.InventoryLocation = c.InventoryLocation;
            h.InventoryNumber = c.InventoryNumber;
            h.InventoryType = c.InventoryType;
            h.IpAddress = c.IpAddress;
            h.Miscellaneous = c.Miscellaneous;
            h.LockCaseToWorkingGroup_Id = c.LockCaseToWorkingGroup_Id; 
            h.MOSS_DocId = c.MOSS_DocId;
            h.MOSS_DocUrl = c.MOSS_DocUrl;
            h.MOSS_DocUrlText = c.MOSS_DocUrlText;
            h.MOSS_DocVersion = c.MOSS_DocVersion;
            h.OU_Id = c.OU_Id; 
            h.OtherCost = c.OtherCost;
            h.PersonsCellphone = c.PersonsCellphone;
            h.PersonsEmail = c.PersonsEmail;
            h.PersonsName = c.PersonsName;
            h.PersonsPhone = c.PersonsPhone;
            h.Place = c.Place;
            h.PlanDate = c.PlanDate;
            h.Priority_Id = c.Priority_Id; 
            h.ProductArea_Id = c.ProductArea_Id; 
            h.ProductAreaSetDate = c.ProductAreaSetDate;
            h.Project_Id = c.Project_Id;
            h.Problem_Id = c.Problem_Id; 
            h.ReferenceNumber = c.ReferenceNumber;
            h.RegistrationSource = c.RegistrationSource;
            h.RegLanguage_Id = c.RegLanguage_Id;
            h.RegUserDomain = adUser.GetDomainFromAdPath();
            h.RegUserId = adUser.GetUserFromAdPath(); 
            h.RelatedCaseNumber = c.RelatedCaseNumber;
            h.Region_Id = c.Region_Id, 
            h.ReportedBy = c.ReportedBy;
            h.Status_Id = c.Status_Id; 
            h.StateSecondary_Id = c.StateSecondary_Id; 
            h.SMS = c.SMS;
            h.SolutionRate = c.SolutionRate;
            h.Supplier_Id = c.Supplier_Id; 
            h.System_Id = c.System_Id; 
            h.UserCode = c.UserCode;
            h.User_Id = c.User_Id;
            h.Urgency_Id = c.Urgency_Id; 
            h.Unread = c.Unread; 
            h.WatchDate = c.WatchDate;
            h.Verified = c.Verified;
            h.VerifiedDescription = c.VerifiedDescription; 
            h.WorkingGroup_Id = c.WorkingGroup_Id; 

            return h;
        }
    }
}