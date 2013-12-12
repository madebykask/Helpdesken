namespace dhHelpdesk_NG.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Repositories;
    using dhHelpdesk_NG.Data.Repositories.Notifiers;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Service;
    using dhHelpdesk_NG.Service.WorkflowModels.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure;
    using dhHelpdesk_NG.Web.Infrastructure.Converters;
    using dhHelpdesk_NG.Web.Infrastructure.Converters.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers;
    using dhHelpdesk_NG.Web.Models.Notifiers.Input;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NotifiersController : BaseController
    {
        #region Fields

        private readonly INotifierModelFactory notifierModelFactory;

        private readonly IDepartmentRepository departmentRepository;

        private readonly IDivisionRepository divisionRepository;

        private readonly IDomainRepository domainRepository;

        private readonly IIndexModelFactory indexModelFactory;

        private readonly INewNotifierModelFactory newNotifierModelFactory;

        private readonly INotifierFieldsSettingsRepository notifierFieldsSettingsRepository;

        private readonly INotifierGroupsRepository notifierGroupsRepository;

        private readonly INotifierService notifierService;

        private readonly INotifiersGridModelFactory notifiersGridModelFactory;

        private readonly INotifiersModelFactory notifiersModelFactory;

        private readonly INotifiersRepository notifiersRepository;

        private readonly ISettingsInputModelToUpdatedFieldsSettingsDtoConverter settingsInputModelToSettingsDto;

        private readonly IOrganizationUnitRepository organizationUnitRepository;

        private readonly IRegionRepository regionRepository;

        #endregion

        #region Public Methods and Operators

        public NotifiersController(
            IMasterDataService masterDataService,
            IDepartmentRepository departmentRepository,
            IDivisionRepository divisionRepository,
            IDomainRepository domainRepository,
            IIndexModelFactory indexModelFactory,
            INewNotifierModelFactory newNotifierModelFactory,
            INotifierFieldsSettingsRepository notifierFieldsSettingsRepository,
            INotifierGroupsRepository notifierGroupsRepository,
            INotifierService notifierService,
            INotifiersGridModelFactory notifiersGridModelFactory,
            INotifiersModelFactory notifiersModelFactory,
            INotifiersRepository notifiersRepository,
            ISettingsInputModelToUpdatedFieldsSettingsDtoConverter settingsInputModelToSettingsDto, 
            IOrganizationUnitRepository organizationUnitRepository,
            INotifierModelFactory notifierModelFactory,
            IRegionRepository regionRepository)
            : base(masterDataService)
        {
            this.departmentRepository = departmentRepository;
            this.divisionRepository = divisionRepository;
            this.domainRepository = domainRepository;
            this.indexModelFactory = indexModelFactory;
            this.newNotifierModelFactory = newNotifierModelFactory;
            this.notifierFieldsSettingsRepository = notifierFieldsSettingsRepository;
            this.notifierGroupsRepository = notifierGroupsRepository;
            this.notifierService = notifierService;
            this.notifiersGridModelFactory = notifiersGridModelFactory;
            this.notifiersModelFactory = notifiersModelFactory;
            this.notifiersRepository = notifiersRepository;
            this.settingsInputModelToSettingsDto = settingsInputModelToSettingsDto;
            this.organizationUnitRepository = organizationUnitRepository;
            this.notifierModelFactory = notifierModelFactory;
            this.regionRepository = regionRepository;
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
            var notifiers = this.notifiersRepository.FindDetailedOverviewsByCustomerId(currentCustomerId);

            var displaySettings = this.notifierFieldsSettingsRepository.FindByCustomerIdAndLanguageId(
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

            var model = this.indexModelFactory.Create(
                displaySettings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchDivisions,
                Enums.Show.Active,
                500,
                notifiers);

            return this.View(model);
        }
        
        [HttpPost]
        public RedirectToRouteResult NewNotifier(NewNotifierInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var newNotifier = new NewNotifier(
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

            var displayFieldsSettings =
                this.notifierFieldsSettingsRepository.FindDisplayFieldsSettingsByCustomerIdAndLanguageId(
                    currentCustomerId, SessionFacade.CurrentLanguage);

            List<ItemOverviewDto> domains = null;
            List<ItemOverviewDto> regions = null;
            List<ItemOverviewDto> departments = null;
            List<ItemOverviewDto> organizationUnits = null;
            List<ItemOverviewDto> divisions = null;
            List<ItemOverviewDto> managers = null;
            List<ItemOverviewDto> groups = null;

            if (displayFieldsSettings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (displayFieldsSettings.Department.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);
                departments = this.departmentRepository.FindActiveByCustomerId(currentCustomerId);
            }

            if (displayFieldsSettings.OrganizationUnit.Show)
            {
                organizationUnits = this.organizationUnitRepository.FindActiveAndShowable();
            }

            if (displayFieldsSettings.Division.Show)
            {
                divisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
            }

            if (displayFieldsSettings.Manager.Show)
            {
                managers = this.notifiersRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            if (displayFieldsSettings.Group.Show)
            {
                groups = this.notifierGroupsRepository.FindOverviewByCustomerId(currentCustomerId);
            }

            var model = this.newNotifierModelFactory.Create(
                displayFieldsSettings, domains, regions, departments, organizationUnits, divisions, managers, groups);

            return this.View(model);
        }

        [HttpGet]
        public ViewResult Notifier(int id)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var notifier = this.notifiersRepository.FindById(id);

            var displaySettings =
                this.notifierFieldsSettingsRepository.FindDisplayFieldsSettingsByCustomerIdAndLanguageId(
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
                managers = this.notifiersRepository.FindOverviewsByCustomerId(currentCustomerId);
            }

            if (displaySettings.Group.Show)
            {
                groups = this.notifierGroupsRepository.FindOverviewByCustomerId(currentCustomerId);
            }

            var model = this.notifierModelFactory.Create(
                displaySettings, notifier, domains, regions, departments, organizationUnits, divisions, managers, groups);

            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult Notifier(Models.Notifiers.Input.NotifierInputModel inputModel)
        {
            var updatedNotifier = new UpdatedNotifier(
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

            this.notifierService.UpdateNotifier(updatedNotifier);
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public PartialViewResult Search(SearchInputModel inputModel)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var notifiers = this.notifiersRepository.SearchDetailedOverviews(
                currentCustomerId, 
                inputModel.DomainId, 
                inputModel.DepartmentId, 
                inputModel.DivisionId, 
                inputModel.Pharse, 
                (EntityStatus)inputModel.Show, 
                inputModel.RecordsOnPage);

            // Incorrect model. Too many data.
            var fieldsSettings = this.notifierFieldsSettingsRepository.FindByCustomerIdAndLanguageId(
                currentCustomerId, SessionFacade.CurrentLanguage);

            var model = this.notifiersGridModelFactory.Create(notifiers, fieldsSettings);
            return this.PartialView("NotifiersGrid", model);
        }

        [HttpGet]
        public PartialViewResult Notifiers()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var notifiers = this.notifiersRepository.FindDetailedOverviewsByCustomerId(currentCustomerId);

            // Incorrect model. Too many data.
            var displaySettings = this.notifierFieldsSettingsRepository.FindByCustomerIdAndLanguageId(
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
        public RedirectToRouteResult Settings(SettingsInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, null);
            }

            var updatedFieldsSettingsDto = this.settingsInputModelToSettingsDto.Convert(
                inputModel, DateTime.Now, SessionFacade.CurrentCustomer.Id);

            this.notifierFieldsSettingsRepository.UpdateSetting(updatedFieldsSettingsDto);
            this.notifierFieldsSettingsRepository.Commit();
            return this.RedirectToAction("Notifiers");
        }

        #endregion
    }
}