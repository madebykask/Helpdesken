namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Enums;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.ActionFilters;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.ConfigurableFields;
    using DH.Helpdesk.Web.Infrastructure.Tools;

    public sealed class NotifiersController : BaseController
    {
        #region Fields

        private readonly IDepartmentRepository departmentRepository;

        private readonly IDivisionRepository divisionRepository;

        private readonly IDomainRepository domainRepository;

        private readonly IIndexModelFactory indexModelFactory;

        private readonly ILanguageRepository languageRepository;

        private readonly INewNotifierModelFactory newNotifierModelFactory;

        private readonly INotifierFieldSettingRepository notifierFieldSettingRepository;

        private readonly INotifierGroupRepository notifierGroupRepository;

        private readonly INotifierModelFactory notifierModelFactory;

        private readonly INotifierRepository notifierRepository;

        private readonly INotifierService notifierService;

        private readonly INotifiersGridModelFactory notifiersGridModelFactory;

        private readonly INotifiersModelFactory notifiersModelFactory;

        private readonly IOrganizationUnitRepository organizationUnitRepository;

        private readonly IRegionRepository regionRepository;

        private readonly IUpdatedSettingsFactory updatedSettingsFactory;

        private readonly ISettingsModelFactory settingsModelFactory;

        private readonly INewNotifierFactory newNotifierFactory;

        private readonly IUpdatedNotifierFactory updatedNotifierFactory;

        private readonly IOrganizationService organizationService;

        #endregion

        #region Public Methods and Operators

        public NotifiersController(
            IMasterDataService masterDataService,
            IDepartmentRepository departmentRepository,
            IDivisionRepository divisionRepository,
            IDomainRepository domainRepository,
            IIndexModelFactory indexModelFactory,
            ILanguageRepository languageRepository,
            INewNotifierModelFactory newNotifierModelFactory,
            INotifierFieldSettingRepository notifierFieldSettingRepository,
            INotifierGroupRepository notifierGroupRepository,
            INotifierRepository notifierRepository,
            INotifierModelFactory notifierModelFactory,
            INotifierService notifierService,
            INotifiersGridModelFactory notifiersGridModelFactory,
            INotifiersModelFactory notifiersModelFactory,
            IOrganizationUnitRepository organizationUnitRepository,
            IRegionRepository regionRepository,
            IUpdatedSettingsFactory updatedSettingsFactory,
            ISettingsModelFactory settingsModelFactory,
            IUpdatedNotifierFactory updatedNotifierFactory,
            INewNotifierFactory newNotifierFactory,
            IOrganizationService organizationService)
            : base(masterDataService)
        {
            this.departmentRepository = departmentRepository;
            this.divisionRepository = divisionRepository;
            this.domainRepository = domainRepository;
            this.indexModelFactory = indexModelFactory;
            this.languageRepository = languageRepository;
            this.newNotifierModelFactory = newNotifierModelFactory;
            this.notifierFieldSettingRepository = notifierFieldSettingRepository;
            this.notifierGroupRepository = notifierGroupRepository;
            this.notifierRepository = notifierRepository;
            this.notifierModelFactory = notifierModelFactory;
            this.notifierService = notifierService;
            this.notifiersGridModelFactory = notifiersGridModelFactory;
            this.notifiersModelFactory = notifiersModelFactory;
            this.organizationUnitRepository = organizationUnitRepository;
            this.regionRepository = regionRepository;

            this.updatedSettingsFactory = updatedSettingsFactory;

            this.settingsModelFactory = settingsModelFactory;
            this.updatedNotifierFactory = updatedNotifierFactory;
            this.newNotifierFactory = newNotifierFactory;
            this.organizationService = organizationService;
        }

        [HttpGet]
        public PartialViewResult Settings(int? languageId)
        {
            if (languageId == null)
            {
                languageId = SessionFacade.CurrentLanguageId;
            }

            var settings =
                this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                    SessionFacade.CurrentCustomer.Id,
                    languageId.Value);

            var languages = this.languageRepository.FindActiveOverviews();
            var model = this.settingsModelFactory.Create(settings, languages, languageId.Value);

            return this.PartialView(model);
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id)
        {
            this.notifierService.DeleteNotifier(id);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult DepartmentDropDown(int? regionId)
        {
            List<ItemOverview> departments;

            var departmentsData =
                    this.departmentRepository.GetActiveDepartmentsBy(SessionFacade.CurrentCustomer.Id, regionId).ToArray();

            departments = departmentsData.Select(d => new ItemOverview(d.DepartmentName, d.Id.ToString())).ToList();
            
            var model = new DropDownContent(departments.Select(d => new DropDownItem(d.Name, d.Value)).ToList());
            return this.PartialView(model);
        }

        [HttpGet]
        public PartialViewResult OrganizationUnitDropDown(int? departmentId)
        {
            List<ItemOverview> organizationUnits;           
            organizationUnits = organizationUnits = this.organizationService.GetOrganizationUnits(departmentId);

            var model = new DropDownContent(organizationUnits.Select(d => new DropDownItem(d.Name, d.Value)).ToList());
            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentLanguageId = SessionFacade.CurrentLanguageId;

            var filters = SessionFacade.FindPageFilters<NotifierFilters>(PageName.Notifiers);
            if (filters == null)
            {
                filters = NotifierFilters.CreateDefault();
                SessionFacade.SavePageFilters(PageName.Notifiers, filters);
            }

            var settings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId,
                currentLanguageId);
            if (settings.IsEmpty)
            {
                var empty = this.indexModelFactory.CreateEmpty();
                return this.View(empty);
            }

            List<ItemOverview> searchDomains = null;
            List<ItemOverview> searchRegions = null;
            List<ItemOverview> searchDepartments = null;
            List<ItemOverview> searchDivisions = null;

            if (settings.Domain.ShowInNotifiers)
            {
                searchDomains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (settings.Department.ShowInNotifiers)
            {
                searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);

                if (filters.RegionId.HasValue)
                {
                    searchDepartments = this.departmentRepository.FindActiveByCustomerIdAndRegionId(
                        currentCustomerId,
                        filters.RegionId.Value);
                }
                else
                {
                    searchDepartments = this.departmentRepository.FindActiveOverviews(currentCustomerId);
                }
            }

            if (settings.Division.ShowInNotifiers)
            {
                searchDivisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            var sortField = !string.IsNullOrEmpty(filters.SortByField)
                ? new SortField(filters.SortByField, filters.SortBy)
                : null;

            var parameters = new SearchParameters(
                currentCustomerId,
                filters.DomainId,
                filters.RegionId,
                filters.DepartmentId,
                filters.DivisionId,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage,
                sortField);

            var searchResult = this.notifierRepository.Search(parameters);

            var model = this.indexModelFactory.Create(
                settings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                filters,
                searchResult);

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewNotifier(InputModel model)
        {
            var newNotifier = this.newNotifierFactory.Create(model, SessionFacade.CurrentCustomer.Id, DateTime.Now);
            this.notifierService.AddNotifier(newNotifier);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public JsonResult NewNotifierPopup(InputModel model)
        {
            var newNotifier = this.newNotifierFactory.Create(model, SessionFacade.CurrentCustomer.Id, DateTime.Now);
            this.notifierService.AddNotifier(newNotifier);
            return new JsonResult { Data = newNotifier.Id };
        }

        [HttpGet]
        public ViewResult NewNotifierPopup(
            string userId,
            string fName,
            string email,
            string phone,
            string placement,
            string cellPhone, 
            int? regionId, 
            int? departmentId, 
            int? organizationUnitId)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var inputParams = new Dictionary<string,string>();

            var settings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(
                    currentCustomerId,
                    SessionFacade.CurrentLanguageId);

            List<ItemOverview> domains = null;
            List<ItemOverview> regions = null;
            List<ItemOverview> departments = null;
            List<ItemOverview> organizationUnits = null;
            List<ItemOverview> divisions = null;
            List<ItemOverview> managers = null;
            List<ItemOverview> groups = null;

            if (settings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (settings.Region.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);

                if (regionId != null && regionId.Value > 0)
                    inputParams.Add("RegionId", regionId.Value.ToString()); // Takes default from Case page
            }

            if (settings.Department.Show)
            {
                var departmentsData =
                    this.departmentRepository.GetActiveDepartmentsBy(currentCustomerId, regionId).ToArray();

                departments = departmentsData.Select(d => new ItemOverview(d.DepartmentName, d.Id.ToString())).ToList();

                if (departmentId != null && departmentId.Value > 0)
                    inputParams.Add("DepartmentId", departmentId.Value.ToString()); // Takes default from Case page                
            }
                       
            if (settings.OrganizationUnit.Show)
            {                
                organizationUnits = this.organizationService.GetOrganizationUnits(departmentId);

                if (organizationUnitId != null && organizationUnitId.Value > 0)
                    inputParams.Add("OrganizationUnitId", organizationUnitId.Value.ToString()); // Takes default from Case page                
            }

            if (settings.Division.Show)
            {
                divisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            if (settings.Manager.Show)
            {
                managers = this.notifierRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            if (settings.Group.Show)
            {
                groups = this.notifierGroupRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            if (!string.IsNullOrEmpty(userId))
                inputParams.Add("UserId", userId);

            if (!string.IsNullOrEmpty(fName))
               inputParams.Add("FName", fName);

            if (!string.IsNullOrEmpty(email))
                inputParams.Add("Email", email);

            if (!string.IsNullOrEmpty(phone))
                inputParams.Add("Phone", phone);

            if (!string.IsNullOrEmpty(placement))
                inputParams.Add("Placement", placement);

            if (!string.IsNullOrEmpty(cellPhone))
                inputParams.Add("CellPhone", cellPhone);
            ViewBag.displaySettings = settings;
            var model = this.newNotifierModelFactory.Create(
                settings,
                domains,
                regions,
                departments,
                organizationUnits,
                divisions,
                managers,
                groups,
                inputParams);
            
            return this.View(model);
        }

        [HttpGet]
        public ViewResult NewNotifier()
        {
            
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var inputParams = new Dictionary<string, string>();

            var settings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(
                    currentCustomerId,
                    SessionFacade.CurrentLanguageId);

            List<ItemOverview> domains = null;
            List<ItemOverview> regions = null;
            List<ItemOverview> departments = null;
            List<ItemOverview> organizationUnits = null;
            List<ItemOverview> divisions = null;
            List<ItemOverview> managers = null;
            List<ItemOverview> groups = null;

            int? regionId = null;
            int? departmentId = null;
            int? organizationUnitId = null;

            if (settings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (settings.Region.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);

                regionId = this.regionRepository.GetDefaultRegion(currentCustomerId);
                if (regionId != null && regionId.Value > 0)
                    inputParams.Add("RegionId", regionId.Value.ToString()); // Takes default from setting
            }

            if (settings.Department.Show)
            {
                var departmentsData =
                    this.departmentRepository.GetActiveDepartmentsBy(currentCustomerId, regionId).ToArray();

                departments = departmentsData.Select(d => new ItemOverview(d.DepartmentName, d.Id.ToString())).ToList();

                if (departmentId != null && departmentId.Value > 0)
                    inputParams.Add("DepartmentId", departmentId.Value.ToString()); // Takes default from setting
            }

            if (settings.OrganizationUnit.Show)
            {
                organizationUnits = this.organizationService.GetOrganizationUnits(departmentId);
                                                
                if (organizationUnitId != null && organizationUnitId.Value > 0)
                    inputParams.Add("OrganizationUnitId", organizationUnitId.Value.ToString()); // Takes default from setting
            }            

            if (settings.Division.Show)
            {
                divisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            if (settings.Manager.Show)
            {
                managers = this.notifierRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            if (settings.Group.Show)
            {
                groups = this.notifierGroupRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            ViewBag.displaySettings = settings;
            var model = this.newNotifierModelFactory.Create(
                settings,
                domains,
                regions,
                departments,
                organizationUnits,
                divisions,
                managers,
                groups,
                inputParams);

            return this.View(model);
        }

        [HttpGet]
        public ViewResult Notifier(int id)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var inputParams = new Dictionary<string, string>();
            var notifier = this.notifierRepository.FindNotifierDetailsById(id);

            var displaySettings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(
                    currentCustomerId,
                    SessionFacade.CurrentLanguageId);

            List<ItemOverview> domains = null;
            List<ItemOverview> regions = null;
            List<ItemOverview> departments = null;
            List<ItemOverview> organizationUnits = null;
            List<ItemOverview> divisions = null;
            List<ItemOverview> managers = null;
            List<ItemOverview> groups = null;

            int? departmentRegionId = null;
            int? departmentId = null;
            int? organizationUnitId = null;

            if (displaySettings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }
                        
            if (displaySettings.Region.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);

                // As each "Department" must has a "Region" in defination, we get notifier RegionId from Department.
                var selectedDepartment = this.departmentRepository.GetById(notifier.DepartmentId.Value);
                departmentRegionId = selectedDepartment.Region_Id;

                if (departmentRegionId != null && departmentRegionId.Value > 0) 
                    inputParams.Add("RegionId", departmentRegionId.Value.ToString()); // Takes default from saved Notifier
            }

            if (displaySettings.Department.Show)
            {
                var departmentsData =
                    this.departmentRepository.GetActiveDepartmentsBy(currentCustomerId, departmentRegionId).ToArray();

                departments = departmentsData.Select(d => new ItemOverview(d.DepartmentName, d.Id.ToString())).ToList();
                departmentId = notifier.DepartmentId;
                
                if (departmentId != null && departmentId.Value > 0)
                    inputParams.Add("DepartmentId", departmentId.Value.ToString()); // Takes default from saved Notifier
            }

            if (displaySettings.OrganizationUnit.Show)
            {
                organizationUnits = this.organizationService.GetOrganizationUnits(departmentId);
                organizationUnitId = notifier.OrganizationUnitId;

                if (organizationUnitId != null && organizationUnitId.Value > 0)
                    inputParams.Add("OrganizationUnitId", organizationUnitId.Value.ToString()); // Takes default from saved Notifier
            }            
            
            if (displaySettings.Division.Show)
            {
                divisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            if (displaySettings.Manager.Show)
            {
                managers = this.notifierRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            if (displaySettings.Group.Show)
            {
                groups = this.notifierGroupRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            ViewBag.displaySettings = displaySettings;
            ViewBag.organizationUnitId = notifier.OrganizationUnitId;
            var model = this.notifierModelFactory.Create(
                displaySettings,
                departmentRegionId,
                notifier,
                domains,
                regions,
                departments,
                organizationUnits,
                divisions,
                managers,
                groups);

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Notifier(InputModel model)
        {
            var updatedNotifier = this.updatedNotifierFactory.Create(model, DateTime.Now);
            this.notifierService.UpdateNotifier(updatedNotifier, SessionFacade.CurrentCustomer.Id);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Notifiers()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            // Incorrect model. Too many data.
            var displaySettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId,
                SessionFacade.CurrentLanguageId);

            var filters = SessionFacade.FindPageFilters<NotifierFilters>(PageName.Notifiers);
            if (filters == null)
            {
                filters = NotifierFilters.CreateDefault();
                SessionFacade.SavePageFilters(PageName.Notifiers, filters);
            }

            List<ItemOverview> searchDomains = null;
            List<ItemOverview> searchRegions = null;
            List<ItemOverview> searchDepartments = null;
            List<ItemOverview> searchDivisions = null;

            if (displaySettings.Domain.ShowInNotifiers)
            {
                searchDomains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (displaySettings.Department.ShowInNotifiers)
            {
                searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);

                if (filters.RegionId.HasValue)
                {
                    searchDepartments = this.departmentRepository.FindActiveByCustomerIdAndRegionId(
                        currentCustomerId,
                        filters.RegionId.Value);
                }
                else
                {
                    searchDepartments = this.departmentRepository.FindActiveOverviews(currentCustomerId);
                }
            }

            if (displaySettings.Division.ShowInNotifiers)
            {
                searchDivisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            var parameters = new SearchParameters(
                currentCustomerId,
                filters.DomainId,
                filters.RegionId,
                filters.DepartmentId,
                filters.DivisionId,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage,
                null);

            var searchResult = this.notifierRepository.Search(parameters);

            var model = this.notifiersModelFactory.Create(
                displaySettings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                filters,
                searchResult);

            return this.PartialView(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult Search(SearchInputModel inputModel)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var filters = inputModel.ExtractFilters();
            SessionFacade.SavePageFilters(PageName.Notifiers, filters);

            var sortField = !string.IsNullOrEmpty(inputModel.SortField.Name)
                ? new SortField(inputModel.SortField.Name, inputModel.SortField.SortBy.Value)
                : null;

            var parameters = new SearchParameters(
                currentCustomerId,
                filters.DomainId,
                filters.RegionId,
                filters.DepartmentId,
                filters.DivisionId,
                filters.Pharse,
                filters.Status,
                filters.RecordsOnPage,
                sortField);

            var searchResult = this.notifierRepository.Search(parameters);

            // Incorrect model. Too many data.
            var displaySettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId,
                SessionFacade.CurrentLanguageId);

            var model = this.notifiersGridModelFactory.Create(searchResult, displaySettings, inputModel.SortField);
            return this.PartialView("NotifiersGrid", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Settings(SettingsModel model)
        {
            var settings = this.updatedSettingsFactory.Create(model, SessionFacade.CurrentCustomer.Id, DateTime.Now);
            this.notifierService.UpdateSettings(settings);
            return this.RedirectToAction("Notifiers");
        }


        #endregion
    }
}