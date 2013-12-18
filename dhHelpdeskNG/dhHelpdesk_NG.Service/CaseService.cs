using System.Collections.Generic;
using System.Linq;
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
            c.MOSS_DocUrl = " ";
            c.MOSS_DocUrlText = " ";
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

            errors = new Dictionary<string, string>();

            if (cases.Id == 0)
            {
                cases.RegTime = DateTime.UtcNow;
                _caseRepository.Add(cases);
            }
            else
            {
                cases.ChangeTime = DateTime.UtcNow;
                cases.ChangeByUser_Id = user.Id;
                _caseRepository.Update(cases);
            }

            if (errors.Count == 0)
                this.Commit();

            // save casehistory
            ret = SaveCaseHistory(cases, user, adUser, out errors);  
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
            h.Deleted = c.Deleted; 
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
            h.ProductAreaSetDate = c.ProductAreaSetDate;
            h.Project_Id = c.Project_Id;
            h.Problem_Id = c.Problem_Id; 
            h.ReferenceNumber = c.ReferenceNumber;
            h.RegistrationSource = c.RegistrationSource;
            h.RegLanguage_Id = c.RegLanguage_Id;
            h.RegUserDomain = c.RegUserDomain;
            h.RegUserId = c.RegUserId;
            h.RelatedCaseNumber = c.RelatedCaseNumber;
            h.ReportedBy = c.ReportedBy;
            h.Status_Id = c.Status_Id; 
            h.StateSecondary_Id = c.StateSecondary_Id; 
            h.SMS = c.SMS;
            h.SolutionRate = c.SolutionRate;
            h.Supplier_Id = c.Supplier_Id; 
            h.System_Id = c.System_Id; 
            h.UserCode = c.UserCode;
            h.User_Id = c.User_Id;
            h.Unread = c.Unread; 
            h.WatchDate = c.WatchDate;
            h.Verified = c.Verified;
            h.VerifiedDescription = c.VerifiedDescription; 
            h.WorkingGroup_Id = c.WorkingGroup_Id; 

            return h;
        }
    }
}
