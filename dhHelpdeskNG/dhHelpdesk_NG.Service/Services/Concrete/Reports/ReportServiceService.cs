using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.ReportService;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Dal.Repositories.ReportService;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;
using System.Collections.Generic;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services.Reports;

namespace DH.Helpdesk.Services.Services.Concrete.Reports
{   

    public sealed class ReportServiceService : IReportServiceService
    {
        #region Fields
        
        private readonly ICustomerService _customerService;        
        private readonly IDepartmentService _departmentService;
        private readonly IOUService _ouService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IUserService _userService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly IProductAreaService _productAreaService;
        private readonly IReportServiceRepository _reportServiceRepository;

        #endregion

        #region Constructors and Destructors

        public ReportServiceService(
            ICustomerService customerService,            
            IDepartmentService departmentService,
            IOUService ouService,
            IWorkingGroupService workingGroupService,
            IUserService userService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            IReportServiceRepository reportServiceRepository)
        {
            this._customerService = customerService;            
            this._departmentService = departmentService;
            this._ouService = ouService;
            this._workingGroupService = workingGroupService;
            this._userService = userService;
            this._caseTypeService = caseTypeService;
            this._productAreaService = productAreaService;
            this._reportServiceRepository = reportServiceRepository;            
        }

        #endregion

        #region Public Methods and Operators

        public ReportFilter GetReportFilter(int customerId, int userId, bool addOUsToDepartments = true)
        {
            var reportFilter = new ReportFilter();                        
            reportFilter.CaseCreationDate = new DateToDate(DateTime.Now.AddMonths(-1), DateTime.Now);

            var userSearch = new UserSearch() { CustomerId = customerId, StatusId = 3 };
            var administrators = this._userService.SearchSortAndGenerateUsers(userSearch).ToList();            
            reportFilter.Administrators = administrators;

            var departments = this._departmentService.GetDepartments(customerId, ActivationStatus.All);
            if (addOUsToDepartments)
                departments = AddOrganizationUnitsToDepartments(departments);

            var workingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).ToList();                

            var caseTypes = this._caseTypeService.GetCaseTypes(customerId).ToList();
            var caseTypesInRow = this._caseTypeService.GetChildsInRow(caseTypes).ToList();

            var productAreas = this._productAreaService.GetAllProductAreas(customerId);
            var productAreasInRow = this._productAreaService.GetChildsInRow(productAreas).ToList();
    
            reportFilter.Departments = departments.ToList();
            reportFilter.WorkingGroups = workingGroups;
            reportFilter.CaseTypes = caseTypesInRow;
            reportFilter.ProductAreas = productAreasInRow;
            
            return reportFilter;
        }
        
        public ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters)
        {
            return _reportServiceRepository.GetReportData(reportIdentity, filters);
        }

        #endregion

        #region Private Methods and Operators

        private IList<Department> AddOrganizationUnitsToDepartments(IList<Department> departments)
        {
            if (departments.Any())
            {
                var allOUsNeeded = this._ouService.GetAllOUs()
                                                  .Where(o => o.Department_Id.HasValue)
                                                  .Where(o => departments.Select(d => d.Id).ToList().Contains(o.Department_Id.Value))
                                                  .ToList();
                var dep_Ou = new List<Department>();
                foreach (var dep in departments)
                {
                    var currentOUsForDep = allOUsNeeded.Where(o => o.Department_Id == dep.Id).ToList();
                    foreach (var o in currentOUsForDep)
                    {
                        /*Attention: As we make combination of Department and OU list in a one filter in case overview, 
                        we use -OU.id (Id with minus sign) to detect it is OU.Id (Not Department.Id) and 
                        then for fetching data we convert Negative to positive, Also they are storing Negative in the Session*/
                        var newDep = new Department()
                        {
                            Id = -o.Id,
                            DepartmentName = dep.DepartmentName + " - " + (o.Parent_OU_Id.HasValue ? o.Parent.Name + " - " + o.Name : o.Name),
                            IsActive = 1
                        };
                        dep_Ou.Add(newDep);
                    }
                }

                foreach (var newDep_OU in dep_Ou.ToList())
                    departments.Add(newDep_OU);

                departments = departments.OrderBy(d => d.DepartmentName).ToList();
            }

            return departments;
        }        

        #endregion

    }
}