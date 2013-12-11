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
        Case InitCase(int customerId, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, Setting customerSetting);
        Case GetCaseById(int id);
        void SaveCase(Case cases, out IDictionary<string, string> errors);
        void Commit();
    }

    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegionService _regionService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ISupplierService _supplierServicee;
        private readonly IPriorityService _priorityService;
        private readonly IStatusService _statusService;
        private readonly IWorkingGroupService _workingGroupService;

        public CaseService(
            ICaseRepository caseRepository,
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
            _regionService = regionService;
            _caseTypeService = caseTypeService;
            _supplierServicee = supplierService;
            _priorityService = priorityService;
            _statusService = statusService;
            _workingGroupService = workingGroupService;
        }

        public Case GetCaseById(int id)
        {
            return _caseRepository.GetCaseById(id);
        }

        public Case InitCase(int customerId, int userId, int languageId, string ipAddress, GlobalEnums.RegistrationSource source, Setting customerSetting)
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

        public void SaveCase(Case cases, out IDictionary<string, string> errors)
        {
            if (cases == null)
                throw new ArgumentNullException("cases");

            errors = new Dictionary<string, string>();

            if (cases.Id == 0)
                _caseRepository.Add(cases);
            else
                _caseRepository.Update(cases);

            if (errors.Count == 0)
                this.Commit();
        }
    }
}