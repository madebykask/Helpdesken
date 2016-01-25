using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.ReportService;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.ReportService;
using DH.Helpdesk.Common.Extensions.Integer;

namespace DH.Helpdesk.Services.Services.Concrete
{   

    public sealed class ReportServiceService : IReportServiceService
    {
        #region Fields
        
        private readonly ICustomerService _customerService;        
        private readonly IDepartmentService _departmentService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IUserService _userService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IReportServiceRepository _reportServiceRepository;

        #endregion

        #region Constructors and Destructors

        public ReportServiceService(
            ICustomerService customerService,            
            IDepartmentService departmentService,
            IWorkingGroupService workingGroupService,
            IUserService userService,
            ICaseTypeService caseTypeService,
            IReportServiceRepository reportServiceRepository)
        {
            this._customerService = customerService;            
            this._departmentService = departmentService;
            this._workingGroupService = workingGroupService;
            this._userService = userService;
            this._caseTypeService = caseTypeService;
            this._reportServiceRepository = reportServiceRepository;            
        }

        #endregion

        #region Public Methods and Operators

        public ReportFilter GetReportFilter(int defaultCustomerId, int? selectedCustomerId = null)
        {
            var reportFilter = new ReportFilter();            

            var customers = this._customerService.GetAllCustomers().ToList();
            reportFilter.Customers = customers;
            reportFilter.CaseCreationDate = new DateToDate(DateTime.Now.AddMonths(-1), DateTime.Now);

            var administrators = this._userService.GetUsers(defaultCustomerId).ToList();
            reportFilter.Administrators = administrators;

            if (selectedCustomerId.HasValue)
            {
                var departments = this._departmentService.GetDepartments(selectedCustomerId.Value).ToList();
                var workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(selectedCustomerId.Value).ToList();                
                var caseTypes = this._caseTypeService.GetCaseTypes(selectedCustomerId.Value).ToList();
                
                reportFilter.Departments = departments;
                reportFilter.WorkingGroups = workingGroups;
                reportFilter.CaseTypes = caseTypes;
            }
            else
            {                
                reportFilter.Departments = null;
                reportFilter.WorkingGroups = null;
                reportFilter.CaseTypes = null;
            }
            
            return reportFilter;
        }

        public ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters)
        {
            return _reportServiceRepository.GetReportData(reportIdentity, filters);
        }

        #endregion


    }
}