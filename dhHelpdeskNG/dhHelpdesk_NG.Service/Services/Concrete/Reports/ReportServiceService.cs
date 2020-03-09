using System;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.ReportService;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Dal.Repositories.ReportService;
using DH.Helpdesk.Domain;
using System.Collections.Generic;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Services.Services.Reports;

namespace DH.Helpdesk.Services.Services.Concrete.Reports
{   

    public sealed class ReportServiceService : IReportServiceService
    {
        #region Fields
        
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
            IDepartmentService departmentService,
            IOUService ouService,
            IWorkingGroupService workingGroupService,
            IUserService userService,
            ICaseTypeService caseTypeService,
            IProductAreaService productAreaService,
            IReportServiceRepository reportServiceRepository)
        {
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

            var administrators = _userService.GetAllPerformers(customerId).ToList();            
            reportFilter.Administrators = administrators;

            var departments = _departmentService.GetDepartmentsByUserPermissions(userId, customerId, false);
            if (!departments.Any())
                departments = _departmentService.GetDepartments(customerId, ActivationStatus.All);
            if (addOUsToDepartments)
                departments = AddOrganizationUnitsToDepartments(departments);

            var workingGroups = new List<WorkingGroupEntity>();
            var user = _userService.GetUser(userId);
            /*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */
            workingGroups = user.UserGroup_Id > UserGroups.Administrator ? 
                _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).ToList() :
                _workingGroupService.GetWorkingGroups(customerId, userId, false, true).ToList();
            
            var caseTypes = _caseTypeService.GetAllCaseTypes(customerId, false, true).ToList();
            var caseTypesInRow = _caseTypeService.GetChildrenInRow(caseTypes).ToList();

            var productAreas = _productAreaService.GetTopProductAreasWithChilds(customerId);
            var productAreasInRow = _productAreaService.GetChildrenInRow(productAreas).ToList();
    
            reportFilter.Departments = departments.ToList();
            reportFilter.WorkingGroups = workingGroups.ToList();
            reportFilter.CaseTypes = caseTypesInRow;
            reportFilter.ProductAreas = productAreasInRow;
            
