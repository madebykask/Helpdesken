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

        public CaseOptionsController(IRegistrationSourceCustomerService registrationSourceCustomerService, ISystemService systemService, IUrgencyService urgencyService,
            IImpactService impactService, ISupplierService supplierService, ICountryService countryService, ICurrencyService currencyService,
            IWorkingGroupService workingGroupService, IUserService userService, IPriorityService priorityService, IStateSecondaryService stateSecondaryService, 
            IStatusService statusService, IProjectService projectService, IProblemService problemService, IBaseChangesService changeService)
        {
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

            if (input.CustomerRegistrationSources)
            {
                model.CustomerRegistrationSources = _registrationSourceCustomerService
                    .GetCustomersActiveRegistrationSources(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.SourceName)).ToList();
            }

            if (input.Systems)
                model.Systems = _systemService.GetSystems(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.SystemName)).ToList();

            if (input.Urgencies)
                model.Urgencies = _urgencyService.GetUrgencies(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
            
            if (input.Impacts)
                model.Impacts = _impactService.GetImpacts(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            if (input.Suppliers)
            {
                model.Suppliers = _supplierService.GetSuppliers(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
                model.Countries = _countryService.GetCountries(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
            }

            if (input.Currencies)
                model.Currencies = _currencyService.GetCurrencies()
                    .Select(d => new ItemOverview(d.Code, d.Code)).ToList();//TODO: refactor get case to get currencyID instead on Code

            if (input.WorkingGroups)
                model.WorkingGroups = _workingGroupService.GetAllWorkingGroupsForCustomer(cid, false) //TODO: filter active
                    .Select(d => new ItemOverview(d.Id.ToString(), d.WorkingGroupName)).ToList();

            if (input.ResponsibleUsers)
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

            if (input.Performers)
                model.Performers = model.ResponsibleUsers;// TODO: see above

            if (input.Priorities)
                model.Priorities = _priorityService.GetPriorities(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            if (input.Statuses)
                model.Statuses = _statusService.GetStatuses(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            if (input.StateSecondaries)
                model.StateSecondaries = _stateSecondaryService.GetStateSecondaries(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
            //if (customerSetting.ModuleProject == 1)
            //{
            if (input.Projects)
                model.Projects = _projectService.GetCustomerProjects(cid)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();
            //}
            if (input.Problems)
                model.Problems =_problemService.GetCustomerProblems(cid, false)
                    .Select(d => new ItemOverview(d.Id.ToString(), d.Name)).ToList();

            //TODO: see GetCausingPartsModel(int customerId, int? curCausingPartId)

            //if (customerSetting.ModuleChangeManagement == 1)
            //{
            if (input.Changes)
                model.Changes = (await _changeService.GetChangesAsync(cid))
                    .Select(d => new ItemOverview(d.Id.ToString(), d.ChangeTitle)).ToList();
            //}

            if (input.SolutionsRates)
            {
                model.SolutionsRates = new List<ItemOverview>();
                for (var i = 10; i < 110; i = i + 10)
                    model.SolutionsRates.Add(new ItemOverview(i.ToString(), i.ToString()));
            }

            //TODO: CaseType_Id
            //TODO: ProductArea_Id
            //TODO: Category_Id

            return model;
        }
    }
}