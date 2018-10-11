using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.ComputerUsers;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Domain.Computers;
using DH.Helpdesk.Services.Services.Cases;
using DH.Helpdesk.Web.Infrastructure.Extensions;

namespace DH.Helpdesk.Web.Controllers
{
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

        private readonly ICustomerService customerService;

        private readonly IComputerService computerService;
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSectionService _caseSectionService;

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
            IOrganizationService organizationService,
            ICustomerService customerService,
            IComputerService computerService,
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSectionService caseSectionService)
            : base(masterDataService)
        {
            _caseSectionService = caseSectionService;
            _caseFieldSettingService = caseFieldSettingService;
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
            this.customerService = customerService;
            this.computerService = computerService;
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

            // Prevent to show departments with inactive region
            if (regionId.HasValue)
            {
                var curRegion = this.regionRepository.GetById(regionId.Value);
                if (curRegion.IsActive == 0)
                    regionId = null;
            }

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
        public PartialViewResult UserCategories()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categories = this.computerService.GetComputerUserCategoriesByCustomerID(currentCustomerId, true);

            var emptyCategory = categories.FirstOrDefault(x => x.IsEmpty);

            var list = new List<ComputerUserCategoryOverview>(categories.Where(x => !x.IsEmpty));
            list.Insert(0, emptyCategory);

            var customerFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(currentCustomerId);
            var initiatorCaseFieldSettings = customerFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.UserSearchCategory_Id.ToString());
            var regardingCaseFieldSettings = customerFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id.ToString());

            InitSectionHeaders(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            var model = list.Select(x => new ComputerUserCategoryListItem()
            {
                Id = x.Id,
                Name = Translation.GetMasterDataTranslation(x.Name),
                IsEmpty = x.IsEmpty,
                IsDefaultInitiator = IsGetDefaultCategory(x.Id, x.IsEmpty, initiatorCaseFieldSettings),
                IsDefaultRegarding = IsGetDefaultCategory(x.Id, x.IsEmpty, regardingCaseFieldSettings)
            }).ToList();

            return PartialView(model);
        }
        
        [HttpGet]
        public ViewResult NewCategory()
        {
            var category = new ComputerUserCategoryData()
            {
                Name = "",
                CustomerId = SessionFacade.CurrentCustomer.Id,
            };

            InitSectionHeaders(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);

            return View("EditUserCategory", category);
        }

        [HttpGet]
        public ViewResult EditEmptyCategory()
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var emptyCategory = this.computerService.GetEmptyComputerUserCategory(customerId);

            var model = emptyCategory.Id > 0
                ? GetComputerUserCategoryData(emptyCategory.Id, customerId)
                : new ComputerUserCategoryData()
                {
                    Id = emptyCategory.Id,
                    CustomerId = customerId,
                    Name = emptyCategory.Name,
                    IsEmpty = true
                };
            
            InitSectionHeaders(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);
            return View("EditUserCategory", model);
        }

        [HttpGet]
        public ActionResult EditUserCategory(int id)
        {
            var data = GetComputerUserCategoryData(id, SessionFacade.CurrentCustomer.Id);
            if (data == null)
                return HttpNotFound($"Category (Id={id}) was not found");

            InitSectionHeaders(SessionFacade.CurrentCustomer.Id, SessionFacade.CurrentLanguageId);
            return View(data);
        }

        private void InitSectionHeaders(int customerId, int languageId)
        {
            var initiatorSection = _caseSectionService.GetCaseSectionByType((int)CaseSectionType.Initiator, customerId, languageId);
            var regardingSection = _caseSectionService.GetCaseSectionByType((int)CaseSectionType.Regarding, customerId, languageId);
            ViewBag.InitiatorSectionHeader = initiatorSection.SectionHeader ?? Translation.GetCoreTextTranslation(CaseSections.InitiatorHeader);
            ViewBag.RegardingSectionHeader = regardingSection.SectionHeader ?? Translation.GetCoreTextTranslation(CaseSections.RegardingHeader);
        }

        private int? GetInitiatorDefaultCategoryId(int customerId)
        {
            int defaultCategoryId = 0;
            var customerFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(customerId);

            var initiatorCaseFieldSettings =
                customerFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.UserSearchCategory_Id.ToString());

            if (Int32.TryParse(initiatorCaseFieldSettings.DefaultValue, out defaultCategoryId))
            {
                return defaultCategoryId;
            }

            return null;
        }

        private ComputerUserCategoryData GetComputerUserCategoryData(int categoryId, int customerId, bool isEmpty = false)
        {
            var category = this.computerService.GetComputerUserCategoryByID(categoryId);

            if (category == null)
                return null;

            var customerFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(customerId);

            var isDefaultInitiator =
                CheckIfDefaultCategory(category, GlobalEnums.TranslationCaseFields.UserSearchCategory_Id, customerFieldSettings);

            var isDefaultRegarding =
                CheckIfDefaultCategory(category, GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id, customerFieldSettings);
            
            var data = new ComputerUserCategoryData()
            {
                Id = category.ID,
                CustomerId = category.CustomerID,
                Name = category.Name,
                IsEmpty = category.IsEmpty,
                IsReadOnly = category.IsReadOnly,
                DefaultInitiatorCategory = isDefaultInitiator,
                DefaultRegardingCategory = isDefaultRegarding
            };

            return data;
        }

        private bool CheckIfDefaultCategory(ComputerUserCategory category, GlobalEnums.TranslationCaseFields caseField, IList<CaseFieldSetting> customerFieldSettings)
        {
            var isDefault = false;

            var caseFieldSetting = customerFieldSettings.getCaseSettingsValue(caseField.ToString());
            if (caseFieldSetting != null)
            {
                isDefault = IsGetDefaultCategory(category.ID, category.IsEmpty, caseFieldSetting);
            }

            return isDefault;
        }

        private bool IsGetDefaultCategory(int categoryId, bool isEmpty, CaseFieldSetting caseFieldSetting)
        {
            var defaultValue = 0;
            var isDefault = false;
            if (caseFieldSetting != null && Int32.TryParse(caseFieldSetting.DefaultValue, out defaultValue))
            {
                isDefault = (isEmpty && defaultValue == ComputerUserCategory.EmptyCategoryId) ||
                            (!isEmpty && defaultValue == categoryId);
            }

            return isDefault;
        }

        [HttpPost]
        public ActionResult EditUserCategory(ComputerUserCategoryData data, int? activeTab)
        {
            data.Id = this.computerService.SaveComputerUserCategory(data);

            //set categories default value
            var caseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(data.CustomerId);

            var fs  = caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.UserSearchCategory_Id.ToString());
            UpdateCategoryFieldDefaultValue(data, fs, data.DefaultInitiatorCategory);

            fs = caseFieldSettings.getCaseSettingsValue(GlobalEnums.TranslationCaseFields.IsAbout_UserSearchCategory_Id.ToString());
            UpdateCategoryFieldDefaultValue(data, fs, data.DefaultRegardingCategory);

            return RedirectToAction("Index", new { activeTab = "2"});
        }

        private void UpdateCategoryFieldDefaultValue(ComputerUserCategoryData category, CaseFieldSetting fs, bool setDefault)
        {
            if (fs == null)
                return;

            var categoryId = category.IsEmpty ? ComputerUserCategory.EmptyCategoryId : category.Id;

            //set category as a default 
            if (setDefault)
            {
                _caseFieldSettingService.SaveFieldSettingsDefaultValue(fs.Id, categoryId.ToString());
                return;
            }

            //reset default category only if current default category equals edited category
            if (!string.IsNullOrEmpty(fs.DefaultValue))
            {
                var curValue = -1;
                if (Int32.TryParse(fs.DefaultValue, out curValue) && categoryId == curValue)
                {
                    _caseFieldSettingService.SaveFieldSettingsDefaultValue(fs.Id, string.Empty);
                }
            }
        }

        [HttpGet]
        public ViewResult Index()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var currentLanguageId = SessionFacade.CurrentLanguageId;

            var notifierFilterKey = string.Format(PageName.Notifiers, currentCustomerId);
            var filters = SessionFacade.FindPageFilters<NotifierFilters>(notifierFilterKey);
            if (filters == null)
            {
                filters = NotifierFilters.CreateDefault();
                filters.ComputerUserCategoryID = GetInitiatorDefaultCategoryId(currentCustomerId);
                SessionFacade.SavePageFilters(notifierFilterKey, filters);
            }

            var settings = 
                this.notifierFieldSettingRepository.FindByCustomerIdAndLanguageId(currentCustomerId, currentLanguageId);

            IndexModel model = null;

            if (settings.IsEmpty)
            {
                model = this.indexModelFactory.CreateEmpty();
            }
            else
            {
                List<ItemOverview> searchDomains = null;
                List<ItemOverview> searchRegions = null;
                List<ItemOverview> searchDepartments = null;
                List<ItemOverview> searchOrganizationUnit = null;
                List<ItemOverview> searchComputerUserCategories = null;
                List<ItemOverview> searchDivisions = null;

                if (settings.Domain.ShowInNotifiers)
                {
                    searchDomains = this.domainRepository.FindByCustomerId(currentCustomerId);
                }

                if (settings.Region.ShowInNotifiers)
                {
                    searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);
                }

                if (settings.Department.ShowInNotifiers)
                {
                    searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);

                    if (filters.RegionId.HasValue)
                    {
                        searchDepartments = 
                            this.departmentRepository.FindActiveByCustomerIdAndRegionId(currentCustomerId, filters.RegionId.Value);
                    }
                    else
                    {
                        searchDepartments = this.departmentRepository.FindActiveOverviews(currentCustomerId);
                    }

                    searchOrganizationUnit = this.organizationService.GetOrganizationUnits(filters.DepartmentId);
                }

                if (settings.Division.ShowInNotifiers)
                {
                    searchDivisions = this.divisionRepository.FindByCustomerId(currentCustomerId);
                }

                searchComputerUserCategories = GetUserCategoriesList(currentCustomerId);
                
                //reset categoryId if its not in the list of user categories of the customer
                if (filters.ComputerUserCategoryID.HasValue && 
                    !searchComputerUserCategories.Any(x => x.Value.Equals(filters.ComputerUserCategoryID.ToString(), StringComparison.OrdinalIgnoreCase)))
                {
                    filters.ComputerUserCategoryID = null;
                    SessionFacade.SavePageFilters(notifierFilterKey, filters);
                }

                var sortField = !string.IsNullOrEmpty(filters.SortByField)
                    ? new SortField(filters.SortByField, filters.SortBy)
                    : null;

                var parameters = new SearchParameters(
                    currentCustomerId,
                    filters.DomainId,
                    filters.RegionId,
                    filters.DepartmentId,
                    filters.OrganizationUnitId,
                    filters.DivisionId,
                    filters.Pharse,
                    filters.ComputerUserCategoryID,
                    filters.Status,
                    filters.RecordsOnPage,
                    sortField);

                var searchResult = this.notifierRepository.Search(parameters);

                ViewBag.ActiveTab = ControllerContext.HttpContext.Request.QueryString["activeTab"];

                model = this.indexModelFactory.Create(
                     settings,
                     searchDomains,
                     searchRegions,
                     searchDepartments,
                     searchOrganizationUnit,
                     searchDivisions,
                     searchComputerUserCategories,
                     filters,
                     searchResult);
            }

            return this.View(model);
        }

        private List<ItemOverview> GetUserCategoriesList(int customerId)
        {
            List<ItemOverview> searchComputerUserCategories = new List<ItemOverview>();

            var computerUserCategories = computerService.GetComputerUserCategoriesByCustomerID(customerId, true);
            if (computerUserCategories.Any())
            {
                searchComputerUserCategories = computerUserCategories.Where(o => !o.IsEmpty)
                    .Select(o => new ItemOverview(Translation.GetMasterDataTranslation(o.Name), o.Id.ToString()))
                    .OrderBy(x => x.Name)
                    .ToList();

                var emptyCategoryName = computerUserCategories.FirstOrDefault(x => x.IsEmpty)?.Name ?? ComputerUserCategory.EmptyCategoryDefaultName;
                searchComputerUserCategories.Insert(0, new ItemOverview(Translation.GetMasterDataTranslation(emptyCategoryName), ComputerUserCategory.EmptyCategoryId.ToString()));
            }
            return searchComputerUserCategories;
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult NewNotifier(InputModel model)
        {
            if (((model.FirstName == null || !model.FirstName.Show || string.IsNullOrEmpty(model.FirstName.Value)) ||
                 (model.LastName == null || !model.LastName.Show || string.IsNullOrEmpty(model.LastName.Value))) && model.DisplayName != null &&
               !string.IsNullOrEmpty(model.DisplayName.Value))
            {
                var splitedName = GetSplitedName(model.DisplayName != null ? model.DisplayName.Value : string.Empty);

                if (model.FirstName == null || !model.FirstName.Show || string.IsNullOrEmpty(model.FirstName.Value))
                    model.FirstName = new StringFieldModel(false, "FirstName", splitedName.Key);

                if (model.LastName == null || !model.LastName.Show || string.IsNullOrEmpty(model.LastName.Value))
                    model.LastName = new StringFieldModel(false, "LastName", splitedName.Value);
            }

            if (model.DisplayName == null || !model.DisplayName.Show || string.IsNullOrEmpty(model.DisplayName.Value))
            {
                var firstName = string.Empty;
                var lastName = string.Empty;

                if (model.FirstName != null)
                    firstName = model.FirstName.Value;

                if (model.LastName != null)
                    lastName = model.LastName.Value;

                model.DisplayName = new StringFieldModel(false, "DisplayName", string.Format("{0} {1}", firstName, lastName));
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var categoriesList = GetCategoriesSelectList(currentCustomerId);
            model.ComputerUserCategoryModel = new ComputerUserCategoryModel(categoriesList); //todo: check if required for AddNotifier

            //handle empty virtual cateogry
            if (model.CategoryId == ComputerUserCategory.EmptyCategoryId)
            {
                model.CategoryId = null;
            }

            var newNotifier = this.newNotifierFactory.Create(model, currentCustomerId, DateTime.Now);

            this.notifierService.AddNotifier(newNotifier);
            return this.RedirectToAction("Index");
        }

        [ValidateInput(false)]
        [BadRequestOnNotValid]
        [HttpPost]
        public JsonResult NewNotifierPopup(InputModel model)
        {
            if (((model.FirstName == null || !model.FirstName.Show || string.IsNullOrEmpty(model.FirstName.Value)) ||
                 (model.LastName == null || !model.LastName.Show || string.IsNullOrEmpty(model.LastName.Value))) && model.DisplayName != null &&
               !string.IsNullOrEmpty(model.DisplayName.Value))
            {
                var splitedName = GetSplitedName(model.DisplayName != null ? model.DisplayName.Value : string.Empty);

                if (model.FirstName == null || !model.FirstName.Show || string.IsNullOrEmpty(model.FirstName.Value))
                    model.FirstName = new StringFieldModel(false, "FirstName", splitedName.Key);

                if (model.LastName == null || !model.LastName.Show || string.IsNullOrEmpty(model.LastName.Value))
                    model.LastName = new StringFieldModel(false, "LastName", splitedName.Value);
            }

            if (model.DisplayName == null || !model.DisplayName.Show || string.IsNullOrEmpty(model.DisplayName.Value))
            {
                var firstName = string.Empty;
                var lastName = string.Empty;

                if (model.FirstName != null)
                    firstName = model.FirstName.Value;

                if (model.LastName != null)
                    lastName = model.LastName.Value;

                model.DisplayName = new StringFieldModel(false, "DisplayName", string.Format("{0} {1}", firstName, lastName));
            }

            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var categoriesList = GetCategoriesSelectList(currentCustomerId);
            model.ComputerUserCategoryModel = new ComputerUserCategoryModel(categoriesList);

            //handle empty virtual cateogry
            if (model.CategoryId == ComputerUserCategory.EmptyCategoryId)
            {
                model.CategoryId = null;
            }

            var newNotifier = this.newNotifierFactory.Create(model, SessionFacade.CurrentCustomer.Id, DateTime.Now);
            this.notifierService.AddNotifier(newNotifier);
            return new JsonResult { Data = newNotifier.Id };
        }

        [ValidateInput(false)]
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
            int? organizationUnitId,
            string costcentre,
            int? userCategory)
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
            List<ItemOverview> languages = null;

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

            if (settings.Language.Show)
            {
                languages = this.languageRepository.FindActiveOverviews();
            }

            if (!string.IsNullOrEmpty(userId))
            {
                userId = userId.Replace("<", "").Replace(">", "");
                inputParams.Add("UserId", userId);
            }

            var splitedName = GetSplitedName(fName);
            inputParams.Add("FName", fName);
            inputParams.Add("FirstName", splitedName.Key);
            inputParams.Add("LastName", splitedName.Value);


            if (!string.IsNullOrEmpty(email))
                inputParams.Add("Email", email);

            if (!string.IsNullOrEmpty(phone))
                inputParams.Add("Phone", phone);

            if (!string.IsNullOrEmpty(placement))
                inputParams.Add("Placement", placement);

            if (!string.IsNullOrEmpty(cellPhone))
                inputParams.Add("CellPhone", cellPhone);

            if (!string.IsNullOrEmpty(costcentre))
                inputParams.Add("CostCentre", costcentre);

            ComputerUserCategory category = null;
            if (userCategory.HasValue && userCategory.Value > 0)
            {
                category = computerService.GetComputerUserCategoryByID(userCategory.Value);
            }
            
            var categoriesList = GetCategoriesSelectList(currentCustomerId, category);
            var categoryModel = new ComputerUserCategoryModel(categoriesList);

            var model = this.newNotifierModelFactory.Create(
                settings,
                domains,
                regions,
                departments,
                organizationUnits,
                divisions,
                managers,
                groups,
                inputParams,
                languages,
                categoryModel);

            return this.View(model);
        }

        [HttpGet]
        public ViewResult NewNotifier()
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;
            var langId = SessionFacade.CurrentLanguageId;
            var inputParams = new Dictionary<string, string>();

            var settings =
                this.notifierFieldSettingRepository.FindDisplayFieldSettingsByCustomerIdAndLanguageId(currentCustomerId, langId);

            List<ItemOverview> domains = null;
            List<ItemOverview> regions = null;
            List<ItemOverview> departments = null;
            List<ItemOverview> organizationUnits = null;
            List<ItemOverview> divisions = null;
            List<ItemOverview> managers = null;
            List<ItemOverview> groups = null;
            List<ItemOverview> languages = null;

            int? regionId = null;
            int? departmentId = null;
            int? organizationUnitId = null;
            int? languageId = null;

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


            if (settings.Language.Show)
            {
                var languagesData =
                    this.languageRepository.GetActiveLanguages().ToArray();

                languages = languagesData.Select(d => new ItemOverview(d.Name, d.Id.ToString())).ToList();

                //ItemOverview f = new ItemOverview(" ", "0");
                //languages.Add(f);

                languages = languages.OrderBy(z => z.Value).ToList();

                languageId = this.customerService.GetCustomerLanguage(currentCustomerId);

                if (languageId != null && languageId.Value > 0)
                    inputParams.Add("LanguageId", languageId.Value.ToString()); // Takes default from setting
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

            
            ComputerUserCategory selectedCategory = null;

            var notifierFilterKey = string.Format(PageName.Notifiers, currentCustomerId);
            var initiatorFilter = SessionFacade.FindPageFilters<NotifierFilters>(notifierFilterKey);
            var categoryId = initiatorFilter?.ComputerUserCategoryID ?? 0;
            if (categoryId > 0)
            {
                selectedCategory = this.computerService.GetComputerUserCategoryByID(categoryId);
            }

            var categoriesList = GetCategoriesSelectList(currentCustomerId, selectedCategory);
            var categoryModel = new ComputerUserCategoryModel(categoriesList);

            var model = this.newNotifierModelFactory.Create(
                settings,
                domains,
                regions,
                departments,
                organizationUnits,
                divisions,
                managers,
                groups,
                inputParams,
                languages,
                categoryModel);

            return this.View(model);
        }

        private IList<SelectListItem> GetCategoriesSelectList(int customerId, ComputerUserCategory category = null)
        {
            var selectedCateogryId = category?.ID ?? ComputerUserCategory.EmptyCategoryId;
            var categoriesList =
                computerService.GetComputerUserCategoriesByCustomerID(customerId, true)
                    .Select(x => new
                    {
                        x.Id,
                        Name = Translation.GetMasterDataTranslation(x.Name),
                        Order = x.IsEmpty ? 1 : 0 
                    })
                    .OrderByDescending(x => x.Order).ThenBy(x => x.Name)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name,
                        Selected = x.Id == selectedCateogryId
                    }).ToList();

            return categoriesList;
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
            List<ItemOverview> languages = null;

            int? departmentRegionId = null;
            int? departmentId = null;
            int? organizationUnitId = null;
            int? languageId = null;

            if (displaySettings.Domain.Show)
            {
                domains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (displaySettings.Region.Show)
            {
                regions = this.regionRepository.FindByCustomerId(currentCustomerId);

                // As each "Department" must has a "Region" in defination, we get notifier RegionId from Department.
                if (notifier.DepartmentId != null)
                    departmentRegionId = this.departmentRepository.GetById(notifier.DepartmentId.Value).Region_Id;

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

            if (displaySettings.Language.Show)
            {
                var languageData =
                this.languageRepository.GetActiveLanguages();

                ItemOverview io = new ItemOverview(" ", "0");


                

                languages = languageData.Select(d => new ItemOverview(d.Name, d.Id.ToString())).ToList();
                languages.Add(io);
                //ItemOverview f = new ItemOverview(" ", "0");
                //languages.Add(f);

                languages = languages.OrderBy(z => z.Value).ToList();

                languageId = notifier.LanguageId;

                if (languageId != null && languageId.Value > 0)
                    inputParams.Add("LanguageId", languageId.Value.ToString()); // Takes default from saved Notifier

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

            var computerUser = computerService.GetComputerUser(notifier.Id);

            var categoriesList = GetCategoriesSelectList(currentCustomerId, computerUser.ComputerUserCategory);
            var categoryModel = new ComputerUserCategoryModel(categoriesList, computerUser.ComputerUserCategory);

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
                groups,
                languages,
                categoryModel);

            return this.View(model);
        }

        [HttpPost]
        [BadRequestOnNotValid]
        public RedirectToRouteResult Notifier(InputModel model)
        {
            //handle empty virtual cateogry
            if (model.CategoryId == ComputerUserCategory.EmptyCategoryId)
            {
                model.CategoryId = null;
            }

            var updatedNotifier = this.updatedNotifierFactory.Create(model, DateTime.Now);
            //updatedNotifier.LanguageId = model.LanguageId;
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

            var notifierFilterKey = string.Format(PageName.Notifiers, currentCustomerId);
            var filters = SessionFacade.FindPageFilters<NotifierFilters>(notifierFilterKey);
            if (filters == null)
            {
                filters = NotifierFilters.CreateDefault();
                SessionFacade.SavePageFilters(notifierFilterKey, filters);
            }

            List<ItemOverview> searchDomains = null;
            List<ItemOverview> searchRegions = null;
            List<ItemOverview> searchDepartments = null;
            List<ItemOverview> searchOrganizationUnit = null;
            List<ItemOverview> searchDivisions = null;
            List<ItemOverview> searchComputerUserCategories = null;

            if (displaySettings.Domain.ShowInNotifiers)
            {
                searchDomains = this.domainRepository.FindByCustomerId(currentCustomerId);
            }

            if (displaySettings.Region.ShowInNotifiers)
            {
                searchRegions = this.regionRepository.FindByCustomerId(currentCustomerId);
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

                searchOrganizationUnit = this.organizationService.GetOrganizationUnits(filters.DepartmentId);
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
                filters.OrganizationUnitId,
                filters.DivisionId,
                filters.Pharse,
                filters.ComputerUserCategoryID,
                filters.Status,
                filters.RecordsOnPage,
                null);

            searchComputerUserCategories = GetUserCategoriesList(currentCustomerId);

            var searchResult = this.notifierRepository.Search(parameters);

            var model = this.notifiersModelFactory.Create(
                displaySettings,
                searchDomains,
                searchRegions,
                searchDepartments,
                searchOrganizationUnit,
                searchDivisions,
                searchComputerUserCategories,
                filters,
                searchResult);

            return this.PartialView(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        [BadRequestOnNotValid]
        public PartialViewResult Search(SearchInputModel inputModel)
        {
            var currentCustomerId = SessionFacade.CurrentCustomer.Id;

            var filters = inputModel.ExtractFilters();
            var notifierFilterKey = string.Format(PageName.Notifiers, currentCustomerId);
            SessionFacade.SavePageFilters(notifierFilterKey, filters);

            var sortField = !string.IsNullOrEmpty(inputModel.SortField.Name)
                ? new SortField(inputModel.SortField.Name, inputModel.SortField.SortBy.Value)
                : null;

            var parameters = new SearchParameters(
                currentCustomerId,
                filters.DomainId,
                filters.RegionId,
                filters.DepartmentId,
                filters.OrganizationUnitId,
                filters.DivisionId,
                filters.Pharse,
                filters.ComputerUserCategoryID,
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

        [HttpGet]
        public JsonResult CheckUniqueUserId(string userId, int initiatorId)
        {
            var isUnique = notifierRepository.IsInitiatorUserIdUnique(userId, initiatorId, SessionFacade.CurrentCustomer.Id, true);
            return Json(isUnique, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetLanguageFromDepartment(string departmentid)
        {
            departmentid = departmentid.Replace("'", "");
            if (departmentid != string.Empty)
            {
                int depid = Convert.ToInt32(departmentid);
                int languageId = this.departmentRepository.GetDepartmentLanguage(depid);
                return languageId.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        [HttpGet]
        public string GetLanguageFromRegion(string regionid)

        {

            regionid = regionid.Replace("'", "");
            if (regionid != string.Empty)
            {
                int regid = Convert.ToInt32(regionid);
                int languageId = this.regionRepository.GetRegionLanguage(regid);
                return languageId.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private KeyValuePair<string, string> GetSplitedName(string fullName)
        {
            var firstName = string.Empty;
            var lastName = string.Empty;

            if (!string.IsNullOrEmpty(fullName))
            {
                fullName = fullName.TrimStart();
                var spacePos = fullName.IndexOf(" ");

                if (spacePos > 0)
                {
                    firstName = fullName.Substring(0, spacePos);
                    if (fullName.Length >= spacePos + 1)
                        lastName = fullName.Substring(spacePos + 1);
                }
                else
                {
                    firstName = fullName;
                }
            }

            return new KeyValuePair<string, string>(firstName, lastName);
        }
        #endregion
    }
}