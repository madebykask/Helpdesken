﻿
namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Requests.Cases;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Services.Services.Concrete;
    using DH.Helpdesk.Web.Models.CaseSolution;

    using DH.Helpdesk.Common.Enums.CaseSolution;
    using System.Threading;
    using DH.Helpdesk.BusinessData.Models;
    using Models.CaseRules;
    using Infrastructure.ModelFactories.Case.Concrete;
    using Common.Enums.Settings;
    using System;
    using DH.Helpdesk.Domain.Cases;

    public class CaseSolutionController : UserInteractionController
    {
        private readonly ICaseFieldSettingService _caseFieldSettingService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICaseTypeService _caseTypeService;
        private readonly ICategoryService _categoryService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly IPriorityService _priorityService;
        private readonly IProductAreaService _productAreaService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IWorkingGroupService _workingGroupService;
        private readonly IDepartmentService _departmentService;
        private readonly IRegionService _regionService;
        private readonly IOUService _ouService;
        private readonly ISystemService _systemService;
        private readonly IUrgencyService _urgencyService;
        private readonly IImpactService _impactService;
        private readonly IStatusService _statusService;
        private readonly IStateSecondaryService _stateSecondaryService;
        private readonly ICountryService _countryService;
        private readonly ISupplierService _supplierService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ISettingService _settingService;
        private readonly IProblemService _problemService;
        private readonly IChangeService _changeService;
        private readonly ICausingPartService _causingPartService;
        private readonly IOrganizationService _organizationService;
        private readonly IRegistrationSourceCustomerService _registrationSourceCustomerService;
        private readonly ICaseSolutionSettingService caseSolutionSettingService;
        private readonly ICaseRuleFactory _caseRuleFactory;
        private readonly IWatchDateCalendarService _watchDateCalendarService;
        private readonly ICacheProvider _cache;
        private readonly ICaseSolutionConditionService _caseSolutionConditionService;

        private const int MAX_QUICK_BUTTONS_COUNT = 5;
        private const string CURRENT_USER_ITEM_CAPTION = "Inloggad användare";
        private const string CURRENT_USER_WORKINGGROUP_CAPTION = "Inloggad användares driftgrupp";

        public CaseSolutionController(
            ICaseFieldSettingService caseFieldSettingService,
            ICaseSolutionService caseSolutionService,
            ICaseTypeService caseTypeService,
            ICategoryService categoryService,
            IFinishingCauseService finishingCauseService,
            IPriorityService priorityService,
            IProductAreaService productAreaService,
            IProjectService projectService,
            IUserService userService,
            IWorkingGroupService workingGroupService,
            IDepartmentService departmentService,
            IMasterDataService masterDataService,
            ICaseSolutionSettingService caseSolutionSettingService,
            IRegionService regionService,
            IOUService ouService,
            ISystemService systemService,
            IUrgencyService urgencyService,
            IImpactService impactService,
            IStatusService statusService,
            IStateSecondaryService stateSecondaryService,
            ICountryService countryService,
            ISupplierService supplierService,
            ICurrencyService currencyService,
            ICustomerUserService customerUserService,
            ISettingService settingService,
            IProblemService problemService,
            IChangeService changeService,
            ICausingPartService causingPartService,
            IOrganizationService organizationService,
            IRegistrationSourceCustomerService registrationSourceCustomerService,
            ICaseRuleFactory caseRuleFactory,
            IWatchDateCalendarService watchDateCalendarService,
            ICacheProvider cache,
            ICaseSolutionConditionService caseSolutionConditionService)
            : base(masterDataService)
        {
            this._caseFieldSettingService = caseFieldSettingService;
            this._caseSolutionService = caseSolutionService;
            this._caseTypeService = caseTypeService;
            this._categoryService = categoryService;
            this._finishingCauseService = finishingCauseService;
            this._priorityService = priorityService;
            this._productAreaService = productAreaService;
            this._projectService = projectService;
            this._userService = userService;
            this._workingGroupService = workingGroupService;
            this._departmentService = departmentService;
            this.caseSolutionSettingService = caseSolutionSettingService;
            this._regionService = regionService;
            this._ouService = ouService;
            this._systemService = systemService;
            this._urgencyService = urgencyService;
            this._impactService = impactService;
            this._statusService = statusService;
            this._stateSecondaryService = stateSecondaryService;
            this._countryService = countryService;
            this._supplierService = supplierService;
            this._currencyService = currencyService;
            this._customerUserService = customerUserService;
            this._settingService = settingService;
            this._problemService = problemService;
            this._changeService = changeService;
            this._causingPartService = causingPartService;
            this._organizationService = organizationService;
            this._registrationSourceCustomerService = registrationSourceCustomerService;
            this._caseRuleFactory = caseRuleFactory;
            this._cache = cache;
            this._caseSolutionConditionService = caseSolutionConditionService;
            _watchDateCalendarService = watchDateCalendarService;
        }

        #region Template 

        public ActionResult Index()
        {
            //Set default
            var cs = new CaseSolutionSearch()
            {
                Ascending = true,
                SearchCss = string.Empty,
                SortBy = CaseSolutionIndexColumns.Name
            };

            if (SessionFacade.CurrentCaseSolutionSearch != null)
                cs = SessionFacade.CurrentCaseSolutionSearch;
            else
                SessionFacade.CurrentCaseSolutionSearch = cs;

            if (string.IsNullOrEmpty(SessionFacade.FindActiveTab("CaseSolution")))
                SessionFacade.SaveActiveTab("CaseSolution", "CaseTemplate");

            var model = CreateIndexViewModel(cs);
            return this.View(model);
        }

        [HttpGet]
        public JsonResult RememberTab(string topic, string tab)
        {
            SessionFacade.SaveActiveTab(topic, tab);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult Sort(string fieldName, bool asc)
        {
            var caseSolutionSearch = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                caseSolutionSearch = SessionFacade.CurrentCaseSolutionSearch;

            caseSolutionSearch.Ascending = !asc;
            caseSolutionSearch.SortBy = fieldName;
            SessionFacade.CurrentCaseSolutionSearch = caseSolutionSearch;

            var model = CreateIndexViewModel(caseSolutionSearch);
            return PartialView("_RowsOverview", model.Rows);
        }

        [HttpPost]
        public PartialViewResult Search(string searchText, List<int> categoryIds)
        {
            var caseSolutionSearch = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                caseSolutionSearch = SessionFacade.CurrentCaseSolutionSearch;

            caseSolutionSearch.SearchCss = searchText;
            caseSolutionSearch.CategoryIds = categoryIds.ToList();
            SessionFacade.CurrentCaseSolutionSearch = caseSolutionSearch;

            var model = CreateIndexViewModel(caseSolutionSearch);
            return PartialView("_RowsOverview", model.Rows);
        }

        public ActionResult New(int? backToPageId)
        {
            // Positive: Send Mail to...
            var caseSolution = new CaseSolution() { Customer_Id = SessionFacade.CurrentCustomer.Id, NoMailToNotifier = 1, Status = 1, ShowOnCaseOverview = 1, ShowInsideCase = 1, OverWritePopUp = 1 };

            if (backToPageId == null)
                ViewBag.PageId = 0;
            else
                ViewBag.PageId = backToPageId;

            if (caseSolution == null)
                return new HttpNotFoundResult("No case solution found...");

            var model = this.CreateInputViewModel(caseSolution);

            return this.View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult New(CaseSolution caseSolution, CaseSolutionInputViewModel caseSolutionInputViewModel, CaseSolutionSettingModel[] caseSolutionSettingModels, int PageId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            IList<CaseFieldSetting> CheckMandatory = null;//_caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id);
            this.TempData["RequiredFields"] = null;
            if (caseSolutionSettingModels == null)
            {
                caseSolutionSettingModels = new CaseSolutionSettingModel[0];
            }

            var caseSolutionSchedule = this.CreateCaseSolutionSchedule(caseSolutionInputViewModel);

            // Positive: Send Mail to...
            if (caseSolutionInputViewModel.CaseSolution.NoMailToNotifier == 0)
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 1;
            else
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 0;

            if (caseSolutionInputViewModel.CaseSolution.PerformerUser_Id == -1)
            {
                caseSolutionInputViewModel.CaseSolution.PerformerUser_Id = null;
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = 1;
            }
            else
            {
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = null;
            }

            if (caseSolutionInputViewModel.CaseSolution.CaseWorkingGroup_Id == -1)
            {
                caseSolutionInputViewModel.CaseSolution.CaseWorkingGroup_Id = null;
                caseSolutionInputViewModel.CaseSolution.SetCurrentUsersWorkingGroup = 1;
            }
            else
            {
                caseSolutionInputViewModel.CaseSolution.SetCurrentUsersWorkingGroup = null;
            }

            if (caseSolutionInputViewModel.CaseSolution.ShowInsideCase != 1 || caseSolutionInputViewModel.CaseSolution.SaveAndClose < 0)
                caseSolutionInputViewModel.CaseSolution.SaveAndClose = null;

            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            CaseSettingsSolutionAggregate settingsSolutionAggregate = this.CreateCaseSettingsSolutionAggregate(caseSolutionInputViewModel.CaseSolution.Id, caseSolutionSettingModels);
            this.caseSolutionSettingService.AddCaseSolutionSettings(settingsSolutionAggregate);

            if (errors.Count == 0)
            {
                switch (PageId) // back to refrence page
                {
                    case 0:
                        return this.RedirectToAction("index", "casesolution");

                    case 1:
                        return this.RedirectToAction("index", "Cases");
                }
            }

            this.TempData["RequiredFields"] = errors;

            var model = this.CreateInputViewModel(caseSolution);

            return this.View(model);
        }

        public ActionResult Edit(int id, int? backToPageId)
        {
            var caseSolution = this._caseSolutionService.GetCaseSolution(id);

            if (caseSolution == null)
                return new HttpNotFoundResult("No case solution found...");

            // Positive: Send Mail to...
            /// If you ever remove this - please remove it in GetTemplate() action also
            if (caseSolution.NoMailToNotifier == 0)
                caseSolution.NoMailToNotifier = 1;
            else
                caseSolution.NoMailToNotifier = 0;

            if (backToPageId == null)
                ViewBag.PageId = 0;
            else
                ViewBag.PageId = backToPageId;


            var model = this.CreateInputViewModel(caseSolution);

            return this.View(model);
        }

        public ActionResult GetTemplate(int id)
        {
            var caseSolution = this._caseSolutionService.GetCaseSolution(id);

            if (caseSolution == null)
            {
                return new HttpNotFoundResult("No case solution found...");
            }

            /// This strange logic I took from Edit() action
            caseSolution.NoMailToNotifier = caseSolution.NoMailToNotifier == 0 ? 1 : 0;

            // Check CaseType is Active
            if (caseSolution.CaseType_Id.HasValue)
            {
                var caseType = _caseTypeService.GetCaseType(caseSolution.CaseType_Id.Value);
                if (!(caseType != null && caseType.IsActive != 0))
                    caseSolution.CaseType_Id = null;
            }

            // Check ProductArea is Active
            if (caseSolution.ProductArea_Id.HasValue)
            {
                var productArea = _productAreaService.GetProductArea(caseSolution.ProductArea_Id.Value);
                if (!(productArea != null && productArea.IsActive != 0))
                    caseSolution.ProductArea_Id = null;
            }

            // Check Finishing Cause is Active
            if (caseSolution.FinishingCause_Id.HasValue)
            {
                var finishingCause = _finishingCauseService.GetFinishingCause(caseSolution.FinishingCause_Id.Value);
                if (!(finishingCause != null && finishingCause.IsActive != 0))
                    caseSolution.FinishingCause_Id = null;
            }

            if (SessionFacade.CurrentUser != null && caseSolution.SetCurrentUserAsPerformer == 1)
                caseSolution.PerformerUser_Id = SessionFacade.CurrentUser.Id;

            if (SessionFacade.CurrentUser != null && SessionFacade.CurrentCustomer != null && caseSolution.SetCurrentUsersWorkingGroup == 1)
            {
                var userDefaultWGId = this._userService.GetUserDefaultWorkingGroupId(SessionFacade.CurrentUser.Id, SessionFacade.CurrentCustomer.Id);
                if (userDefaultWGId.HasValue)
                {
                    caseSolution.CaseWorkingGroup_Id = userDefaultWGId.Value;
                }
                else
                {
                    caseSolution.WorkingGroup_Id = null;
                }
            }

            return this.Json(
                new
                {
                    dateFormat = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern,
                    caseSolution.PersonsName,
                    caseSolution.PersonsPhone,
                    caseSolution.PersonsCellPhone,
                    caseSolution.Region_Id,
                    caseSolution.Department_Id,
                    caseSolution.OU_Id,
                    caseSolution.CostCentre,
                    caseSolution.Place,
                    caseSolution.UserCode,
                    UpdateNotifierInformation = caseSolution.UpdateNotifierInformation.ToBool().ToString(),

                    caseSolution.IsAbout_ReportedBy,
                    caseSolution.IsAbout_PersonsName,
                    caseSolution.IsAbout_PersonsEmail,
                    caseSolution.IsAbout_PersonsPhone,
                    caseSolution.IsAbout_PersonsCellPhone,
                    caseSolution.IsAbout_Region_Id,
                    caseSolution.IsAbout_Department_Id,
                    caseSolution.IsAbout_OU_Id,
                    caseSolution.IsAbout_CostCentre,
                    caseSolution.IsAbout_Place,
                    caseSolution.IsAbout_UserCode,

                    caseSolution.System_Id,
                    caseSolution.Urgency_Id,
                    caseSolution.Impact_Id,
                    caseSolution.InventoryLocation,
                    caseSolution.InventoryNumber,
                    caseSolution.InventoryType,
                    caseSolution.CaseType_Id,
                    caseSolution.PerformerUser_Id,
                    caseSolution.Category_Id,
                    caseSolution.ReportedBy,
                    NoMailToNotifier = caseSolution.NoMailToNotifier.ToBool(),
                    caseSolution.ProductArea_Id,
                    caseSolution.Caption,
                    caseSolution.Description,
                    caseSolution.Miscellaneous,
                    caseSolution.CaseWorkingGroup_Id,
                    caseSolution.Priority_Id,
                    caseSolution.Project_Id,
                    caseSolution.Text_External,
                    caseSolution.Text_Internal,
                    caseSolution.FinishingCause_Id,
                    caseSolution.RegistrationSource,
                    caseSolution.Status_Id,
                    caseSolution.StateSecondary_Id,
                    caseSolution.PersonsEmail,
                    WatchDate = caseSolution.WatchDate.HasValue ? caseSolution.WatchDate.Value.ToShortDateString() : string.Empty,
                    caseSolution.CausingPartId,
                    caseSolution.InvoiceNumber,
                    caseSolution.ReferenceNumber,

                    SMS = caseSolution.SMS.ToBool(),
                    caseSolution.AgreedDate,
                    caseSolution.Available,
                    caseSolution.Cost,
                    caseSolution.OtherCost,
                    caseSolution.Currency,
                    caseSolution.Problem_Id,
                    PlanDate = caseSolution.PlanDate.HasValue ? caseSolution.PlanDate.Value.ToShortDateString() : string.Empty,
                    VerifiedDescription = caseSolution.VerifiedDescription,
                    SolutionRate = caseSolution.SolutionRate,
                    caseSolution.OverWritePopUp,
                    caseSolution.SaveAndClose
                },
                    JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(
            CaseSolutionInputViewModel caseSolutionInputViewModel,
            CaseSolutionSettingModel[] CaseSolutionSettingModels,
            int PageId, string selectedStates, string selectedCaseWgs)
        {


           

            IDictionary<string, string> errors = new Dictionary<string, string>();
            IList<CaseFieldSetting> CheckMandatory = null; //_caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id); 
            this.TempData["RequiredFields"] = null;
            if (CaseSolutionSettingModels == null)
            {
                CaseSolutionSettingModels = new CaseSolutionSettingModel[0];
            }
            var caseSolutionSchedule = this.CreateCaseSolutionSchedule(caseSolutionInputViewModel);

            // Positive: Send Mail to...
            if (caseSolutionInputViewModel.CaseSolution.NoMailToNotifier == 0)
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 1;
            else
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 0;

            if (caseSolutionInputViewModel.CaseSolution.PerformerUser_Id == -1)
            {
                caseSolutionInputViewModel.CaseSolution.PerformerUser_Id = null;
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = 1;
            }
            else
            {
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = null;
            }

            if (caseSolutionInputViewModel.CaseSolution.CaseWorkingGroup_Id == -1)
            {
                caseSolutionInputViewModel.CaseSolution.CaseWorkingGroup_Id = null;
                caseSolutionInputViewModel.CaseSolution.SetCurrentUsersWorkingGroup = 1;
            }
            else
            {
                caseSolutionInputViewModel.CaseSolution.SetCurrentUsersWorkingGroup = null;
            }

            if (caseSolutionInputViewModel.CaseSolution.ShowInsideCase != 1 || caseSolutionInputViewModel.CaseSolution.SaveAndClose < 0)
                caseSolutionInputViewModel.CaseSolution.SaveAndClose = null;

            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            CaseSettingsSolutionAggregate settingsSolutionAggregate =
                this.CreateCaseSettingsSolutionAggregate(
                    caseSolutionInputViewModel.CaseSolution.Id,
                    CaseSolutionSettingModels);
            this.caseSolutionSettingService.UpdateCaseSolutionSettings(settingsSolutionAggregate);


            string stateId = "case_StateSecondary.StateSecondaryGUID";
            CaseSolutionConditionEntity cce = new CaseSolutionConditionEntity
            {

                CaseSolution_Id = caseSolutionInputViewModel.CaseSolution.Id,
                Property_Name = stateId,
                Values = selectedStates,
                Status = 1
            };

            this._caseSolutionConditionService.Save(cce);

            stateId = "case_WorkingGroup.WorkingGroupGUID";
            cce = new CaseSolutionConditionEntity
            {

                CaseSolution_Id = caseSolutionInputViewModel.CaseSolution.Id,
                Property_Name = stateId,
                Values = selectedCaseWgs,
                Status = 1
            };

            this._caseSolutionConditionService.Save(cce);

            //Remove caching of conditions for this specific template that is used in Case
            string cacheKey = string.Format(DH.Helpdesk.Common.Constants.CacheKey.CaseSolutionCondition, caseSolutionInputViewModel.CaseSolution.Id);
            this._cache.Invalidate(cacheKey);

            if (errors.Count == 0)
            {
                switch (PageId) // back to refrence page
                {
                    case 1:
                        return this.RedirectToAction("index", "Cases");

                    default:
                        return this.RedirectToAction("index", "casesolution");

                }
            }

            this.TempData["RequiredFields"] = errors;
            var model = this.CreateInputViewModel(caseSolutionInputViewModel.CaseSolution);


            return this.View(model);
        }

        [HttpPost]
        public RedirectToRouteResult Delete(int id, int pageId)
        {
            if (this._caseSolutionService.DeleteCaseSolution(id, SessionFacade.CurrentCustomer.Id) == DeleteMessage.Success)
            {
                switch (pageId)
                {
                    case 1:
                        return this.RedirectToAction("index", "Cases");

                    default:
                        return this.RedirectToAction("index", "casesolution");

                }
            }
            else
            {
                this.TempData.Add("Error", string.Empty);
                return this.RedirectToAction("edit", "casesolution", new { id = id });
            }
        }


        public JsonResult ChangeRegion(int? regionId)
        {
            var list = this._departmentService.GetActiveDepartmentsBy(SessionFacade.CurrentCustomer.Id, regionId)
                                            .Select(d => new
                                            {
                                                id = d.Id,
                                                name = d.DepartmentName
                                            });

            return this.Json(new { list });
        }

        public JsonResult ChangeDepartment(int? departmentId)
        {
            var list = this._organizationService.GetOrganizationUnits(departmentId)
                                            .Select(o => new
                                            {
                                                id = o.Value,
                                                name = o.Name
                                            });

            return this.Json(new { list });
        }

        public JsonResult ChangeWorkingGroupFilterUser(int? id, int customerId)
        {
            IList<User> performersList;
            var customerSettings = this._settingService.GetCustomerSetting(customerId);
            if (customerSettings.DontConnectUserToWorkingGroup == 0 && id > 0)
            {
                performersList = this._userService.GetAvailablePerformersForWorkingGroup(customerId, id);
            }
            else
            {
                performersList = this._userService.GetAvailablePerformersOrUserId(customerId);
            }

            var currentUser = new User() { Id = -1, FirstName = string.Format("-- {0} --", Translation.GetCoreTextTranslation(CURRENT_USER_ITEM_CAPTION)) };
            performersList.Insert(0, currentUser);
            if (customerSettings.IsUserFirstLastNameRepresentation == 1)
            {
                return
                    this.Json(
                        new
                        {
                            list =
                                performersList.OrderBy(it => it.FirstName)
                                    .ThenBy(it => it.SurName)
                                    .Select(
                                        it =>
                                        new IdName
                                        {
                                            id = it.Id,
                                            name = string.Format("{0} {1}", it.FirstName, it.SurName)
                                        })
                        });
            }

            return
                this.Json(
                    new
                    {
                        list =
                            performersList.OrderBy(it => it.SurName)
                                .ThenBy(it => it.FirstName)
                                .Select(
                                    it =>
                                    new IdName
                                    {
                                        id = it.Id,
                                        name = string.Format("{0} {1}", it.SurName, it.FirstName)
                                    })
                    });
        }

        private CaseRuleModel GetCaseRuleModel(int customerId, CaseRuleMode ruleMode,
                                               CaseSolution templateModel,
                                               List<CaseSolutionSettingModel> templateSettingModel,
                                               Setting customerSettings,
                                               List<CaseFieldSetting> caseFieldSettings)
        {
            var currentData = new CaseCurrentDataModel();

            #region Assign current data

            currentData.AgreedDate = templateModel.AgreedDate;
            currentData.Available = templateModel.Available;
            currentData.Caption = templateModel.Caption;
            currentData.CaseType_Id = templateModel.CaseType_Id;
            currentData.Category_Id = templateModel.Category_Id;
            currentData.CausingPart_Id = templateModel.CausingPartId;
            currentData.Change_Id = templateModel.Change_Id;
            currentData.ContactBeforeAction = templateModel.ContactBeforeAction;
            currentData.Cost = templateModel.Cost;
            currentData.CostCentre = templateModel.CostCentre;
            currentData.Currency = templateModel.Currency;
            currentData.Customer_Id = templateModel.Customer_Id;
            currentData.Department_Id = templateModel.Department_Id;
            currentData.Description = templateModel.Description;
            currentData.FinishingCause_Id = templateModel.FinishingCause_Id;
            currentData.FinishingDate = templateModel.FinishingDate;
            currentData.FinishingDescription = templateModel.FinishingDescription;
            currentData.Impact_Id = templateModel.Impact_Id;
            currentData.InventoryLocation = templateModel.InventoryLocation;
            currentData.InventoryNumber = templateModel.InventoryNumber;
            currentData.InventoryType = templateModel.InventoryType;
            currentData.InvoiceNumber = templateModel.InvoiceNumber;
            currentData.IsAbout_CostCentre = templateModel.IsAbout_CostCentre;
            currentData.IsAbout_Department_Id = templateModel.IsAbout_Department_Id;
            currentData.IsAbout_OU_Id = templateModel.IsAbout_OU_Id;
            currentData.IsAbout_PersonsCellPhone = templateModel.IsAbout_PersonsCellPhone;
            currentData.IsAbout_PersonsEmail = templateModel.IsAbout_PersonsEmail;
            currentData.IsAbout_PersonsName = templateModel.IsAbout_PersonsName;
            currentData.IsAbout_PersonsPhone = templateModel.IsAbout_PersonsPhone;
            currentData.IsAbout_Place = templateModel.IsAbout_Place;
            currentData.IsAbout_Region_Id = templateModel.IsAbout_Region_Id;
            currentData.IsAbout_ReportedBy = templateModel.IsAbout_ReportedBy;
            currentData.IsAbout_UserCode = templateModel.IsAbout_UserCode;
            currentData.Miscellaneous = templateModel.Miscellaneous;
            currentData.NoMailToNotifier = templateModel.NoMailToNotifier;
            currentData.OtherCost = templateModel.OtherCost;
            currentData.OU_Id = templateModel.OU_Id;
            currentData.PerformerUser_Id = templateModel.PerformerUser_Id;
            currentData.PersonsCellPhone = templateModel.PersonsCellPhone;
            currentData.PersonsEmail = templateModel.PersonsEmail;
            currentData.PersonsName = templateModel.PersonsName;
            currentData.PersonsPhone = templateModel.PersonsPhone;
            currentData.Place = templateModel.Place;
            currentData.PlanDate = templateModel.PlanDate;
            currentData.Priority_Id = templateModel.Priority_Id;
            currentData.Problem_Id = templateModel.Problem_Id;
            currentData.ProductArea_Id = templateModel.ProductArea_Id;
            currentData.Project_Id = templateModel.Project_Id;
            currentData.ReferenceNumber = templateModel.ReferenceNumber;
            currentData.Region_Id = templateModel.Region_Id;
            currentData.RegistrationSource = templateModel.RegistrationSource;
            currentData.ReportedBy = templateModel.ReportedBy;
            currentData.SMS = templateModel.SMS;
            currentData.SolutionRate = templateModel.SolutionRate;
            currentData.StateSecondary_Id = templateModel.StateSecondary_Id;
            currentData.Status_Id = templateModel.Status_Id;
            currentData.Supplier_Id = templateModel.Supplier_Id;
            currentData.System_Id = templateModel.System_Id;
            currentData.Text_External = templateModel.Text_External;
            currentData.Text_Internal = templateModel.Text_Internal;
            currentData.UpdateNotifierInformation = templateModel.UpdateNotifierInformation;
            currentData.Urgency_Id = templateModel.Urgency_Id;
            currentData.UserCode = templateModel.UserCode;
            currentData.Verified = templateModel.Verified;
            currentData.VerifiedDescription = templateModel.VerifiedDescription;
            currentData.WatchDate = templateModel.WatchDate;
            currentData.WorkingGroup_Id = templateModel.WorkingGroup_Id;

            #endregion

            _caseRuleFactory.Initialize(_regionService, _departmentService, _ouService,
                                       _registrationSourceCustomerService, _caseTypeService, _productAreaService,
                                       _systemService, _urgencyService, _impactService, _categoryService,
                                       _supplierService, _workingGroupService, _userService,
                                       _priorityService, _statusService, _stateSecondaryService,
                                       _projectService, _problemService, _causingPartService,
                                       _changeService, _finishingCauseService, _watchDateCalendarService);

            var model = _caseRuleFactory.GetCaseRuleModel(customerId, ruleMode, currentData, customerSettings, caseFieldSettings, templateSettingModel);

            return model;
        }

        #endregion

        #region Category

        public ActionResult NewCategory()
        {
            return this.View(new CaseSolutionCategory() { Customer_Id = SessionFacade.CurrentCustomer.Id });
        }

        [HttpPost]
        public ActionResult NewCategory(CaseSolutionCategory caseSolutionCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._caseSolutionService.SaveCaseSolutionCategory(caseSolutionCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "casesolution");

            return this.View(caseSolutionCategory);
        }

        public ActionResult EditCategory(int id)
        {
            var caseSolutionCategory = this._caseSolutionService.GetCaseSolutionCategory(id);

            if (caseSolutionCategory == null)
                return new HttpNotFoundResult("No case solution cateogry found...");

            return this.View(caseSolutionCategory);
        }

        [HttpPost]
        public ActionResult EditCategory(CaseSolutionCategory caseSolutionCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._caseSolutionService.SaveCaseSolutionCategory(caseSolutionCategory, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "caseSolution");

            return this.View(caseSolutionCategory);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            if (this._caseSolutionService.DeleteCaseSolutionCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "casesolution");
            else
            {
                this.TempData.Add("Error", "You cant delete an used building");
                return this.RedirectToAction("editcategory", "casesolution", new { id = id });
            }
        }

        #endregion

        #region Private Methods

        private CaseSolutionIndexViewModel CreateIndexViewModel(CaseSolutionSearch caseSolutionSearch)
        {
            var customerId = SessionFacade.CurrentCustomer.Id;
            var isUserFirstLastNameRepresentation = this._settingService.GetCustomerSetting(customerId)
                                                                        .IsUserFirstLastNameRepresentation == 1;

            ////Only return casesolution where templatepath is null - these case solutions are E-Forms shown in myhr/linemanager/selfservice
            var caseSolutions = this._caseSolutionService.SearchAndGenerateCaseSolutions(customerId, caseSolutionSearch, isUserFirstLastNameRepresentation)
                                                         .Where(x => x.TemplatePath == null).ToList();




            ////I have removed the  above condition, from now on these will appear in the list /TAN
            //var caseSolutions = this._caseSolutionService.SearchAndGenerateCaseSolutions(customerId, caseSolutionSearch, isUserFirstLastNameRepresentation).ToList();

            var curUserItem = string.Format("-- {0} --", Translation.GetCoreTextTranslation(CURRENT_USER_ITEM_CAPTION));
            var connectedToButton = Translation.GetCoreTextTranslation("Knapp");
            var _rows = caseSolutions.Select(cs => new RowIndexViewModel
            {
                Id = cs.Id,
                Name = cs.Name,
                CategoryName = cs.CaseSolutionCategory == null ? string.Empty : cs.CaseSolutionCategory.Name,
                CaseCaption = cs.Caption,
                PerformerUserName = cs.PerformerUser == null ?
                                                                                (cs.SetCurrentUserAsPerformer == 1 ? curUserItem : string.Empty) :
                                                                                (isUserFirstLastNameRepresentation ?
                                                                                    string.Format("{0} {1}", cs.PerformerUser.FirstName, cs.PerformerUser.SurName) :
                                                                                    string.Format("{0} {1}", cs.PerformerUser.SurName, cs.PerformerUser.FirstName)
                                                                                ),
                PriorityName = cs.Priority == null ? string.Empty : cs.Priority.Name,
                IsActive = (cs.Status != 0),
                ConnectedToButton = (cs.ConnectedButton.HasValue && cs.ConnectedButton == 0) ? Translation.GetCoreTextTranslation(DH.Helpdesk.Common.Constants.Text.WorkflowStep) : (cs.ConnectedButton.HasValue) ? connectedToButton + " " + cs.ConnectedButton.Value : "",
                SortOrder = cs.SortOrder
            }).ToArray();

            var activeTab = SessionFacade.FindActiveTab("CaseSolution");
            activeTab = activeTab ?? "CaseTemplate";

            var model = new CaseSolutionIndexViewModel(activeTab)
            {
                Rows = _rows,
                CaseSolutionCategories = this._caseSolutionService.GetCaseSolutionCategories(customerId)
            };

            return model;
        }



        private CaseSolutionInputViewModel CreateInputViewModel(CaseSolution caseSolution)
        {
            var curCustomerId = SessionFacade.CurrentCustomer.Id;
            var cs = this._settingService.GetCustomerSetting(curCustomerId);

            List<SelectListItem> regions = null;
            List<SelectListItem> departments = null;
            List<SelectListItem> organizationUnits = null;

            var regionList = this._regionService.GetActiveRegions(curCustomerId);

            ///////////////////////////////////////////////StateSecondaries/////////////////////////////////////////////////////////////////////////////////////////////////////////
            string selected = string.Empty;
            //IList<CaseSolutionCondition> stsec = _caseSolutionConditionService.GetStateSecondaries(caseSolution.Id, curCustomerId);
            string wgguid = "case_StateSecondary.StateSecondaryGUID";
            IList<CaseSolutionCondition> stsec = _caseSolutionConditionService.GetCaseSolutionConditionModel(caseSolution.Id, curCustomerId, wgguid);
            foreach (CaseSolutionCondition sb in stsec)
            {
                if (sb.IsSelected == 1)
                {
                    if (selected == string.Empty)
                    {
                        selected = sb.StateSecondaryGUID.ToString();
                    }
                    else
                    {
                        selected = selected + "," + sb.StateSecondaryGUID.ToString();
                    }
                }
            }

            List<SelectListItem> stateSecondaries = null;
            stateSecondaries = stsec
                  .Select(x => new SelectListItem
                  {
                      Text = x.Name,
                      Value = x.StateSecondaryGUID.ToString(),
                      Selected = x.IsSelected == 1 ? true : false
                  }).ToList();

            ViewBag.selectedStates = selected;
            ///////////////////////////////////////////////StateSecondaries/////////////////////////////////////////////////////////////////////////////////////////////////////////


            ///////////////////////////////////////////////CaseWorkingGroups/////////////////////////////////////////////////////////////////////////////////////////////////////////
            string selectedWgs = string.Empty;
            //IList<CaseSolutionCondition> wkgroup = _caseSolutionConditionService.GetCaseWorkingGroups(caseSolution.Id, curCustomerId);
            wgguid = "case_WorkingGroup.WorkingGroupGUID";
            IList<CaseSolutionCondition> wkgroup = _caseSolutionConditionService.GetCaseSolutionConditionModel(caseSolution.Id, curCustomerId, wgguid);
            foreach (CaseSolutionCondition sb in wkgroup)
            {
                if (sb.IsSelected == 1)
                {
                    if (selectedWgs == string.Empty)
                    {
                        selectedWgs = sb.StateSecondaryGUID.ToString();
                    }
                    else
                    {
                        selectedWgs = selectedWgs + "," + sb.StateSecondaryGUID.ToString();
                    }
                }
            }

            List<SelectListItem> caseWorkingGroups = null;
            caseWorkingGroups = wkgroup
                  .Select(x => new SelectListItem
                  {
                      Text = x.Name,
                      Value = x.StateSecondaryGUID.ToString(),
                      Selected = x.IsSelected == 1 ? true : false
                  }).ToList();

            ViewBag.selectedCaseWgs = selectedWgs;
            ///////////////////////////////////////////////CaseWorkingGroups/////////////////////////////////////////////////////////////////////////////////////////////////////////


            regions = regionList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = x.Id == caseSolution.Region_Id
            }).ToList();

            departments = this._departmentService.GetActiveDepartmentsBy(curCustomerId, caseSolution.Region_Id)
                                         .Select(x => new SelectListItem
                                         {
                                             Text = x.DepartmentName,
                                             Value = x.Id.ToString(),
                                             Selected = x.Id == caseSolution.Department_Id
                                         }).ToList();

            organizationUnits = this._organizationService.GetOrganizationUnits(caseSolution.Department_Id)
                                         .Select(x => new SelectListItem
                                         {
                                             Text = x.Name,
                                             Value = x.Value,
                                             Selected = x.Value == caseSolution.OU_Id?.ToString()
                                         }).ToList();

            List<SelectListItem> isAbout_Regions = null;
            List<SelectListItem> isAbout_Departments = null;
            List<SelectListItem> isAbout_OrganizationUnits = null;



            isAbout_Regions = regionList.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = x.Id == caseSolution.IsAbout_Region_Id
            }).ToList();


            isAbout_Departments = this._departmentService.GetActiveDepartmentsBy(curCustomerId, caseSolution.IsAbout_Region_Id)
                                         .Select(x => new SelectListItem
                                         {
                                             Text = x.DepartmentName,
                                             Value = x.Id.ToString(),
                                             Selected = x.Id == caseSolution.IsAbout_Department_Id
                                         }).ToList();

            isAbout_OrganizationUnits = this._organizationService.GetOrganizationUnits(caseSolution.IsAbout_Department_Id)
                                         .Select(x => new SelectListItem
                                         {
                                             Text = x.Name,
                                             Value = x.Value,
                                             Selected = x.Value == caseSolution.IsAbout_OU_Id?.ToString()
                                         }).ToList();

            var isCreatingNew = caseSolution.Id == 0;

            if (caseSolution.SetCurrentUserAsPerformer == 1)
                caseSolution.PerformerUser_Id = -1;

            if (caseSolution.SetCurrentUsersWorkingGroup == 1)
                caseSolution.CaseWorkingGroup_Id = -1;

            var performersList = isCreatingNew ?
                                     this._userService.GetAvailablePerformersOrUserId(curCustomerId)
                                         .MapToSelectList(cs, true, true)
                                     : this._userService.GetAvailablePerformersForWorkingGroup(
                                         curCustomerId,
                                         caseSolution.CaseWorkingGroup_Id).MapToSelectList(cs, true, true);
            const bool TakeOnlyActive = true;


            var workingGroupList = this._workingGroupService.GetAllWorkingGroupsForCustomer(curCustomerId).Select(x => new SelectListItem
            {
                Text = x.WorkingGroupName,
                Value = x.Id.ToString()
            }).ToList();

            var currentWG = new List<SelectListItem>();
            currentWG.Add(new SelectListItem
            {
                Text = string.Format("-- {0} --", Translation.GetCoreTextTranslation(CURRENT_USER_WORKINGGROUP_CAPTION)),
                Value = "-1"
            });

            workingGroupList = currentWG.Union(workingGroupList).ToList();

            var usedButtons = _caseSolutionService.GetCaseSolutions(curCustomerId)
                                                  .Where(c => c.ConnectedButton.HasValue && c.Id != caseSolution.Id)
                                                  .Select(c => c.ConnectedButton.Value).ToList();

            var buttonList = new List<SelectListItem>();

            var buttonCaption = Translation.GetCoreTextTranslation("Knapp");

            //Add workflow choise
            buttonList.Add(new SelectListItem()
            {
                Value = "0",
                Text = Translation.GetCoreTextTranslation(DH.Helpdesk.Common.Constants.Text.WorkflowStep),
                Selected = caseSolution.ConnectedButton == 0
            });

            for (var i = 1; i <= MAX_QUICK_BUTTONS_COUNT; i++)
            {
                if (!usedButtons.Contains(i))
                {
                    buttonList.Add(new SelectListItem()
                    {
                        Value = i.ToString(),
                        Text = string.Format("{0} {1}", buttonCaption, i),
                        Selected = caseSolution.ConnectedButton == i
                    });
                }
            }

            var actionList = new List<SelectListItem>();
            actionList.Add(new SelectListItem() { Value = "-1", Text = "", Selected = !caseSolution.SaveAndClose.HasValue });
            actionList.Add(new SelectListItem()
            {
                Value = "0",
                Text = Translation.GetCoreTextTranslation("Spara"),
                Selected = caseSolution.SaveAndClose.HasValue && caseSolution.SaveAndClose.Value == 0
            });

            actionList.Add(new SelectListItem()
            {
                Value = "1",
                Text = Translation.GetCoreTextTranslation("Spara och stäng"),
                Selected = caseSolution.SaveAndClose.HasValue && caseSolution.SaveAndClose.Value != 0
            });

            var model = new CaseSolutionInputViewModel
            {
                CaseSolution = caseSolution,
                CaseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(curCustomerId),
                Countries = this._countryService.GetCountries(curCustomerId),
                currencies = this._currencyService.GetCurrencies(),
                CsCategories = this._caseSolutionService.GetCaseSolutionCategories(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseTypes = this._caseTypeService.GetCaseTypes(curCustomerId, TakeOnlyActive),

                CaseWorkingGroups = workingGroupList,

                Categories = this._categoryService.GetActiveParentCategories(curCustomerId),

                FinishingCauses = this._finishingCauseService.GetFinishingCauses(curCustomerId),

                PerformerUsers = performersList,

                Priorities = this._priorityService.GetPriorities(curCustomerId).Where(x => x.IsActive == 1).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                ProductAreas = this._productAreaService.GetTopProductAreasForUser(curCustomerId, SessionFacade.CurrentUser),

                WorkingGroups = this._workingGroupService.GetWorkingGroups(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                Regions = regions,

                Departments = departments,

                OUs = organizationUnits,

                IsAbout_Regions = isAbout_Regions,

                IsAbout_Departments = isAbout_Departments,

                IsAbout_OUs = isAbout_OrganizationUnits,

                Systems = this._systemService.GetSystems(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.SystemName,
                    Value = x.Id.ToString()
                }).ToList(),

                Urgencies = this._urgencyService.GetUrgencies(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Impacts = this._impactService.GetImpacts(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Status = this._statusService.GetActiveStatuses(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                StateSecondaries = this._stateSecondaryService.GetActiveStateSecondaries(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Suppliers = this._supplierService.GetActiveSuppliers(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CausingParts = GetCausingPartsModel(curCustomerId, caseSolution.CausingPartId),

                RegistrationSources = this._registrationSourceCustomerService.GetCustomersActiveRegistrationSources(curCustomerId).Select(x => new SelectListItem
                {
                    Text = Translation.Get(x.SourceName),
                    Value = x.Id.ToString()
                }).ToList(),

                ButtonList = buttonList,

                ActionList = actionList,

                StateSecondariesSelect = stateSecondaries,
                CaseWorkingGroupSelect = caseWorkingGroups
            };

            model.ParantPath_Category = "--";
            if (caseSolution.Category_Id.HasValue)
            {
                var c = this._categoryService.GetCategory(caseSolution.Category_Id.Value, curCustomerId);
                if (c != null)
                    model.ParantPath_Category = string.Join(" - ", this._categoryService.GetParentPath(c.Id, curCustomerId));
            }

            if (model.CaseSolution.Id == 0)
            {
                var defaultCat = _caseSolutionService.GetCaseSolutionCategories(curCustomerId).Where(c => c.IsDefault == 1).FirstOrDefault();
                if (defaultCat != null)
                    model.CaseSolution.CaseSolutionCategory_Id = defaultCat.Id;
            }

            model.ParantPath_CaseType = "--";
            if ((model.CaseSolution.CaseType_Id.HasValue))
            {
                var c = this._caseTypeService.GetCaseType(model.CaseSolution.CaseType_Id.Value);
                if (c != null)
                    model.ParantPath_CaseType = c.getCaseTypeParentPath();
            }

            model.Finishing_Cause_Path = "--";
            if (caseSolution.FinishingCause_Id.HasValue)
            {
                var c = this._finishingCauseService.GetFinishingCause(caseSolution.FinishingCause_Id.Value);
                if (c != null)
                    model.Finishing_Cause_Path = c.getFinishingCauseParentPath();
            }


            model.ParantPath_ProductArea = "--";
            if (caseSolution.ProductArea_Id.HasValue)
            {
                var c = this._productAreaService.GetProductArea(caseSolution.ProductArea_Id.Value);
                if (c != null)
                    model.ParantPath_ProductArea = string.Join(" - ", this._productAreaService.GetParentPath(c.Id, curCustomerId));
            }

            if (cs.ModuleProject == 1)
            {
                model.projects = this._projectService.GetCustomerProjects(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();

            }

            if (cs.ModuleProblem == 1)
            {
                model.problems = this._problemService.GetCustomerProblems(curCustomerId, false).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            }

            if (cs.ModuleChangeManagement == 1)
            {
                model.changes = this._changeService.GetChanges(curCustomerId).Select(x => new SelectListItem
                {
                    Text = x.ChangeTitle,
                    Value = x.Id.ToString()
                }).ToList();
            }
            //var deps = this._departmentService.GetDepartmentsByUserPermissions(SessionFacade.CurrentUser.Id, SessionFacade.CurrentCustomer.Id);
            //model.departments = deps ?? this._departmentService.GetDepartments(SessionFacade.CurrentCustomer.Id);


            model.Schedule = 0;

            var schedule = this._caseSolutionService.GetCaseSolutionSchedule(caseSolution.Id);

            model.ScheduleDays = string.Empty;
            model.ScheduleMonths = string.Empty;
            model.Schedule = 0;

            if (schedule != null)
            {
                model.Schedule = 1;
                model.ScheduleTime = (int)schedule.ScheduleTime;
                model.ScheduleType = schedule.ScheduleType;
                model.ScheduleWatchDate = schedule.ScheduleWatchDate;

                model.ScheduleMonths = string.Empty;
                model.ScheduleDays = string.Empty;

                if (schedule.ScheduleDay != null)
                    model.ScheduleDays = schedule.ScheduleDay;

                int pos = 0;
                pos = model.ScheduleDays.IndexOf(";", 0);

                if (pos > 0)
                {
                    model.ScheduleMonths = model.ScheduleDays.Substring(pos + 1);
                    model.ScheduleDays = model.ScheduleDays.Substring(0, pos);
                }

                pos = model.ScheduleDays.IndexOf(":", 0);

                if (pos > 0)
                {
                    model.ScheduleMonthlyOrder = int.Parse(model.ScheduleDays.Substring(0, pos));
                    model.ScheduleMonthlyWeekday = int.Parse(model.ScheduleDays.Substring(pos + 1, 1));
                }
            }

            ReadOnlyCollection<CaseSolutionSettingOverview> settingOverviews =
                    this.caseSolutionSettingService.GetCaseSolutionSettingOverviews(model.CaseSolution.Id);

            if (settingOverviews.Any())
            {
                model.CaseSolutionSettingModels = CaseSolutionSettingModel.CreateModel(settingOverviews);
            }

            model.CaseFilesModel = new CaseFilesModel();

            var caseRuleModel = GetCaseRuleModel(curCustomerId, CaseRuleMode.TemplateUserChangeMode, caseSolution,
                                                 model.CaseSolutionSettingModels.ToList(), cs,
                                                 model.CaseFieldSettings.ToList());

            model.RuleModel = caseRuleModel;
            model.CaseSolution.Name = model.CaseSolution.Name;
            return model;
        }

        private List<SelectListItem> GetCausingPartsModel(int customerId, int? curCausingPartId)
        {
            var allActiveCausinParts = this._causingPartService.GetActiveParentCausingParts(customerId, curCausingPartId);
            var ret = new List<SelectListItem>();

            var parentRet = new List<SelectListItem>();
            var childrenRet = new List<SelectListItem>();

            foreach (var causingPart in allActiveCausinParts)
            {
                var curName = string.Empty;
                if (causingPart.Parent != null && curCausingPartId.HasValue && causingPart.Id == curCausingPartId.Value)
                {
                    curName = string.Format("{0} - {1}", Translation.Get(causingPart.Parent.Name, Enums.TranslationSource.TextTranslation, customerId),
                                                         Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId));

                    childrenRet.Add(new SelectListItem() { Value = causingPart.Id.ToString(), Text = curName, Selected = true });
                }
                else
                {
                    if (causingPart.Children.Any())
                    {
                        foreach (var child in causingPart.Children)
                        {
                            if (child.IsActive)
                            {
                                curName = string.Format("{0} - {1}", Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId),
                                                                     Translation.Get(child.Name, Enums.TranslationSource.TextTranslation, customerId));

                                var isSelected = (child.Id == curCausingPartId);
                                childrenRet.Add(new SelectListItem() { Value = child.Id.ToString(), Text = curName, Selected = isSelected });
                            }
                        }
                    }
                    else
                    {
                        curName = Translation.Get(causingPart.Name, Enums.TranslationSource.TextTranslation, customerId);
                        var isSelected = (causingPart.Id == curCausingPartId);
                        parentRet.Add(new SelectListItem() { Value = causingPart.Id.ToString(), Text = curName, Selected = isSelected });
                    }
                }
            }

            ret = parentRet.OrderBy(p => p.Text).Union(childrenRet.OrderBy(c => c.Text)).ToList();

            return ret;

        }

        private CaseSettingsSolutionAggregate CreateCaseSettingsSolutionAggregate(
            int caseSolutionId,
            IEnumerable<CaseSolutionSettingModel> caseSolutionSettingModels)
        {
            var businessModels = new List<CaseSolutionSettingForWrite>();
            foreach (CaseSolutionSettingModel x in caseSolutionSettingModels)
            {
                var businessModel = new CaseSolutionSettingForWrite(x.CaseSolutionField, x.CaseSolutionMode);
                if (x.Id != 0)
                {
                    businessModel.Id = x.Id;
                }

                businessModels.Add(businessModel);
            }

            return this.CreateCaseSettingsSolutionAggregate(caseSolutionId, businessModels);
        }

        private CaseSettingsSolutionAggregate CreateCaseSettingsSolutionAggregate(int caseSolutionId, List<CaseSolutionSettingForWrite> businessModels)
        {
            var req = new CaseSettingsSolutionAggregate(caseSolutionId, businessModels, this.OperationContext);
            return req;
        }

        private CaseSolutionSchedule CreateCaseSolutionSchedule(CaseSolutionInputViewModel caseSolutionInputViewModel)
        {
            int caseSolutionId = caseSolutionInputViewModel.CaseSolution.Id;
            int scheduleMonthly = caseSolutionInputViewModel.ScheduleMonthly;
            int scheduleType = caseSolutionInputViewModel.ScheduleType;
            int scheduleTime = caseSolutionInputViewModel.ScheduleTime;
            int scheduleWatchDate = caseSolutionInputViewModel.ScheduleWatchDate;
            string[] scheduleDay = caseSolutionInputViewModel.ScheduleDay;
            string[] scheduleMonth = caseSolutionInputViewModel.ScheduleMonth;
            int scheduleMonthlyDay = caseSolutionInputViewModel.ScheduleMonthlyDay;
            int scheduleMonthlyOrder = caseSolutionInputViewModel.ScheduleMonthlyOrder;
            int scheduleMonthlyWeekday = caseSolutionInputViewModel.ScheduleMonthlyWeekday;

            var caseSolutionSchedule = new CaseSolutionSchedule();


            if (caseSolutionInputViewModel.Schedule != 0)
            {
                caseSolutionSchedule.CaseSolution_Id = caseSolutionId;
                caseSolutionSchedule.ScheduleType = scheduleType;
                caseSolutionSchedule.ScheduleTime = scheduleTime;
                caseSolutionSchedule.ScheduleWatchDate = scheduleWatchDate;

                if (scheduleType == 1)
                    caseSolutionSchedule.ScheduleDay = null;

                if (scheduleType == 2 && scheduleDay != null)
                {
                    caseSolutionSchedule.ScheduleDay = "," + string.Join(",", scheduleDay) + ",";
                }

                if (scheduleType == 3)
                {
                    if (scheduleMonthly == 1 && scheduleMonth != null)
                    {
                        caseSolutionSchedule.ScheduleDay = scheduleMonthlyDay + ";," + string.Join(",", scheduleMonth) + ",";
                    }

                    if (scheduleMonthly == 2 && scheduleMonth != null)
                    {
                        caseSolutionSchedule.ScheduleDay = scheduleMonthlyOrder + ":" + scheduleMonthlyWeekday + ";," + string.Join(",", scheduleMonth) + ",";
                    }
                }
            }
            else
                caseSolutionSchedule = null;


            return caseSolutionSchedule;
        }

        #endregion
    }
}