            return reportFilter;
        }

        public ReportData GetReportData(string reportIdentity, ReportSelectedFilter filters, int userId, int customerId)
        {
            filters.SeletcedDepartments = EnsureDepartments(filters.SeletcedDepartments, filters.SeletcedOUs, userId, customerId);
            filters.SelectedWorkingGroups = EnsureWorkingGroups(filters.SelectedWorkingGroups, userId, customerId);

            return _reportServiceRepository.GetReportData(reportIdentity, filters);
        }

        public IList<HistoricalDataResult> GetHistoricalData(HistoricalDataFilter filter, int userId)
        {
            var user = _userService.GetUser(userId);

            filter.IncludeCasesWithNoWorkingGroup = CanSeeCasesWithEmptyWorkingGroups(filter.WorkingGroups, user);
            filter.IncludeCasesWithNoDepartments = false;
            filter.WorkingGroups = GetWorkingGroups(filter.WorkingGroups, filter.CustomerID, user);
            filter.Departments = GetDepartments(filter.Departments, filter.CustomerID, userId);

            var result = _reportServiceRepository.GetHistoricalData(filter);
            return result;
        }

        public IList<ReportedTimeDataResult> GetReportedTimeData(ReportedTimeDataFilter filter, int userId)
        {
            var user = _userService.GetUser(userId);

            filter.IncludeCasesWithNoWorkingGroup = CanSeeCasesWithEmptyWorkingGroups(filter.WorkingGroups, user);
            filter.IncludeCasesWithNoDepartments = false;
            filter.WorkingGroups = GetWorkingGroups(filter.WorkingGroups, filter.CustomerID, user);
            filter.Departments = GetDepartments(filter.Departments, filter.CustomerID, userId);

            var result = _reportServiceRepository.GetReportedTimeData(filter);
            return result;
        }

        public IList<NumberOfCaseDataResult> GetNumberOfCasesData(NumberOfCasesDataFilter filter, int userId)
        {
            if (filter.CaseTypes != null && filter.CaseTypes.Any())
            {
                var caseTypeChainIds = new List<int>(filter.CaseTypes);
                foreach (var caseTypeId in filter.CaseTypes)
                    caseTypeChainIds.AddRange(_caseTypeService.GetChildrenIds(caseTypeId));
                filter.CaseTypes = caseTypeChainIds;
            }
            if (filter.ProductAreas != null && filter.ProductAreas.Any())
            {
                var productAreasChainIds = new List<int>(filter.ProductAreas);
                foreach (var productAreaId in filter.ProductAreas)
                    productAreasChainIds.AddRange(_productAreaService.GetChildrenIds(productAreaId));
                filter.ProductAreas = productAreasChainIds;
            }

            var user = _userService.GetUser(userId);

            filter.IncludeCasesWithNoWorkingGroup = CanSeeCasesWithEmptyWorkingGroups(filter.WorkingGroups, user);
            filter.IncludeCasesWithNoDepartments = false;
            filter.WorkingGroups = GetWorkingGroups(filter.WorkingGroups, filter.CustomerID, user);
            filter.Departments = GetDepartments(filter.Departments, filter.CustomerID, userId);

            var result = _reportServiceRepository.GetNumberOfCasesData(filter);
            return result;
        }

        public IList<SolvedInTimeDataResult> GetSolvedInTimeData(SolvedInTimeDataFilter filter, int userId)
        {
            var user = _userService.GetUser(userId);

            filter.IncludeCasesWithNoWorkingGroup = CanSeeCasesWithEmptyWorkingGroups(filter.WorkingGroups, user);
            filter.IncludeCasesWithNoDepartments = false;
            filter.WorkingGroups = GetWorkingGroups(filter.WorkingGroups, filter.CustomerID, user);
            filter.Departments = GetDepartments(filter.Departments, filter.CustomerID, userId);

            var result = _reportServiceRepository.GetSolvedInTimeData(filter);
            return result;
        }

        #endregion

        #region Private Methods and Operators

        private List<int> GetWorkingGroups(List<int> filterWorkingGroups, int customerId, User user)
        {
            /*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */
            var workingGroups = user.UserGroup_Id > UserGroups.Administrator ?
                _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).Select(w => w.Id).ToList() :
                _workingGroupService.GetWorkingGroups(customerId, user.Id, false).Select(x => x.Id).ToList();

            if (filterWorkingGroups == null || !filterWorkingGroups.Any())
                return workingGroups;

            return filterWorkingGroups.Where(w => workingGroups.Contains(w)).ToList();
        }

        private bool CanSeeCasesWithEmptyWorkingGroups(IList<int> filterWorkingGroups,  User user)
        {
            return (filterWorkingGroups == null || !filterWorkingGroups.Any()) &&
                   (user.UserGroup_Id > UserGroups.Administrator || user.ShowNotAssignedWorkingGroups != 0);
        }

        private List<int> GetDepartments(List<int> filterDepartments, int customerId, int userId)
        {
            var departments = _departmentService.GetDepartmentsByUserPermissions(userId, customerId, false).Select(w => w.Id).ToList();

            if (filterDepartments == null || !filterDepartments.Any())
                return departments;

            return departments.Any() ? filterDepartments.Where(w => departments.Contains(w)).ToList() : filterDepartments;
        }

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

        private SelectedItems EnsureDepartments(SelectedItems departments, SelectedItems ous, int userId, int customerId)
        {
            if (departments.Any() || ous.Any())
                return departments;
            else
            {
                var allowedDepartmentIds = _departmentService.GetDepartmentsByUserPermissions(userId, customerId, false).Select(x => x.Id).ToList();
                return new SelectedItems(allowedDepartmentIds);                
            }
        }

        private SelectedItems EnsureWorkingGroups(SelectedItems workingGroups, int userId, int customerId)
        {
            var user = _userService.GetUser(userId);
            var ret = new SelectedItems();

            if (workingGroups.Any())
                ret = workingGroups;
            else
            {
                var allowedWorkingGroups = new List<int>();
                /*If user has no wg and is a SystemAdmin or customer admin, he/she can see all available wgs */

                if (user.UserGroup_Id > UserGroups.Administrator)
                    allowedWorkingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false).Select(w => w.Id).ToList();
                else
                {
                    allowedWorkingGroups = _workingGroupService.GetWorkingGroups(customerId, userId, false).Select(x => x.Id).ToList();
                    /* If allowed wg is empty, means user has no access to see any case. so we make a false condition */
                }
                ret.AddItems(allowedWorkingGroups);

                if (user.UserGroup_Id > UserGroups.Administrator || (user.ShowNotAssignedWorkingGroups != 0))
                    ret.Add(0); //Not assigned wg
            }

            

            return new SelectedItems(ret);            
        }

        #endregion

    }
}