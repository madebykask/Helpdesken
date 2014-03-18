namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Notifiers;
    using DH.Helpdesk.BusinessData.Models.Notifiers.Input;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.Notifiers;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.BusinessModelFactories.Notifiers;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Infrastructure.Filters.Notifiers;
    using DH.Helpdesk.Web.Infrastructure.FiltersExtractors.Notifiers;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers;
    using DH.Helpdesk.Web.Models.Notifiers.Input;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public sealed class NotifiersController : BaseController
    {
        private const NotifierStatus ShowDefaultValue = NotifierStatus.Active;

        private const int RecordsOnPageDefaultValue = 500;

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

        private readonly IUpdatedFieldSettingsFactory updatedFieldSettingsInputModelToUpdatedFieldSettings;

        private readonly ISettingsModelFactory settingsModelFactory;

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
            IUpdatedFieldSettingsFactory updatedFieldSettingsInputModelToUpdatedFieldSettings, 
            ISettingsModelFactory settingsModelFactory)
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

            this.updatedFieldSettingsInputModelToUpdatedFieldSettings =
                updatedFieldSettingsInputModelToUpdatedFieldSettings;

            this.settingsModelFactory = settingsModelFactory;
        }

        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult Settings()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentLanguageId = SessionFacade.CurrentLanguageId;

            var settings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId,
                currentLanguageId);

            var languages = this.languageRepository.FindActive();
            var model = this.settingsModelFactory.Create(settings, languages, currentLanguageId);

            return this.PartialView(model);
        }

        [HttpGet]
        public JsonResult Captions(int languageId)
        {
            var captions = this.notifierService.GetSettingsCaptions(SessionFacade.CurrentCustomer.Id, languageId);
            var model = captions.Select(c => new CaptionModel(c.FieldName, c.Text)).ToList();
            return this.Json(model, JsonRequestBehavior.AllowGet);
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

            if (regionId.HasValue)
            {
                departments =
                    this.departmentRepository.FindActiveByCustomerIdAndRegionId(
                        SessionFacade.CurrentCustomer.Id, regionId.Value);
            }
            else
            {
                departments = this.departmentRepository.FindActiveOverviews(SessionFacade.CurrentCustomer.Id);
            }

            var model = new DropDownContent(departments.Select(d => new DropDownItem(d.Name, d.Value)).ToList());
            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentLanguageId = SessionFacade.CurrentLanguageId;

            var filters = SessionFacade.GetPageFilters<NotifiersFilter>(Enums.PageName.Notifiers);

            int? selectedDomainId = null;
            int? selectedRegionId = null;
            int? selectedDepartmentId = null;
            int? selectedDivisionId = null;
            string pharse = null;
            var show = ShowDefaultValue;
            var recordsOnPage = RecordsOnPageDefaultValue;

            if (filters != null)
            {
                selectedDomainId = filters.DomainId;
                selectedRegionId = filters.RegionId;
                selectedDepartmentId = filters.DepartmentId;
                selectedDivisionId = filters.DivisionId;
                pharse = filters.Pharse;
                show = filters.Status;
                recordsOnPage = filters.RecordsOnPage;
            }

            var fieldSettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId, currentLanguageId);

            List<ItemOverview> searchDomains = null;
            List<ItemOverview> searchRegions = null;
            List<ItemOverview> searchDepartments = null;
            List<ItemOverview> searchDivisions = null;

            if (fieldSettings.Domain.ShowInNotifiers)
            {
                searchDomains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (fieldSettings.Department.ShowInNotifiers)
            {
                searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);
                
                if (selectedRegionId.HasValue)
                {
                    searchDepartments = this.departmentRepository.FindActiveByCustomerIdAndRegionId(
                        currentCustomerId, selectedRegionId.Value);
                }
                else
                {
                    searchDepartments = this.departmentRepository.FindActiveOverviews(currentCustomerId);
                }
            }

            if (fieldSettings.Division.ShowInNotifiers)
            {
                searchDivisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            var parameters = new SearchParameters(
                currentCustomerId,
                selectedDomainId,
                selectedRegionId,
                selectedDepartmentId,
                selectedDivisionId,
                pharse,
                show,
                recordsOnPage,
                null);

            var searchResult = this.notifierRepository.Search(parameters);

            var model = this.indexModelFactory.Create(
                fieldSettings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                filters,
                ShowDefaultValue,
                RecordsOnPageDefaultValue,
                searchResult);

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewNotifier(InputModel model)
        {
            var newNotifier = new NewNotifier(
                SessionFacade.CurrentCustomer.Id,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.UserId),
                model.DomainId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.LoginName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.FirstName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Initials),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.LastName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.DisplayName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Place),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.CellPhone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Email),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Code),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.PostalAddress),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.PostalCode),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.City),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Title),
                model.DepartmentId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Unit),
                model.OrganizationUnitId,
                model.DivisionId,
                model.ManagerId,
                model.GroupId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Other),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Ordered),
                model.IsActive,
                DateTime.Now);

            this.notifierService.AddNotifier(newNotifier);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewNotifier()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var settings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(
                    currentCustomerId, SessionFacade.CurrentLanguageId);

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

            if (settings.Department.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);
                departments = this.departmentRepository.FindActiveOverviews(currentCustomerId);
            }

            if (settings.OrganizationUnit.Show)
            {
                organizationUnits = this.organizationUnitRepository.FindActiveAndShowable();
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

            var model = this.newNotifierModelFactory.Create(
                settings,
                domains,
                regions,
                departments,
                organizationUnits,
                divisions,
                managers,
                groups);
            
            return this.View(model);
        }

        [HttpGet]
        public ViewResult Notifier(int id)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var notifier = this.notifierRepository.FindNotifierDetailsById(id);

            var displaySettings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(
                    currentCustomerId, SessionFacade.CurrentLanguageId);

            List<ItemOverview> domains = null;
            List<ItemOverview> regions = null;
            List<ItemOverview> departments = null;
            List<ItemOverview> organizationUnits = null;
            List<ItemOverview> divisions = null;
            List<ItemOverview> managers = null;
            List<ItemOverview> groups = null;

            if (displaySettings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            // Begins urgent emergency fix.
            int? departmentRegionId = null;

            if (displaySettings.Department.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);

                if (notifier.DepartmentId.HasValue)
                {
                    var selectedDepartment = this.departmentRepository.GetById(notifier.DepartmentId.Value);
                    departmentRegionId = selectedDepartment.Region_Id;
                    
                    if (departmentRegionId.HasValue)
                    {
                        departments = this.departmentRepository.FindActiveByCustomerIdAndRegionId(
                            currentCustomerId, selectedDepartment.Region_Id.Value);
                    }
                    else
                    {
                        departments = this.departmentRepository.FindActiveOverviews(currentCustomerId);
                    }
                }
                else
                {
                    departments = this.departmentRepository.FindActiveOverviews(currentCustomerId);
                }
            }
            // Ends urgent emergency fix.

            if (displaySettings.OrganizationUnit.Show)
            {
                organizationUnits = this.organizationUnitRepository.FindActiveAndShowable();
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
            var updatedNotifier = new UpdatedNotifier(
                model.Id,
                model.DomainId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.LoginName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.FirstName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Initials),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.LastName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.DisplayName),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Place),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Phone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.CellPhone),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Email),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Code),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.PostalAddress),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.PostalCode),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.City),
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Title),
                model.DepartmentId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Unit),
                model.OrganizationUnitId,
                model.DivisionId,
                model.ManagerId,
                model.GroupId,
                ConfigurableFieldModel<string>.GetValueOrDefault(model.Other),
                ConfigurableFieldModel<bool>.GetValueOrDefault(model.Ordered),
                model.IsActive,
                DateTime.Now);

            this.notifierService.UpdateNotifier(updatedNotifier, SessionFacade.CurrentCustomer.Id);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Notifiers()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            // Incorrect model. Too many data.
            var displaySettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId, SessionFacade.CurrentLanguageId);

            var filters = SessionFacade.GetPageFilters<NotifiersFilter>(Enums.PageName.Notifiers);

            int? selectedDomainId = null;
            int? selectedRegionId = null;
            int? selectedDepartmentId = null;
            int? selectedDivisionId = null;
            string pharse = null;
            var status = ShowDefaultValue;
            var recordsOnPage = RecordsOnPageDefaultValue;

            if (filters != null)
            {
                selectedDomainId = filters.DomainId;
                selectedRegionId = filters.RegionId;
                selectedDepartmentId = filters.DepartmentId;
                selectedDivisionId = filters.DivisionId;
                pharse = filters.Pharse;
                status = filters.Status;
                recordsOnPage = filters.RecordsOnPage;
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

                if (selectedRegionId.HasValue)
                {
                    searchDepartments = this.departmentRepository.FindActiveByCustomerIdAndRegionId(
                        currentCustomerId, selectedRegionId.Value);
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
                selectedDomainId,
                selectedRegionId,
                selectedDepartmentId,
                selectedDivisionId,
                pharse,
                status,
                recordsOnPage,
                null);

            var searchResult = this.notifierRepository.Search(parameters);

            var model = this.notifiersModelFactory.Create(
                displaySettings, 
                searchDomains, 
                searchRegions, 
                searchDepartments, 
                searchDivisions, 
                filters,
                ShowDefaultValue, 
                RecordsOnPageDefaultValue, 
                searchResult);

            return this.PartialView(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult Search(SearchInputModel inputModel)
        {
            var filters = SearchModelFiltersExtractor.Extract(inputModel);
            SessionFacade.SavePageFilters(Enums.PageName.Notifiers, filters);

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var sortField = inputModel.SortField != null
                                ? new SortField(inputModel.SortField.Name, inputModel.SortField.SortBy)
                                : null;

            var parameters = new SearchParameters(
                currentCustomerId,
                inputModel.DomainId,
                inputModel.RegionId,
                inputModel.DepartmentId,
                inputModel.DivisionId,
                inputModel.Pharse,
                inputModel.Status,
                inputModel.RecordsOnPage,
                sortField);

            var searchResult = this.notifierRepository.Search(parameters);
            
            // Incorrect model. Too many data.
            var displaySettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId, SessionFacade.CurrentLanguageId);

            var model = this.notifiersGridModelFactory.Create(searchResult, displaySettings, inputModel.SortField);
            return this.PartialView("NotifiersGrid", model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Settings(SettingsInputModel model)
        {
            var updatedSettings = this.updatedFieldSettingsInputModelToUpdatedFieldSettings.Convert(
                model, DateTime.Now, SessionFacade.CurrentCustomer.Id);
            
            this.notifierService.UpdateSettings(updatedSettings);
            return this.RedirectToAction("Notifiers");
        }

        #endregion
    }
}