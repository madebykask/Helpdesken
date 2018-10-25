using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Models.Case.Options;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Concrete.Changes;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Translate;

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
        private readonly ISettingService _customerSettingsService;
        private readonly ICausingPartService _causingPartService;

        public CaseOptionsController(IRegistrationSourceCustomerService registrationSourceCustomerService, ISystemService systemService, IUrgencyService urgencyService,
            IImpactService impactService, ISupplierService supplierService, ICountryService countryService, ICurrencyService currencyService,
            IWorkingGroupService workingGroupService, IUserService userService, IPriorityService priorityService, IStateSecondaryService stateSecondaryService, 
            IStatusService statusService, IProjectService projectService, IProblemService problemService, IBaseChangesService changeService,
            ICausingPartService causingPartService, ISettingService customerSettingsService)
        {
            _causingPartService = causingPartService;
            _customerSettingsService = customerSettingsService;
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

        [HttpPost]//TODO: split this action to different controllers for modularity
        public async Task<CaseOptionsOutputModel> Bundle([FromUri]int cid, [FromBody]GetCaseOptionsInputModel input)
        {
            var customerId = cid;
            var model = new CaseOptionsOutputModel();

            var customerSettings = _customerSettingsService.GetCustomerSettings(customerId);

            if (input.CustomerRegistrationSources)
            {
                model.CustomerRegistrationSources =
                    _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId)
                        .Select(d => new ItemOverview(d.SourceName, d.Id.ToString()))
                        .ToList();
            }

            if (input.Systems)
            {
                model.Systems = _systemService.GetSystems(customerId).Select(d => new ItemOverview(d.SystemName, d.Id.ToString())).ToList();
            }

            if (input.Urgencies)
            {
                model.Urgencies = _urgencyService.GetUrgencies(customerId).Select(d => new ItemOverview(d.Name, d.Id.ToString())).ToList();
            }
            
            if (input.Impacts)
            {
                model.Impacts = _impactService.GetImpacts(customerId).Select(d => new ItemOverview(d.Name, d.Id.ToString())).ToList();
            }

            if (input.Suppliers)
            {
                model.Suppliers = 
                    _supplierService.GetSuppliers(customerId).Select(d => new ItemOverview(d.Name, d.Id.ToString())).ToList();

                model.Countries = 
                    _countryService.GetCountries(customerId).Select(d => new ItemOverview(d.Name, d.Id.ToString())).ToList();
            }

            if (input.Currencies)
            {
                model.Currencies = 
                    _currencyService.GetCurrencies().Select(d => new ItemOverview(d.Code, d.Code)).ToList(); //TODO: refactor get case to get currencyID instead on Code
            }

            if (input.WorkingGroups)
            {
                model.WorkingGroups =
                    _workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false) //TODO: filter active
                        .Select(d => new ItemOverview(d.WorkingGroupName, d.Id.ToString()))
                        .ToList();
            }

            if (input.ResponsibleUsers || input.Performers)
            {
                var responsibleUsers = _userService.GetAvailablePerformersOrUserId(customerId, input.CaseResponsibleUserId);

                if (input.ResponsibleUsers)
                {
                    model.ResponsibleUsers = responsibleUsers
                        .Select(d => new ItemOverview($"{d.FirstName} {d.SurName}", d.Id.ToString()))//TODO: see responsibleUsersList.MapToSelectList
                        .ToList();
                }

                if (input.Performers)
                {
                    CustomerUserInfo admUser = null;
                    var performerUserId = input.CasePerformerUserId ?? 0;
                    var caseWorkingGroupId = input.CaseWorkingGroupId ?? 0;
                    var performersList = responsibleUsers.ToList() as IList<CustomerUserInfo>;

                    if (performerUserId > 0)
                    {
                        admUser = this._userService.GetUserInfo(performerUserId);
                    }

                    if (customerSettings.DontConnectUserToWorkingGroup == 0 && input.CaseWorkingGroupId > 0)
                    {
                        performersList = _userService.GetAvailablePerformersForWorkingGroup(customerId, caseWorkingGroupId);
                    }

                    if (admUser != null && !performersList.Any(u => u.Id == admUser.Id))
                    {
                        performersList.Insert(0, admUser);
                    }

                    model.Performers = performersList.Where(it => it.IsActive == 1 && (it.Performer == 1 || it.Id == performerUserId))
                        .Select(u => new ItemOverview($"{u.FirstName} {u.SurName}", u.Id.ToString()))
                        .ToList();
                }
            }

            if (input.Priorities)
            {
                model.Priorities = _priorityService.GetPriorities(customerId)
                    .Select(d => new ItemOverview(d.Name, d.Id.ToString()))
                    .ToList();
            }

            if (input.Statuses)
            {
                model.Statuses = _statusService.GetStatuses(customerId)
                    .Select(d => new ItemOverview(d.Name, d.Id.ToString()))
                    .ToList();
            }

            if (input.StateSecondaries)
            {
                model.StateSecondaries = _stateSecondaryService.GetStateSecondaries(customerId)
                    .Select(d => new ItemOverview(d.Name, d.Id.ToString()))
                    .ToList();
            }

            //if (customerSetting.ModuleProject == 1)
            //{
            if (input.Projects)
            {
                model.Projects =
                    _projectService.GetCustomerProjects(customerId)
                        .Select(d => new ItemOverview(d.Name, d.Id.ToString()))
                        .ToList();
            }
            //}

            if (input.Problems)
            {
                model.Problems = _problemService.GetCustomerProblems(customerId, false)
                    .Select(d => new ItemOverview(d.Name, d.Id.ToString()))
                    .ToList();
            }
            
            if (input.CausingParts)
            {
                model.CausingParts = BuildCausingPartsList(customerId, input.CaseCausingPartId);
            }

            //if (customerSetting.ModuleChangeManagement == 1)
            //{
            if (input.Changes)
            {
                model.Changes = (await _changeService.GetChangesAsync(customerId))
                    .Select(d => new ItemOverview(d.ChangeTitle, d.Id.ToString()))
                    .ToList();
            }
            //}

            if (input.SolutionsRates)
            {
                model.SolutionsRates = new List<ItemOverview>();
                for (var i = 10; i < 110; i = i + 10)
                {
                    model.SolutionsRates.Add(new ItemOverview(i.ToString(), i.ToString()));
                }
            }

            return model;
        }

        //todo: a copy from Helpdesk.Web\CaseController.cs\GetCausingPartsModel Need to refactor to use one implementation
        private IList<ItemOverview> BuildCausingPartsList(int customerId, int? causingPartId)
        {
            var allActiveCausinParts = _causingPartService.GetActiveParentCausingParts(customerId, causingPartId);
            var ret = new List<ItemOverview>();

            var parentRet = new List<ItemOverview>();
            var childrenRet = new List<ItemOverview>();

            foreach (var causingPart in allActiveCausinParts)
            {
                if (causingPart.Parent != null && causingPartId.HasValue && causingPart.Id == causingPartId.Value)
                {
                    childrenRet.Add(new ItemOverview($"{Translation.Get(causingPart.Parent.Name)} - {Translation.Get(causingPart.Name)}", causingPart.Id.ToString()));
                }
                else
                {
                    if (causingPart.Children.Any())
                    {
                        foreach (var child in causingPart.Children)
                        {
                            if (child.IsActive)
                            {
                                //var isSelected = (child.Id == curCausingPartId);
                                childrenRet.Add(new ItemOverview($"{Translation.Get(causingPart.Name)} - {Translation.Get(child.Name)}", child.Id.ToString()));
                            }
                        }
                    }
                    else
                    {
                        //var isSelected = (causingPart.Id == curCausingPartId);
                        parentRet.Add(new ItemOverview(Translation.Get(causingPart.Name), causingPart.Id.ToString()));
                    }
                }
            }

            ret = parentRet.OrderBy(p => p.Name).Union(childrenRet.OrderBy(c => c.Name)).ToList();
            return ret.GroupBy(r => r.Value).Select(g => g.First()).ToList();
         }
    }
}