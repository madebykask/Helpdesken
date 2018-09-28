using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Models.Case.Options;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Services.Services.Concrete.Changes;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class CaseOptionsController : BaseApiController
    {

        private readonly IRegionService _regionService;
        private readonly IDepartmentService _departmentService;
        private readonly IOrganizationService _organizationService;
        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IImpactService _impactService;
        private readonly ISupplierService _supplierService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IUserService _userService;
        private readonly IPriorityService _priorityService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly IStatusService _statusService;
        private readonly IProjectService _projectService;
        private readonly IProblemService _problemService;
        private readonly IBaseChangesService _changeService;

        public CaseOptionsController(IRegionService regionService, IDepartmentService departmentService, IOrganizationService organizationService, 
            IRegistrationSourceCustomerService registrationSourceCustomerService, ISystemService systemService, IUrgencyService urgencyService,
            IImpactService impactService, ISupplierService supplierService, ICountryService countryService, ICurrencyService currencyService,
            IWorkingGroupService workingGroupService, IUserService userService, IPriorityService priorityService, IStateSecondaryService stateSecondaryService, 
            IStatusService statusService, IProjectService projectService, IProblemService problemService, IBaseChangesService changeService)
        {
            _regionService = regionService;
            _departmentService = departmentService;
            _organizationService = organizationService;
            _registrationSourceCustomerService = registrationSourceCustomerService;
            _systemService = systemService;
            _urgencyService = urgencyService;
            _impactService = impactService;
            _supplierService = supplierService;
            _countryService = countryService;
            _currencyService = currencyService;
            _workingGroupService = workingGroupService;
            _userService = userService;
            _priorityService = priorityService;
            _stateSecondaryService = stateSecondaryService;
            _statusService = statusService;
            _projectService = projectService;
            _problemService = problemService;
            _changeService = changeService;
        }

        [HttpPost]
        public async Task<CaseOptionsOutputModel> Bundle([FromUri]int cid, [FromBody]GetCaseOptionsInputModel input)
        {
            var model = new CaseOptionsOutputModel();
            model.Regions = await Task.FromResult(_regionService.GetRegions(cid)
                .Where(r => r.IsActive == 1)
                .Select(r => new ItemOverview(r.Id.ToString(), r.Name)).ToList());

            var userDeps = await Task.FromResult(_departmentService.GetDepartmentsByUserPermissions(UserId, cid));
            var customerDeps = await Task.FromResult(_departmentService.GetDepartments(cid));
            var departments = userDeps.Any()//TODO: check GetActiveDepartmentForUserByRegion
                ? userDeps
                : customerDeps
                    .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                    .ToList();
            model.Departments = departments
                    .Where(d => !input.RegionId.HasValue || d.Region_Id == input.RegionId.Value)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.DepartmentName)).ToList();

            model.OUs = await Task.FromResult(_organizationService.GetOUs(input.DepartmentId)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList());//TODO: See ChangeDepartments

            model.IsAboutDepartments = departments
                .Where(d => !input.IsAboutRegionId.HasValue || d.Region_Id == input.IsAboutRegionId.Value)
                .Select(d => new ItemOverview(d.Id.ToString(), d.DepartmentName)).ToList();

            model.IsAboutOUs = await Task.FromResult(_organizationService.GetOUs(input.IsAboutDepartmentId)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList());//TODO: See ChangeDepartments

            model.CustomerRegistrationSources =_registrationSourceCustomerService.GetCustomersActiveRegistrationSources(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.SourceName)).ToList();

            model.Systems = _systemService.GetSystems(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.SystemName)).ToList();

            model.Urgencies = _urgencyService.GetUrgencies(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            model.Impacts = _impactService.GetImpacts(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            model.Suppliers = _supplierService.GetSuppliers(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
            model.Countries = _countryService.GetCountries(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            model.Currencies = _currencyService.GetCurrencies()
                .Select(d => new ItemOverview(d.Code, d.Code)).ToList();//TODO: refactor get case to get currencyID instead on Code

            model.WorkingGroups = _workingGroupService.GetAllWorkingGroupsForCustomer(cid, false) //TODO: filter active
                .Select(d => new ItemOverview(d.Id.ToString(), d.WorkingGroupName)).ToList();

            model.ResponsibleUsers = _userService.GetAvailablePerformersOrUserId(cid, input.CaseResponsibleUserId)
                .Select(d => new ItemOverview(d.Id.ToString(), $"{d.FirstName} {d.SurName}")).ToList();//TODO: see responsibleUsersList.MapToSelectList

            //BusinessData.Models.User.CustomerUserInfo admUser = null;
            //if (m.case_.Performer_User_Id.HasValue)
            //{
            //    admUser = this._userService.GetUserInfo(m.case_.Performer_User_Id.Value);
            //}

            //var performersList = responsibleUsersList;
            //if (customerSetting.DontConnectUserToWorkingGroup == 0 && m.case_.WorkingGroup_Id > 0)
            //{
            //    performersList = this._userService.GetAvailablePerformersForWorkingGroup(customerId, m.case_.WorkingGroup_Id);
            //}

            //if (admUser != null && !performersList.Any(u => u.Id == admUser.Id) )
            //{
            //    performersList.Insert(0, admUser);
            //}

            model.Performers = model.ResponsibleUsers;// TODO: see above

            model.Priorities = _priorityService.GetPriorities(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            model.Statuses = _statusService.GetStatuses(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            model.StateSecondaries = _stateSecondaryService.GetStateSecondaries(cid)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
            //if (customerSetting.ModuleProject == 1)
            //{
                model.Projects = _projectService.GetCustomerProjects(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
            //}
            model.Problems =_problemService.GetCustomerProblems(cid, false)
                .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            //TODO: see GetCausingPartsModel(int customerId, int? curCausingPartId)

            //if (customerSetting.ModuleChangeManagement == 1)
            //{
            model.Changes = (await _changeService.GetChangesAsync(cid))
                .Select(d => new ItemOverview(d.Id.ToString(), d.ChangeTitle)).ToList();
            //}

            for (var i = 10; i < 110; i = i + 10)
            {
                model.SolutionsRates = new List<ItemOverview>();
                model.SolutionsRates.Add(new ItemOverview(i.ToString(), i.ToString()));
            }

            //TODO: CaseType_Id
            //TODO: ProductArea_Id
            //TODO: Category_Id

            return model;
        }
    }
}