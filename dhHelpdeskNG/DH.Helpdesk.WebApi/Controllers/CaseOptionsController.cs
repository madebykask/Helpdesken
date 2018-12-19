using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Models.Case.Options;
using DH.Helpdesk.Services.Enums;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.Services.Services.Concrete.Changes;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/caseoptions")]
    public class CaseOptionsController : BaseApiController
    {
        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IImpactService _impactService;
        private readonly ISupplierService _supplierService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserService _userService;
        private readonly IPriorityService _priorityService;
        private readonly IStatusService _statusService;
        private readonly IProjectService _projectService;
        private readonly IProblemService _problemService;
        private readonly IBaseChangesService _changeService;
        private readonly ISettingService _customerSettingsService;
        private readonly ICausingPartService _causingPartService;
        private readonly ITranslateCacheService _translateCacheService;
        private readonly IStateSecondaryService _stateSecondaryService;

        public CaseOptionsController(IRegistrationSourceCustomerService registrationSourceCustomerService, ISystemService systemService, IUrgencyService urgencyService,
            IImpactService impactService, ISupplierService supplierService, ICountryService countryService, ICurrencyService currencyService,
            IUserService userService, IPriorityService priorityService, IStateSecondaryService stateSecondaryService,
            IStatusService statusService, IProjectService projectService, IProblemService problemService, IBaseChangesService changeService,
            ICausingPartService causingPartService, ISettingService customerSettingsService, ITranslateCacheService translateCacheService)
        {
            _translateCacheService = translateCacheService;
            _causingPartService = causingPartService;
            _customerSettingsService = customerSettingsService;
            _registrationSourceCustomerService = registrationSourceCustomerService;
            _systemService = systemService;
            _urgencyService = urgencyService;
            _impactService = impactService;
            _supplierService = supplierService;
            _countryService = countryService;
            _currencyService = currencyService;
            _userService = userService;
            _priorityService = priorityService;
            _statusService = statusService;
            _projectService = projectService;
            _problemService = problemService;
            _changeService = changeService;
            _stateSecondaryService = stateSecondaryService;
        }

        /// <summary>
        /// Use to get bundle of options united in one request.
        /// Systems, Urgencies, Impacts, Suppliers, Currencies, Currencies, WorkingGroups, ResponsibleUsers,
        /// Performers, Priorities, Statuses, StateSecondaries, Projects, Problems, CausingParts, Changes, SolutionsRates
        /// </summary>
        /// <param name="cid">CustomerId</param>
        /// <param name="input">Input model</param>
        /// /// <param name="langId">Language Id</param>
        /// <returns></returns>
        [HttpPost]//TODO: split this action to different controllers for modularity
        [Route("bundle")]
        public async Task<CaseOptionsOutputModel> Bundle([FromUri]int cid, [FromBody]GetCaseOptionsInputModel input, [FromUri]int langId)
        {
            var customerId = cid;
            var languageId = langId;
            var model = new CaseOptionsOutputModel();

            var customerSettings = _customerSettingsService.GetCustomerSettings(customerId);

            if (input.CustomerRegistrationSources)
            {
                model.CustomerRegistrationSources =
                    _registrationSourceCustomerService.GetCustomersActiveRegistrationSources(customerId)
                        .Select(d => new ItemOverview(Translate(d.SourceName, languageId), d.Id.ToString()))
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

            if (input.ResponsibleUsers)
            {
                var responsibleUsers = _userService.GetAvailablePerformersOrUserId(customerId, input.CaseResponsibleUserId);
                model.ResponsibleUsers = customerSettings.IsUserFirstLastNameRepresentation == 1 ?
                    responsibleUsers
                        .OrderBy(it => it.FirstName).ThenBy(it => it.SurName)
                        .Select(d => new ItemOverview($"{d.FirstName} {d.SurName}", d.Id.ToString()))//TODO: see responsibleUsersList.MapToSelectList
                        .ToList() :
                    responsibleUsers
                        .OrderBy(it => it.SurName).ThenBy(it => it.FirstName)
                        .Select(d => new ItemOverview($"{d.SurName} {d.FirstName}", d.Id.ToString()))//TODO: see responsibleUsersList.MapToSelectList
                        .ToList();
            }

            if (input.Priorities)
            {
                model.Priorities = _priorityService.GetPriorities(customerId)
                    .Select(d => new ItemOverview(Translate(d.Name, languageId, TranslationTextTypes.MasterData), d.Id.ToString()))
                    .ToList();
            }

            if (input.Statuses)
            {
                model.Statuses = _statusService.GetStatuses(customerId)
                    .Select(d => new ItemOverview(Translate(d.Name, languageId, TranslationTextTypes.MasterData), d.Id.ToString()))
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
                model.CausingParts = BuildCausingPartsList(customerId, input.CaseCausingPartId, languageId);
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

        [HttpGet]
        [Route("statesecondaries")]
        public async Task<IList<ItemOverview>> Get([FromUri]int cid, [FromUri]int langId)
        {
            var items = await _stateSecondaryService.GetStateSecondariesAsync(cid).ConfigureAwait(false);
            return items
                .Select(d => new ItemOverview(Translate(d.Name, langId, TranslationTextTypes.MasterData), d.Id.ToString()))
                .ToList();
        }

        //todo: a copy from Helpdesk.Web\CaseController.cs\GetCausingPartsModel Need to refactor to use one implementation
        private IList<ItemOverview> BuildCausingPartsList(int customerId, int? causingPartId, int languageId)
        {
            var allActiveCausinParts = _causingPartService.GetActiveParentCausingParts(customerId, causingPartId);
            var ret = new List<ItemOverview>();

            var parentRet = new List<ItemOverview>();
            var childrenRet = new List<ItemOverview>();

            foreach (var causingPart in allActiveCausinParts)
            {
                if (causingPart.Parent != null && causingPartId.HasValue && causingPart.Id == causingPartId.Value)
                {
                    childrenRet.Add(new ItemOverview($"{Translate(causingPart.Parent.Name, languageId)} - {Translate(causingPart.Name, languageId)}", causingPart.Id.ToString()));
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
                                childrenRet.Add(new ItemOverview($"{Translate(causingPart.Name, languageId)} - {Translate(child.Name, languageId)}", child.Id.ToString()));
                            }
                        }
                    }
                    else
                    {
                        //var isSelected = (causingPart.Id == curCausingPartId);
                        parentRet.Add(new ItemOverview(Translate(causingPart.Name, languageId), causingPart.Id.ToString()));
                    }
                }
            }

            ret = parentRet.OrderBy(p => p.Name).Union(childrenRet.OrderBy(c => c.Name)).ToList();

            return ret.GroupBy(r => r.Value).Select(g => g.First()).ToList();
        }


        private string Translate(string translate, int languageId, int? tt = null)
        {
            return _translateCacheService.GetTextTranslation(translate, languageId, tt);
        }
    }
}