namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Input;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    using NotifierInputModel = dhHelpdesk_NG.Web.Models.Notifiers.Input.NotifierInputModel;

    public sealed class NotifiersController : BaseController
    {
        #region Fields

        private readonly IDepartmentRepository departmentRepository;

        private readonly IDivisionRepository divisionRepository;

        private readonly IDomainRepository domainRepository;

        private readonly IIndexModelFactory indexModelFactory;

        private readonly ILanguageRepository languageRepository;

        private readonly INewNotifierModelFactory newNotifierModelFactory;

        private readonly INotifierFieldSettingLanguageRepository notifierFieldSettingLanguageRepository;

        private readonly INotifierFieldSettingRepository notifierFieldSettingRepository;

        private readonly INotifierGroupRepository notifierGroupRepository;

        private readonly INotifierModelFactory notifierModelFactory;

        private readonly INotifierRepository notifierRepository;

        private readonly INotifierService notifierService;

        private readonly INotifiersGridModelFactory notifiersGridModelFactory;

        private readonly INotifiersModelFactory notifiersModelFactory;

        private readonly IOrganizationUnitRepository organizationUnitRepository;

        private readonly IRegionRepository regionRepository;

        private readonly ISettingsInputModelToUpdatedFieldSettingsConverter settingsInputModelToSettings;

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
            INotifierFieldSettingLanguageRepository notifierFieldSettingLanguageRepository,
            INotifierGroupRepository notifierGroupRepository,
            INotifierRepository notifierRepository,
            INotifierModelFactory notifierModelFactory,
            INotifierService notifierService,
            INotifiersGridModelFactory notifiersGridModelFactory,
            INotifiersModelFactory notifiersModelFactory,
            IOrganizationUnitRepository organizationUnitRepository,
            IRegionRepository regionRepository,
            ISettingsInputModelToUpdatedFieldSettingsConverter settingsInputModelToSettings)
            : base(masterDataService)
        {
            this.departmentRepository = departmentRepository;
            this.divisionRepository = divisionRepository;
            this.domainRepository = domainRepository;
            this.indexModelFactory = indexModelFactory;
            this.languageRepository = languageRepository;
            this.newNotifierModelFactory = newNotifierModelFactory;
            this.notifierFieldSettingRepository = notifierFieldSettingRepository;
            this.notifierFieldSettingLanguageRepository = notifierFieldSettingLanguageRepository;
            this.notifierGroupRepository = notifierGroupRepository;
            this.notifierRepository = notifierRepository;
            this.notifierModelFactory = notifierModelFactory;
            this.notifierService = notifierService;
            this.notifiersGridModelFactory = notifiersGridModelFactory;
            this.notifiersModelFactory = notifiersModelFactory;
            this.organizationUnitRepository = organizationUnitRepository;
            this.regionRepository = regionRepository;
            this.settingsInputModelToSettings = settingsInputModelToSettings;
        }

        [HttpGet]
        public JsonResult Captions(int languageId)
        {
            var captions =
                this.notifierFieldSettingLanguageRepository.FindByCustomerIdAndLanguageId(
                    SessionFacade.CurrentCustomer.Id, languageId);

            var model = captions.Select(c => new CaptionModel(c.FieldName, c.Text)).ToList();
            return this.Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void Delete(int id)
        {
            this.notifierRepository.DeleteById(id);
            this.notifierRepository.Commit();
        }

        [HttpGet]
        public PartialViewResult DepartmentDropDown(int? regionId)
        {
            List<ItemOverviewDto> departments;

            if (regionId.HasValue)
            {
                departments =
                    this.departmentRepository.FindActiveByCustomerIdAndRegionId(
                        SessionFacade.CurrentCustomer.Id, regionId.Value);
            }
            else
            {
                departments = this.departmentRepository.FindActiveByCustomerId(SessionFacade.CurrentCustomer.Id);
            }

            var model = new DropDownContent(departments.Select(d => new DropDownItem(d.Name, d.Value)).ToList());
            return this.PartialView(model);
        }

        [HttpGet]
        public ViewResult Index()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentLanguageId = SessionFacade.CurrentLanguage;

            var notifiers = this.notifierRepository.FindDetailedOverviewsByCustomerId(currentCustomerId);

            var fieldSettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId, currentLanguageId);

            List<ItemOverviewDto> searchDomains = null;
            List<ItemOverviewDto> searchRegions = null;
            List<ItemOverviewDto> searchDepartments = null;
            List<ItemOverviewDto> searchDivisions = null;

            if (fieldSettings.Domain.ShowInNotifiers)
            {
                searchDomains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (fieldSettings.Department.ShowInNotifiers)
            {
                searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);
                searchDepartments = this.departmentRepository.FindActiveByCustomerId(currentCustomerId);
            }

            if (fieldSettings.Division.ShowInNotifiers)
            {
                searchDivisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            var languages = this.languageRepository.FindActive();

            var model = this.indexModelFactory.Create(fieldSettings, 
                searchDomains, 
                searchRegions, 
                searchDepartments, 
                searchDivisions, 
                Enums.Show.Active, 
                500, 
                notifiers, languages, currentLanguageId);

            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult NewNotifier(NewNotifierInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var newNotifier = new NewNotifierDto(
                SessionFacade.CurrentCustomer.Id, 
                inputModel.UserId, 
                inputModel.DomainId, 
                inputModel.LoginName, 
                inputModel.FirstName, 
                inputModel.Initials, 
                inputModel.LastName, 
                inputModel.DisplayName, 
                inputModel.Place, 
                inputModel.Phone, 
                inputModel.CellPhone, 
                inputModel.Email, 
                inputModel.Code, 
                inputModel.PostalAddress, 
                inputModel.PostalCode, 
                inputModel.City, 
                inputModel.Title, 
                inputModel.DepartmentId, 
                inputModel.Unit, 
                inputModel.OrganizationUnitId, 
                inputModel.DivisionId, 
                inputModel.ManagerId, 
                inputModel.GroupId, 
                inputModel.Password, 
                inputModel.Other, 
                inputModel.Ordered, 
                inputModel.IsActive, 
                DateTime.Now);

            this.notifierService.AddNotifier(newNotifier);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult NewNotifier()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var displaySettings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(
                    currentCustomerId, SessionFacade.CurrentLanguage);

            List<ItemOverviewDto> domains = null;
            List<ItemOverviewDto> regions = null;
            List<ItemOverviewDto> departments = null;
            List<ItemOverviewDto> organizationUnits = null;
            List<ItemOverviewDto> divisions = null;
            List<ItemOverviewDto> managers = null;
            List<ItemOverviewDto> groups = null;

            if (displaySettings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (displaySettings.Department.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);
                departments = this.departmentRepository.FindActiveByCustomerId(currentCustomerId);
            }

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

            var model = this.newNotifierModelFactory.Create(
                displaySettings, domains, regions, departments, organizationUnits, divisions, managers, groups);

            return this.View(model);
        }

        [HttpGet]
        public ViewResult Notifier(int id)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var notifier = this.notifierRepository.FindNotifierDetailsById(id);

            var displaySettings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(
                    currentCustomerId, SessionFacade.CurrentLanguage);

            List<ItemOverviewDto> domains = null;
            List<ItemOverviewDto> regions = null;
            List<ItemOverviewDto> departments = null;
            List<ItemOverviewDto> organizationUnits = null;
            List<ItemOverviewDto> divisions = null;
            List<ItemOverviewDto> managers = null;
            List<ItemOverviewDto> groups = null;

            if (displaySettings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (displaySettings.Department.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);
                departments = this.departmentRepository.FindActiveByCustomerId(currentCustomerId);
            }

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
                displaySettings, notifier, domains, regions, departments, organizationUnits, divisions, managers, groups);

            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult Notifier(NotifierInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var updatedNotifier = new UpdatedNotifierDto(
                inputModel.Id, 
                inputModel.DomainId, 
                inputModel.LoginName, 
                inputModel.FirstName, 
                inputModel.Initials, 
                inputModel.LastName, 
                inputModel.DisplayName, 
                inputModel.Place, 
                inputModel.Phone, 
                inputModel.CellPhone, 
                inputModel.Email, 
                inputModel.Code, 
                inputModel.PostalAddress, 
                inputModel.PostalCode, 
                inputModel.City, 
                inputModel.Title, 
                inputModel.DepartmentId, 
                inputModel.Unit, 
                inputModel.OrganizationUnitId, 
                inputModel.DivisionId, 
                inputModel.ManagerId, 
                inputModel.GroupId, 
                inputModel.Password, 
                inputModel.Other, 
                inputModel.Ordered, 
                inputModel.IsActive, 
                DateTime.Now);

            this.notifierService.UpdateNotifier(updatedNotifier, SessionFacade.CurrentCustomer.Id);
            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public PartialViewResult Notifiers()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var notifiers = this.notifierRepository.FindDetailedOverviewsByCustomerId(currentCustomerId);

            // Incorrect model. Too many data.
            var displaySettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId, SessionFacade.CurrentLanguage);

            List<ItemOverviewDto> searchDomains = null;
            List<ItemOverviewDto> searchRegions = null;
            List<ItemOverviewDto> searchDepartments = null;
            List<ItemOverviewDto> searchDivisions = null;

            if (displaySettings.Domain.ShowInNotifiers)
            {
                searchDomains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (displaySettings.Department.ShowInNotifiers)
            {
                searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);
                searchDepartments = this.departmentRepository.FindActiveByCustomerId(currentCustomerId);
            }

            if (displaySettings.Division.ShowInNotifiers)
            {
                searchDivisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            var model = this.notifiersModelFactory.Create(
                displaySettings, 
                searchDomains, 
                searchRegions, 
                searchDepartments, 
                searchDivisions, 
                Enums.Show.Active, 
                500, 
                notifiers);

            return this.PartialView(model);
        }

        [HttpPost]
        public PartialViewResult Search(SearchInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var notifiers = this.notifierRepository.SearchDetailedOverviews(
                currentCustomerId, 
                inputModel.DomainId, 
                inputModel.DepartmentId, 
                inputModel.DivisionId, 
                inputModel.Pharse, 
                (EntityStatus)inputModel.Show, 
                inputModel.RecordsOnPage);

            // Incorrect model. Too many data.
            var displaySettings = this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId, SessionFacade.CurrentLanguage);

            var model = this.notifiersGridModelFactory.Create(notifiers, displaySettings);
            return this.PartialView("NotifiersGrid", model);
        }

        [HttpPost]
        public RedirectToRouteResult Settings(SettingsInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var updatedSettings = this.settingsInputModelToSettings.Convert(
                inputModel, DateTime.Now, SessionFacade.CurrentCustomer.Id);

            this.notifierFieldSettingRepository.UpdateSettings(updatedSettings);
            this.notifierFieldSettingRepository.Commit();
            return this.RedirectToAction("Notifiers");
        }

        #endregion
    }
}