
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
    using System.Web;

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
                SortBy = CaseSolutionIndexColumns.Name,
                OnlyActive = true
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
        public PartialViewResult Search(string searchText, List<int> categoryIds, List<string> subStatusIds, List<string> WgroupIds, List<string> PriorityIds, List<string> StatusIds, List<string> ProductAreaIds, List<string> UserWGroupIds, List<string> TemplateProductAreaIds, List<string> ApplicationIds, string Actives, string ImgClass)
        {
            bool act = false;
            if (Actives == "true")
            {
                act = true;
            }

            if (categoryIds != null)
            {
                if (categoryIds.Count() == 1 && (categoryIds[0] == 0 | categoryIds[0].ToString() == ""))
                {
                    categoryIds = null;
                }
            }

            if (subStatusIds != null)
            {
                if (subStatusIds.Count() == 1 && (subStatusIds[0] == "0" | subStatusIds[0].ToString() == ""))
                {
                    subStatusIds = null;
                }
            }

            if (WgroupIds != null)
            {
                if (WgroupIds.Count() == 1 && (WgroupIds[0] == "0" | WgroupIds[0].ToString() == ""))
                {
                    WgroupIds = null;
                }
            }

            if (PriorityIds != null)
            {
                if (PriorityIds.Count() == 1 && (PriorityIds[0] == "0" | PriorityIds[0].ToString() == ""))
                {
                    PriorityIds = null;
                }
            }

            if (StatusIds != null)
            {
                if (StatusIds.Count() == 1 && (StatusIds[0] == "0" | StatusIds[0].ToString() == ""))
                {
                    StatusIds = null;
                }
            }

            if (ProductAreaIds != null)
            {
                if (ProductAreaIds.Count() == 1 && (ProductAreaIds[0] == "0" | ProductAreaIds[0].ToString() == ""))
                {
                    ProductAreaIds = null;
                }
            }

            if (UserWGroupIds != null)
            {
                if (UserWGroupIds.Count() == 1 && (UserWGroupIds[0] == "0" | UserWGroupIds[0].ToString() == ""))
                {
                    UserWGroupIds = null;
                }
            }

            if (TemplateProductAreaIds != null)
            {
                if (TemplateProductAreaIds.Count() == 1 && (TemplateProductAreaIds[0] == "0" | TemplateProductAreaIds[0].ToString() == ""))
                {
                    TemplateProductAreaIds = null;
                }
            }

            if (ApplicationIds != null)
            {
                if (ApplicationIds.Count() == 1 && (ApplicationIds[0] == "0" | ApplicationIds[0].ToString() == ""))
                {
                    ApplicationIds = null;
                }
            }

            var caseSolutionSearch = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                caseSolutionSearch = SessionFacade.CurrentCaseSolutionSearch;

            caseSolutionSearch.SearchCss = searchText;


            if (categoryIds != null)
            {
                caseSolutionSearch.CategoryIds = categoryIds.ToList();
            }

            if (subStatusIds != null)
            {
                caseSolutionSearch.SubStatusIds = subStatusIds.ToList();
            }

            if (WgroupIds != null)
            {
                caseSolutionSearch.WgroupIds = WgroupIds.ToList();
            }

            if (PriorityIds != null)
            {
                caseSolutionSearch.PriorityIds = PriorityIds.ToList();
            }

            if (StatusIds != null)
            {
                caseSolutionSearch.StatusIds = StatusIds.ToList();
            }

            if (ProductAreaIds != null)
            {
                caseSolutionSearch.ProductAreaIds = ProductAreaIds.ToList();
            }

            if (UserWGroupIds != null)
            {
                caseSolutionSearch.UserWGroupIds = UserWGroupIds.ToList();
            }

            if (TemplateProductAreaIds != null)
            {
                caseSolutionSearch.TemplateProductAreaIds = TemplateProductAreaIds.ToList();
            }

            if (ApplicationIds != null)
            {
                caseSolutionSearch.ApplicationIds = ApplicationIds.ToList();
            }


            caseSolutionSearch.OnlyActive = act;
            caseSolutionSearch.ImageClass = ImgClass;

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

            TempData["NewOrOld"] = "0";
            //ViewBag.IdVal = 0;
            var model = this.CreateInputViewModel(caseSolution);

            return this.View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult New(CaseSolution caseSolution, CaseSolutionInputViewModel caseSolutionInputViewModel, CaseSolutionSettingModel[] caseSolutionSettingModels, int PageId, string selectedValues)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            IList<CaseFieldSetting> CheckMandatory = null;//_caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id);
            this.TempData["RequiredFields"] = null;


            //string t = this.TempData["NewOrOld"].ToString();

            //if (t != "0")
            //{
            //    caseSolution.Id = Convert.ToInt32(t.ToString());
            //    caseSolutionInputViewModel.CaseSolution.Id = Convert.ToInt32(t.ToString());

            //}

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

             if (caseSolutionInputViewModel.SplitToCaseSolutionIds != null)
             {
                 caseSolutionInputViewModel.CaseSolution.SplitToCaseSolutionDescendants = _caseSolutionService.GetSplitToCaseSolutionDescendants(caseSolutionInputViewModel.CaseSolution, caseSolutionInputViewModel.SplitToCaseSolutionIds);
             }
         

            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            //if (t == "0")
            //{
            CaseSettingsSolutionAggregate settingsSolutionAggregate = this.CreateCaseSettingsSolutionAggregate(caseSolutionInputViewModel.CaseSolution.Id, caseSolutionSettingModels);
            this.caseSolutionSettingService.AddCaseSolutionSettings(settingsSolutionAggregate);
            //}


            ////////////////Save conditions//////////////////
            List<CaseSolitionConditionListEntity> cse = new List<CaseSolitionConditionListEntity>();

            string[] selectedSplit = selectedValues.Split('~');
            foreach (string s in selectedSplit)
            {
                string[] cap = s.Split(':');
                string text = cap[0].ToString();
                string values = string.Empty;
                if (cap.Count() > 1)
                {
                    values = cap[1].ToString();
                }
                bool exists = false;

                exists = cse.Any(u => u.CaseSolutionConditionCaption == text);
                if (exists == false)
                {
                    CaseSolitionConditionListEntity cSc = new CaseSolitionConditionListEntity
                    {
                        CaseSolutionConditionCaption = text,
                        CaseSolutionConditionValues = values.Replace('_', ',')
                    };

                    cse.Add(cSc);
                }
                else
                {



                    string exvalues = cse.Where(x => x.CaseSolutionConditionCaption == text).FirstOrDefault().CaseSolutionConditionValues;
                    string exid = cse.Where(x => x.CaseSolutionConditionCaption == text).FirstOrDefault().CaseSolutionConditionCaption;

                    string[] existarray = exvalues.Split(',');
                    string[] newarray = values.Split(',');
                    string[] result = existarray.Union(newarray).ToArray();
                    string final = string.Empty;
                    foreach (string word in result)
                    {
                        if (final == string.Empty)
                        {
                            final = word;
                        }
                        else
                        {
                            final = final + "," + word;
                        }
                    }



                    foreach (var item in cse.Where(w => w.CaseSolutionConditionCaption == text))
                    {
                        item.CaseSolutionConditionValues = final;
                    }



                }


            }
            if (cse != null)
            {
                foreach (CaseSolitionConditionListEntity lk in cse)
                {
                    CaseSolutionConditionEntity o = new CaseSolutionConditionEntity
                    {
                        CaseSolution_Id = caseSolutionInputViewModel.CaseSolution.Id,
                        Property_Name = lk.CaseSolutionConditionCaption,
                        Values = lk.CaseSolutionConditionValues
                    };
                    this._caseSolutionConditionService.Save(o);
                }
            }

            /////////////////////////////////////////////////

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



        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NewAndEdit(FormCollection collection)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            IList<CaseFieldSetting> CheckMandatory = null;//_caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id);

            CaseSolutionSettingModel[] caseSolutionSettingModels = new CaseSolutionSettingModel[0];

            string cost = Convert.ToString(collection["CaseSolution.Cost"].ToString());
            string selvals = Convert.ToString(collection["lstAddFieldSettings"].ToString());


            string caseSolutionId = Convert.ToString(collection["CaseSolution.Id"].ToString());
            string scheduleMonthly = Convert.ToString(collection["ScheduleMonthly"].ToString());
            int scheduleType = 0;// Convert.ToString(collection["ScheduleType"].ToString());
            int scheduleTime = 0; // Convert.ToString(collection["ScheduleTime"].ToString()); 
            int scheduleWatchDate = 0;// Convert.ToString(collection["ScheduleWatchDate"].ToString());
            string[] scheduleDay = null;// = Convert.ToString(collection["ScheduleDay"].ToString());
            string[] scheduleMonth = null;// = Convert.ToString(collection["ScheduleMonth"].ToString());
            string scheduleMonthlyDay = Convert.ToString(collection["ScheduleMonthlyDay"].ToString());
            int scheduleMonthlyOrder = 0;// Convert.ToString(collection["ScheduleMonthlyOrder"].ToString());
            int scheduleMonthlyWeekday = 0;// Convert.ToString(collection["ScheduleMonthlyWeekday"].ToString());
            string Schedule = Convert.ToString(collection["Schedule"].ToString());

            var caseSolutionSchedule = new CaseSolutionSchedule();


            if (Convert.ToInt32(Schedule) != 0)
            {
                caseSolutionSchedule.CaseSolution_Id = Convert.ToInt32(caseSolutionId);
                caseSolutionSchedule.ScheduleType = Convert.ToInt32(scheduleType);
                caseSolutionSchedule.ScheduleTime = Convert.ToInt32(scheduleTime);
                caseSolutionSchedule.ScheduleWatchDate = Convert.ToInt32(scheduleWatchDate);

                if (Convert.ToInt32(scheduleType) == 1)
                    caseSolutionSchedule.ScheduleDay = null;

                if (Convert.ToInt32(scheduleType) == 2 && scheduleDay != null)
                {
                    caseSolutionSchedule.ScheduleDay = "," + string.Join(",", scheduleDay) + ",";
                }

                if (Convert.ToInt32(scheduleType) == 3)
                {
                    if (Convert.ToInt32(scheduleMonthly) == 1 && scheduleMonth != null)
                    {
                        caseSolutionSchedule.ScheduleDay = scheduleMonthlyDay + ";," + string.Join(",", scheduleMonth) + ",";
                    }

                    if (Convert.ToInt32(scheduleMonthly) == 2 && scheduleMonth != null)
                    {
                        caseSolutionSchedule.ScheduleDay = scheduleMonthlyOrder + ":" + scheduleMonthlyWeekday + ";," + string.Join(",", scheduleMonth) + ",";
                    }
                }
            }
            else
            {
                caseSolutionSchedule = null;
            }

            CaseSolutionInputViewModel caseSolutionInputViewModel = new CaseSolutionInputViewModel();
            caseSolutionInputViewModel.CaseSolution = new CaseSolution();
            string NoMailToNotifier = Convert.ToString(collection["Casesolution.NoMailToNotifier"].ToString());
            NoMailToNotifier = NoMailToNotifier.Replace(",", ".");
            int pos = NoMailToNotifier.IndexOf(".");
            if (pos > 0)
            {
                NoMailToNotifier = NoMailToNotifier.Substring(0, pos);
            }
            if (NoMailToNotifier == string.Empty)
            {
                NoMailToNotifier = "0";
            }
            // Positive: Send Mail to...

            if (Convert.ToInt32(NoMailToNotifier) == 0)
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 1;
            else
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 0;


            string PerformerUser_Id = Convert.ToString(collection["Casesolution.PerformerUser_Id"].ToString());
            PerformerUser_Id = PerformerUser_Id.Replace(",", ".");
            if (PerformerUser_Id == string.Empty)
            {
                PerformerUser_Id = "0";
            }
            if (Convert.ToInt32(PerformerUser_Id) == -1)
            {
                caseSolutionInputViewModel.CaseSolution.PerformerUser_Id = null;
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = 1;
            }
            else
            {
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = null;
            }

            string CaseWorkingGroup_Id = Convert.ToString(collection["Casesolution.CaseWorkingGroup_Id"].ToString());
            CaseWorkingGroup_Id = CaseWorkingGroup_Id.Replace(",", ".");
            if (CaseWorkingGroup_Id == string.Empty)
            {
                CaseWorkingGroup_Id = "0";
            }
            if (Convert.ToInt32(CaseWorkingGroup_Id) == -1)
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



            /////SAve fields
            string Caption = Convert.ToString(collection["Casesolution.Caption"].ToString());
            caseSolutionInputViewModel.CaseSolution.Caption = Caption ?? string.Empty;

            string Description = Convert.ToString(collection["Casesolution.Description"].ToString());
            caseSolutionInputViewModel.CaseSolution.Description = Description ?? string.Empty;

            string Miscellaneous = Convert.ToString(collection["Casesolution.Miscellaneous"].ToString());
            caseSolutionInputViewModel.CaseSolution.Miscellaneous = Miscellaneous ?? string.Empty;

            string ReportedBy = Convert.ToString(collection["Casesolution.ReportedBy"].ToString());
            caseSolutionInputViewModel.CaseSolution.ReportedBy = ReportedBy ?? string.Empty;

            string Text_External = Convert.ToString(collection["Casesolution.Text_External"].ToString());
            caseSolutionInputViewModel.CaseSolution.Text_External = Text_External ?? string.Empty;

            string Text_Internal = Convert.ToString(collection["Casesolution.Text_Internal"].ToString());
            caseSolutionInputViewModel.CaseSolution.Text_Internal = Text_Internal ?? string.Empty;

            string PersonsName = Convert.ToString(collection["Casesolution.PersonsName"].ToString());
            caseSolutionInputViewModel.CaseSolution.PersonsName = PersonsName ?? string.Empty;

            string PersonsPhone = Convert.ToString(collection["Casesolution.PersonsPhone"].ToString());
            caseSolutionInputViewModel.CaseSolution.PersonsPhone = PersonsPhone ?? string.Empty;

            string PersonsEmail = Convert.ToString(collection["Casesolution.PersonsEmail"].ToString());
            caseSolutionInputViewModel.CaseSolution.PersonsEmail = PersonsEmail ?? string.Empty;

            string Place = Convert.ToString(collection["Casesolution.Place"].ToString());
            caseSolutionInputViewModel.CaseSolution.Place = Place ?? string.Empty;

            string UserCode = Convert.ToString(collection["Casesolution.UserCode"].ToString());
            caseSolutionInputViewModel.CaseSolution.UserCode = UserCode ?? string.Empty;

            string InvoiceNumber = Convert.ToString(collection["Casesolution.InvoiceNumber"].ToString());
            caseSolutionInputViewModel.CaseSolution.InvoiceNumber = InvoiceNumber ?? string.Empty;

            string ReferenceNumber = Convert.ToString(collection["Casesolution.ReferenceNumber"].ToString());
            caseSolutionInputViewModel.CaseSolution.ReferenceNumber = ReferenceNumber ?? string.Empty;

            string VerifiedDescription = Convert.ToString(collection["Casesolution.VerifiedDescription"].ToString());
            caseSolutionInputViewModel.CaseSolution.VerifiedDescription = VerifiedDescription ?? string.Empty;

            string SolutionRate = Convert.ToString(collection["Casesolution.SolutionRate"].ToString());
            caseSolutionInputViewModel.CaseSolution.SolutionRate = SolutionRate ?? string.Empty;

            string InventoryNumber = Convert.ToString(collection["Casesolution.InventoryNumber"].ToString());
            caseSolutionInputViewModel.CaseSolution.InventoryNumber = InventoryNumber ?? string.Empty;

            string InventoryType = Convert.ToString(collection["Casesolution.InventoryType"].ToString());
            caseSolutionInputViewModel.CaseSolution.InventoryType = InventoryType ?? string.Empty;

            string InventoryLocation = Convert.ToString(collection["Casesolution.InventoryLocation"].ToString());
            caseSolutionInputViewModel.CaseSolution.InventoryLocation = InventoryLocation ?? string.Empty;

            string Available = Convert.ToString(collection["Casesolution.Available"].ToString());
            caseSolutionInputViewModel.CaseSolution.Available = Available ?? string.Empty;

            string Currency = Convert.ToString(collection["Casesolution.Currency"].ToString());
            caseSolutionInputViewModel.CaseSolution.Currency = Currency ?? string.Empty;

            if (caseSolutionInputViewModel.CaseSolution.Text_External != null && caseSolutionInputViewModel.CaseSolution.Text_External.Length > 3000)
                caseSolutionInputViewModel.CaseSolution.Text_External = caseSolutionInputViewModel.CaseSolution.Text_External.Substring(0, 3000);

            if (caseSolutionInputViewModel.CaseSolution.Text_Internal != null && caseSolutionInputViewModel.CaseSolution.Text_Internal.Length > 3000)
                caseSolutionInputViewModel.CaseSolution.Text_Internal = caseSolutionInputViewModel.CaseSolution.Text_Internal.Substring(0, 3000);



            if (collection["CaseSolution.AgreedDate"].ToString().Trim() != string.Empty)
            {
                string AgreedDate = Convert.ToString(collection["CaseSolution.AgreedDate"].ToString());

                caseSolutionInputViewModel.CaseSolution.AgreedDate = Convert.ToDateTime(AgreedDate);
            }

            if (collection["CaseSolution.CaseSolutionCategory_Id"].ToString().Trim() != string.Empty)
            {
                int CaseSolutionCategory_Id = Convert.ToInt32(collection["CaseSolution.CaseSolutionCategory_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.CaseSolutionCategory_Id = CaseSolutionCategory_Id;
            }

            if (collection["CaseSolution.CaseType_Id"].ToString().Trim() != string.Empty)
            {
                int CaseType_Id = Convert.ToInt32(collection["CaseSolution.CaseType_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.CaseType_Id = CaseType_Id;
            }

            if (collection["CaseSolution.Category_Id"].ToString().Trim() != string.Empty)
            {
                int Category_Id = Convert.ToInt32(collection["CaseSolution.Category_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Category_Id = Category_Id;
            }

            if (collection["CaseSolution.CausingPartId"].ToString().Trim() != string.Empty)
            {
                int CausingPartId = Convert.ToInt32(collection["CaseSolution.CausingPartId"].ToString());
                caseSolutionInputViewModel.CaseSolution.CausingPartId = CausingPartId;
            }

            if (collection["CaseSolution.Change_Id"].ToString().Trim() != string.Empty)
            {
                int Change_Id = Convert.ToInt32(collection["CaseSolution.Change_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Change_Id = Change_Id;
            }

            caseSolutionInputViewModel.CaseSolution.ChangedDate = DateTime.Now;


            if (collection["CaseSolution.ConnectedButton"].ToString().Trim() != string.Empty)
            {
                int ConnectedButton = Convert.ToInt32(collection["CaseSolution.ConnectedButton"].ToString());
                caseSolutionInputViewModel.CaseSolution.ConnectedButton = ConnectedButton;
            }

            if (collection["CaseSolution.ContactBeforeAction"].ToString().Trim() != string.Empty)
            {
                int ContactBeforeAction = Convert.ToInt32(collection["CaseSolution.ContactBeforeAction"].ToString());
                caseSolutionInputViewModel.CaseSolution.ContactBeforeAction = ContactBeforeAction;
            }

            if (collection["CaseSolution.Cost"].ToString().Trim() != string.Empty)
            {
                int Cost = Convert.ToInt32(collection["CaseSolution.Cost"].ToString());
                caseSolutionInputViewModel.CaseSolution.Cost = Cost;
            }

            if (collection["CaseSolution.CostCentre"].ToString().Trim() != string.Empty)
            {
                string CostCentre = Convert.ToString(collection["CaseSolution.CostCentre"].ToString());
                caseSolutionInputViewModel.CaseSolution.CostCentre = CostCentre;
            }



            caseSolutionInputViewModel.CaseSolution.CreatedDate = DateTime.Now;


            if (collection["CaseSolution.Customer_Id"].ToString().Trim() != string.Empty)
            {
                int Customer_Id = Convert.ToInt32(collection["CaseSolution.Customer_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Customer_Id = Customer_Id;
            }

            if (collection["CaseSolution.Department_Id"].ToString().Trim() != string.Empty)
            {
                int Department_Id = Convert.ToInt32(collection["CaseSolution.Department_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Department_Id = Department_Id;
            }

            if (collection["CaseSolution.FinishingCause_Id"].ToString().Trim() != string.Empty)
            {
                int FinishingCause_Id = Convert.ToInt32(collection["CaseSolution.FinishingCause_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.FinishingCause_Id = FinishingCause_Id;
            }

            if (collection["CaseSolution.FinishingDate"].ToString().Trim() != string.Empty)
            {
                string FinishingDate = Convert.ToString(collection["CaseSolution.FinishingDate"].ToString());
                caseSolutionInputViewModel.CaseSolution.FinishingDate = Convert.ToDateTime(FinishingDate);
            }

            if (collection["CaseSolution.FinishingDescription"].ToString().Trim() != string.Empty)
            {
                string FinishingDescription = Convert.ToString(collection["CaseSolution.FinishingDescription"].ToString());
                caseSolutionInputViewModel.CaseSolution.FinishingDescription = FinishingDescription;
            }

            if (collection["CaseSolution.Id"].ToString().Trim() != string.Empty)
            {
                int Id = Convert.ToInt32(collection["CaseSolution.Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Id = Id;
            }

            if (collection["CaseSolution.Impact_Id"].ToString().Trim() != string.Empty)
            {
                int Impact_Id = Convert.ToInt32(collection["CaseSolution.Impact_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Impact_Id = Impact_Id;
            }

            if (collection["CaseSolution.Information"].ToString().Trim() != string.Empty)
            {
                string Information = Convert.ToString(collection["CaseSolution.Information"].ToString());
                caseSolutionInputViewModel.CaseSolution.Information = Information;
            }

            if (collection["CaseSolution.IsAbout_CostCentre"].ToString().Trim() != string.Empty)
            {
                string IsAbout_CostCentre = Convert.ToString(collection["CaseSolution.IsAbout_CostCentre"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_CostCentre = IsAbout_CostCentre;
            }

            if (collection["CaseSolution.IsAbout_Department_Id"].ToString().Trim() != string.Empty)
            {
                int IsAbout_Department_Id = Convert.ToInt32(collection["CaseSolution.IsAbout_Department_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_Department_Id = IsAbout_Department_Id;
            }

            if (collection["CaseSolution.IsAbout_OU_Id"].ToString().Trim() != string.Empty)
            {
                int IsAbout_OU_Id = Convert.ToInt32(collection["CaseSolution.IsAbout_OU_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_OU_Id = IsAbout_OU_Id;
            }

            if (collection["CaseSolution.IsAbout_PersonsCellPhone"].ToString().Trim() != string.Empty)
            {
                int IsAbout_PersonsCellPhone = Convert.ToInt32(collection["CaseSolution.IsAbout_PersonsCellPhone"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsCellPhone = IsAbout_PersonsCellPhone.ToString();
            }

            if (collection["CaseSolution.IsAbout_PersonsEmail"].ToString().Trim() != string.Empty)
            {
                int IsAbout_PersonsEmail = Convert.ToInt32(collection["CaseSolution.IsAbout_PersonsEmail"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsEmail = IsAbout_PersonsEmail.ToString();
            }
            if (collection["CaseSolution.IsAbout_PersonsName"].ToString().Trim() != string.Empty)
            {
                int IsAbout_PersonsName = Convert.ToInt32(collection["CaseSolution.IsAbout_PersonsName"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsName = IsAbout_PersonsName.ToString();
            }

            if (collection["CaseSolution.IsAbout_PersonsPhone"].ToString().Trim() != string.Empty)
            {
                int IsAbout_PersonsPhone = Convert.ToInt32(collection["CaseSolution.IsAbout_PersonsPhone"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsPhone = IsAbout_PersonsPhone.ToString();
            }

            if (collection["CaseSolution.IsAbout_Place"].ToString().Trim() != string.Empty)
            {
                int IsAbout_Place = Convert.ToInt32(collection["CaseSolution.IsAbout_Place"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_Place = IsAbout_Place.ToString();
            }

            if (collection["CaseSolution.IsAbout_Region_Id"].ToString().Trim() != string.Empty)
            {
                int IsAbout_Region_Id = Convert.ToInt32(collection["CaseSolution.IsAbout_Region_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_Region_Id = IsAbout_Region_Id;
            }

            if (collection["CaseSolution.IsAbout_ReportedBy"].ToString().Trim() != string.Empty)
            {
                int IsAbout_ReportedBy = Convert.ToInt32(collection["CaseSolution.IsAbout_ReportedBy"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_ReportedBy = IsAbout_ReportedBy.ToString();
            }
            if (collection["CaseSolution.IsAbout_UserCode"].ToString().Trim() != string.Empty)
            {
                int IsAbout_UserCode = Convert.ToInt32(collection["CaseSolution.IsAbout_UserCode"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_UserCode = IsAbout_UserCode.ToString();
            }


            if (collection["CaseSolution.Name"].ToString().Trim() != string.Empty)
            {
                string Name = Convert.ToString(collection["CaseSolution.Name"].ToString());
                caseSolutionInputViewModel.CaseSolution.Name = Name;
            }

            if (collection["CaseSolution.OU_Id"].ToString().Trim() != string.Empty)
            {
                int OU_Id = Convert.ToInt32(collection["CaseSolution.OU_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.OU_Id = OU_Id;
            }



            if (collection["CaseSolution.OtherCost"].ToString().Trim() != string.Empty)
            {
                int OtherCost = Convert.ToInt32(collection["CaseSolution.OtherCost"].ToString());
                caseSolutionInputViewModel.CaseSolution.OtherCost = OtherCost;
            }

            if (collection["CaseSolution.OverWritePopUp"].ToString().Trim() != string.Empty)
            {
                string OverWritePopUp = Convert.ToString(collection["CaseSolution.OverWritePopUp"].ToString());

                pos = OverWritePopUp.IndexOf(",");
                if (pos > 0)
                {
                    OverWritePopUp = OverWritePopUp.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.OverWritePopUp = Convert.ToInt32(OverWritePopUp.Substring(0, pos));

                }
            }

            if (collection["CaseSolution.PersonsCellPhone"].ToString().Trim() != string.Empty)
            {
                string PersonsCellPhone = Convert.ToString(collection["CaseSolution.PersonsCellPhone"].ToString());
                caseSolutionInputViewModel.CaseSolution.PersonsCellPhone = PersonsCellPhone;
            }

            if (collection["CaseSolution.PlanDate"].ToString().Trim() != string.Empty)
            {
                string PlanDate = Convert.ToString(collection["CaseSolution.PlanDate"].ToString());
                caseSolutionInputViewModel.CaseSolution.PlanDate = Convert.ToDateTime(PlanDate);
            }

            if (collection["CaseSolution.Priority_Id"].ToString().Trim() != string.Empty)
            {
                int Priority_Id = Convert.ToInt32(collection["CaseSolution.Priority_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Priority_Id = Priority_Id;
            }

            if (collection["CaseSolution.Priority_Id"].ToString().Trim() != string.Empty)
            {
                int Problem_Id = Convert.ToInt32(collection["CaseSolution.Problem_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Problem_Id = Problem_Id;
            }
            if (collection["CaseSolution.ProductArea_Id"].ToString().Trim() != string.Empty)
            {
                int ProductArea_Id = Convert.ToInt32(collection["CaseSolution.ProductArea_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.ProductArea_Id = ProductArea_Id;
            }

            if (collection["CaseSolution.Project_Id"].ToString().Trim() != string.Empty)
            {
                int Project_Id = Convert.ToInt32(collection["CaseSolution.Project_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Project_Id = Project_Id;
            }

            if (collection["CaseSolution.Region_Id"].ToString().Trim() != string.Empty)
            {
                int Region_Id = Convert.ToInt32(collection["CaseSolution.Region_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Region_Id = Region_Id;
            }

            if (collection["CaseSolution.RegistrationSource"].ToString().Trim() != string.Empty)
            {
                int RegistrationSource = Convert.ToInt32(collection["CaseSolution.RegistrationSource"].ToString());
                caseSolutionInputViewModel.CaseSolution.RegistrationSource = RegistrationSource;
            }

            if (collection["CaseSolution.SMS"].ToString().Trim() != string.Empty)
            {
                int SMS = Convert.ToInt32(collection["CaseSolution.SMS"].ToString());
                caseSolutionInputViewModel.CaseSolution.SMS = SMS;
            }

            if (collection["CaseSolution.SaveAndClose"].ToString().Trim() != string.Empty)
            {
                int SaveAndClose = Convert.ToInt32(collection["CaseSolution.SaveAndClose"].ToString());
                caseSolutionInputViewModel.CaseSolution.SaveAndClose = SaveAndClose;
            }


            if (collection["CaseSolution.ShortDescription"].ToString().Trim() != string.Empty)
            {
                string ShortDescription = Convert.ToString(collection["CaseSolution.ShortDescription"].ToString());
                caseSolutionInputViewModel.CaseSolution.ShortDescription = ShortDescription;
            }
            if (collection["CaseSolution.CaseSolutionDescription"].ToString().Trim() != string.Empty)
            {
                string CaseSolutionDescription = Convert.ToString(collection["CaseSolution.CaseSolutionDescription"].ToString());
                caseSolutionInputViewModel.CaseSolution.CaseSolutionDescription = CaseSolutionDescription;
            }

            if (collection["CaseSolution.ShowInSelfService"].ToString().Trim() != string.Empty)
            {

                if (collection["CaseSolution.ShowInSelfService"].ToString() == "0")
                {
                    caseSolutionInputViewModel.CaseSolution.ShowInSelfService = false;
                }
                else
                {
                    caseSolutionInputViewModel.CaseSolution.ShowInSelfService = true;
                }
            }

            if (collection["CaseSolution.ShowInsideCase"].ToString().Trim() != string.Empty)
            {
                string ShowInsideCase = Convert.ToString(collection["CaseSolution.ShowInsideCase"].ToString());

                pos = ShowInsideCase.IndexOf(",");
                if (pos > 0)
                {
                    ShowInsideCase = ShowInsideCase.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.OverWritePopUp = Convert.ToInt32(ShowInsideCase);

                }


            }

            if (collection["CaseSolution.ShowOnCaseOverview"].ToString().Trim() != string.Empty)
            {
                string ShowOnCaseOverview = Convert.ToString(collection["CaseSolution.ShowOnCaseOverview"].ToString());
                pos = ShowOnCaseOverview.IndexOf(",");
                if (pos > 0)
                {
                    ShowOnCaseOverview = ShowOnCaseOverview.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.ShowOnCaseOverview = Convert.ToInt32(ShowOnCaseOverview);

                }


            }

            if (collection["CaseSolution.SortOrder"].ToString().Trim() != string.Empty)
            {
                int SortOrder = Convert.ToInt32(collection["CaseSolution.SortOrder"].ToString());
                caseSolutionInputViewModel.CaseSolution.SortOrder = SortOrder;
            }
            if (collection["CaseSolution.StateSecondary_Id"].ToString().Trim() != string.Empty)
            {
                int StateSecondary_Id = Convert.ToInt32(collection["CaseSolution.StateSecondary_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.StateSecondary_Id = StateSecondary_Id;
            }
            if (collection["CaseSolution.Status_Id"].ToString().Trim() != string.Empty)
            {
                int Status_Id = Convert.ToInt32(collection["CaseSolution.Status_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Status_Id = Status_Id;
            }
            if (collection["CaseSolution.Supplier_Id"].ToString().Trim() != string.Empty)
            {
                int Supplier_Id = Convert.ToInt32(collection["CaseSolution.Supplier_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Supplier_Id = Supplier_Id;
            }
            if (collection["CaseSolution.System_Id"].ToString().Trim() != string.Empty)
            {
                int System_Id = Convert.ToInt32(collection["CaseSolution.System_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.System_Id = System_Id;
            }



            if (collection["CaseSolution.TemplatePath"].ToString().Trim() != string.Empty)
            {
                string TemplatePath = Convert.ToString(collection["CaseSolution.TemplatePath"].ToString());
                caseSolutionInputViewModel.CaseSolution.TemplatePath = TemplatePath;
            }

            if (collection["CaseSolution.UpdateNotifierInformation"].ToString().Trim() != string.Empty)
            {
                int UpdateNotifierInformation = Convert.ToInt32(collection["CaseSolution.UpdateNotifierInformation"].ToString());
                caseSolutionInputViewModel.CaseSolution.UpdateNotifierInformation = UpdateNotifierInformation;
            }
            if (collection["CaseSolution.Urgency_Id"].ToString().Trim() != string.Empty)
            {
                int Urgency_Id = Convert.ToInt32(collection["CaseSolution.Urgency_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Urgency_Id = Urgency_Id;
            }

            if (collection["CaseSolution.Verified"].ToString().Trim() != string.Empty)
            {
                int Verified = Convert.ToInt32(collection["CaseSolution.Verified"].ToString());
                caseSolutionInputViewModel.CaseSolution.Verified = Verified;
            }
            if (collection["CaseSolution.WatchDate"].ToString().Trim() != string.Empty)
            {
                string WatchDate = Convert.ToString(collection["CaseSolution.WatchDate"].ToString());
                caseSolutionInputViewModel.CaseSolution.WatchDate = Convert.ToDateTime(WatchDate);
            }
            if (collection["CaseSolution.WorkingGroup_Id"].ToString().Trim() != string.Empty)
            {
                int WorkingGroup_Id = Convert.ToInt32(collection["CaseSolution.WorkingGroup_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.WorkingGroup_Id = WorkingGroup_Id;
            }
            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            CaseSettingsSolutionAggregate settingsSolutionAggregate = this.CreateCaseSettingsSolutionAggregate(caseSolutionInputViewModel.CaseSolution.Id, caseSolutionSettingModels);
            this.caseSolutionSettingService.AddCaseSolutionSettings(settingsSolutionAggregate);


            int casesoilutionid = caseSolutionInputViewModel.CaseSolution.Id;

            string condid = selvals;
            string caid = casesoilutionid.ToString();
            this._caseSolutionConditionService.Add(Convert.ToInt32(caid), Convert.ToInt32(condid));


            //var caseSolution = this._caseSolutionService.GetCaseSolution(casesoilutionid);




            //Get not selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSetting = new List<CaseSolutionSettingsField>();
            lFieldSetting = _caseSolutionConditionService.GetCaseSolutionFieldSetting(Convert.ToInt32(caid));

            List<SelectListItem> feildSettings = null;
            feildSettings = lFieldSetting
                  .Select(x => new SelectListItem
                  {
                      Text = x.Text,
                      Value = x.CaseSolutionConditionId.ToString(),
                      Selected = false
                  }).ToList();
            //Get not selected case solution conditions

            //Get selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSettingSelected = new List<CaseSolutionSettingsField>();
            int Cust = Convert.ToInt32(collection["CaseSolution.Customer_Id"].ToString());
            lFieldSettingSelected = _caseSolutionConditionService.GetSelectedCaseSolutionFieldSetting(Convert.ToInt32(caid), Cust);


            var model = new CaseSolutionInputViewModel
            {
                CaseSolutionFieldSettings = feildSettings,
                CSSelectedSettingsField = lFieldSettingSelected.ToList()
            };

            TempData["NewOrOld"] = caid.ToString();
            return PartialView("/Views/CaseSolution/_Conditions.cshtml", model);






            //int? backToPageId = null; 
            //if (backToPageId == null)
            //    ViewBag.PageId = 0;
            //else
            //    ViewBag.PageId = backToPageId;


            //var caseSolution = this._caseSolutionService.GetCaseSolution(Convert.ToInt32(caid));


            //var model = this.CreateInputViewModel(caseSolution);

            //return this.View("/Views/CaseSolution/Edit.cshtml", model);


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

            //ViewBag.IdVal = caseSolution.Id;
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
                    SplitToCaseSolution_Id = caseSolution.SplitToCaseSolution_Id.HasValue ? caseSolution.SplitToCaseSolution_Id : null,

                    SMS = caseSolution.SMS.ToBool(),
                    AgreedDate = caseSolution.AgreedDate.HasValue ? caseSolution.AgreedDate.Value.ToShortDateString() : string.Empty,
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

        public PartialViewResult RemoveCondition(string condition, string casesolutionid)
        {
            string condid = condition.Replace("'", "");
            string caid = casesolutionid.Replace("'", "");
            this._caseSolutionConditionService.Remove(Convert.ToString(condid), Convert.ToInt32(caid));

            //Get not selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSetting = new List<CaseSolutionSettingsField>();
            lFieldSetting = _caseSolutionConditionService.GetCaseSolutionFieldSetting(Convert.ToInt32(caid));

            List<SelectListItem> feildSettings = null;
            feildSettings = lFieldSetting
                  .Select(x => new SelectListItem
                  {
                      Text = x.Text,
                      Value = x.CaseSolutionConditionId.ToString(),
                      Selected = false
                  }).ToList();
            //Get not selected case solution conditions

            //Get selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSettingSelected = new List<CaseSolutionSettingsField>();
            int cust = SessionFacade.CurrentCustomer.Id;
            lFieldSettingSelected = _caseSolutionConditionService.GetSelectedCaseSolutionFieldSetting(Convert.ToInt32(caid), cust);

            //Get selected case solution conditions

            var model = new CaseSolutionInputViewModel
            {
                CaseSolutionFieldSettings = feildSettings,
                CSSelectedSettingsField = lFieldSettingSelected.ToList()
            };

            return PartialView("_Conditions", model);
        }

        [ValidateInput(false)]
        public ActionResult AddCondition(string conditionid, string casesolutionid)
        {
            string condid = conditionid.Replace("'", "");
            string caid = casesolutionid.Replace("'", "");

            this._caseSolutionConditionService.Add(Convert.ToInt32(caid), Convert.ToInt32(condid));


            //Get not selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSetting = new List<CaseSolutionSettingsField>();
            lFieldSetting = _caseSolutionConditionService.GetCaseSolutionFieldSetting(Convert.ToInt32(caid));

            List<SelectListItem> feildSettings = null;
            feildSettings = lFieldSetting
                  .Select(x => new SelectListItem
                  {
                      Text = x.Text,
                      Value = x.CaseSolutionConditionId.ToString(),
                      Selected = false
                  }).ToList();
            //Get not selected case solution conditions

            //Get selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSettingSelected = new List<CaseSolutionSettingsField>();
            int cust = SessionFacade.CurrentCustomer.Id;
            lFieldSettingSelected = _caseSolutionConditionService.GetSelectedCaseSolutionFieldSetting(Convert.ToInt32(caid), cust);

            //Get selected case solution conditions

            var model = new CaseSolutionInputViewModel
            {
                CaseSolutionFieldSettings = feildSettings,
                CSSelectedSettingsField = lFieldSettingSelected.ToList()
            };

            return PartialView("/Views/CaseSolution/_Conditions.cshtml", model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(
            CaseSolutionInputViewModel caseSolutionInputViewModel,
            CaseSolutionSettingModel[] CaseSolutionSettingModels,
            int PageId, string selectedValues)
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

            if (caseSolutionInputViewModel.SplitToCaseSolutionIds != null)
            {
                   caseSolutionInputViewModel.CaseSolution.SplitToCaseSolutionDescendants = _caseSolutionService.GetSplitToCaseSolutionDescendants(caseSolutionInputViewModel.CaseSolution, caseSolutionInputViewModel.SplitToCaseSolutionIds);
            }

            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            CaseSettingsSolutionAggregate settingsSolutionAggregate =
                this.CreateCaseSettingsSolutionAggregate(
                    caseSolutionInputViewModel.CaseSolution.Id,
                    CaseSolutionSettingModels);
            this.caseSolutionSettingService.UpdateCaseSolutionSettings(settingsSolutionAggregate);

            List<CaseSolitionConditionListEntity> cse = new List<CaseSolitionConditionListEntity>();

            //Remove caching of all casesolution conditions
            this._cache.InvalidateStartsWith(DH.Helpdesk.Common.Constants.CacheKey.CaseSolutionCondition);

            string[] selectedSplit = selectedValues.Split('~');
            foreach (string s in selectedSplit)
            {
                string[] cap = s.Split(':');
                string text = string.Empty;
                if (cap[0].ToString().Substring (0,1)=="_")
                {
                    int len = cap[0].ToString().Length;
                    text = cap[0].ToString().Substring(1, (len-1));
                }
                else
                {
                    text = cap[0].ToString();
                }
                
                string values = string.Empty;
                if (cap.Count() > 1)
                {
                    values = cap[1].ToString();
                }
                bool exists = false;

                exists = cse.Any(u => u.CaseSolutionConditionCaption == text);
                if (exists == false)
                {
                    CaseSolitionConditionListEntity cSc = new CaseSolitionConditionListEntity
                    {
                        CaseSolutionConditionCaption = text,
                        CaseSolutionConditionValues = values.Replace('_', ',')
                    };

                    cse.Add(cSc);
                }
                else
                {



                    string exvalues = cse.Where(x => x.CaseSolutionConditionCaption == text).FirstOrDefault().CaseSolutionConditionValues;
                    string exid = cse.Where(x => x.CaseSolutionConditionCaption == text).FirstOrDefault().CaseSolutionConditionCaption;

                    string[] existarray = exvalues.Split(',');
                    string[] newarray = values.Split(',');
                    string[] result = existarray.Union(newarray).ToArray();
                    string final = string.Empty;
                    foreach (string word in result)
                    {
                        if (final == string.Empty)
                        {
                            final = word;
                        }
                        else
                        {
                            final = final + "," + word;
                        }
                    }



                    foreach (var item in cse.Where(w => w.CaseSolutionConditionCaption == text))
                    {
                        item.CaseSolutionConditionValues = final;
                    }



                }


            }
            if (cse != null)
            {
                foreach (CaseSolitionConditionListEntity lk in cse)
                {
                    CaseSolutionConditionEntity o = new CaseSolutionConditionEntity
                    {
                        CaseSolution_Id = caseSolutionInputViewModel.CaseSolution.Id,
                        Property_Name = lk.CaseSolutionConditionCaption,
                        Values = lk.CaseSolutionConditionValues
                    };
                    this._caseSolutionConditionService.Save(o);
                }
            }





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

        //[HttpPost]
        [HttpPost, ValidateInput(false)]
        //public RedirectToRouteResult Copy(FormCollection collection, string selectedValues)
        public int Copy(FormCollection collection, string selectedValues)

        {

            IDictionary<string, string> errors = new Dictionary<string, string>();
            IList<CaseFieldSetting> CheckMandatory = null;//_caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id);

            CaseSolutionSettingModel[] caseSolutionSettingModels = new CaseSolutionSettingModel[0];

            string cost = Convert.ToString(collection["CaseSolution.Cost"].ToString());
            string selvals = string.Empty;// Convert.ToString(collection["lstAddFieldSettings"].ToString());


            string caseSolutionId = Convert.ToString(collection["CaseSolution.Id"].ToString());
            string scheduleMonthly = Convert.ToString(collection["ScheduleMonthly"].ToString());
            int scheduleType = 0;// Convert.ToString(collection["ScheduleType"].ToString());
            int scheduleTime = 0; // Convert.ToString(collection["ScheduleTime"].ToString()); 
            int scheduleWatchDate = 0;// Convert.ToString(collection["ScheduleWatchDate"].ToString());
            string[] scheduleDay = null;// = Convert.ToString(collection["ScheduleDay"].ToString());
            string[] scheduleMonth = null;// = Convert.ToString(collection["ScheduleMonth"].ToString());
            string scheduleMonthlyDay = Convert.ToString(collection["ScheduleMonthlyDay"].ToString());
            int scheduleMonthlyOrder = 0;// Convert.ToString(collection["ScheduleMonthlyOrder"].ToString());
            int scheduleMonthlyWeekday = 0;// Convert.ToString(collection["ScheduleMonthlyWeekday"].ToString());
            string Schedule = Convert.ToString(collection["Schedule"].ToString());

            var caseSolutionSchedule = new CaseSolutionSchedule();

            string Sc = string.Empty;
            if (Schedule.ToString().Trim() != string.Empty)
            {
                Sc = Convert.ToString(Schedule.ToString());

                int pos1 = Sc.IndexOf(",");
                if (pos1 > 0)
                {
                    Sc = Sc.Substring(0, pos1);

                    //caseSolutionInputViewModel.CaseSolution.ShowInsideCase = Convert.ToInt32(ShowInsideCase.Substring(0, pos));

                }
            }

            if (Convert.ToInt32(Sc) != 0)
            {
                caseSolutionSchedule.CaseSolution_Id = Convert.ToInt32(0);
                caseSolutionSchedule.ScheduleType = Convert.ToInt32(scheduleType);
                caseSolutionSchedule.ScheduleTime = Convert.ToInt32(scheduleTime);
                caseSolutionSchedule.ScheduleWatchDate = Convert.ToInt32(scheduleWatchDate);

                if (Convert.ToInt32(scheduleType) == 1)
                    caseSolutionSchedule.ScheduleDay = null;

                if (Convert.ToInt32(scheduleType) == 2 && scheduleDay != null)
                {
                    caseSolutionSchedule.ScheduleDay = "," + string.Join(",", scheduleDay) + ",";
                }

                if (Convert.ToInt32(scheduleType) == 3)
                {
                    if (Convert.ToInt32(scheduleMonthly) == 1 && scheduleMonth != null)
                    {
                        caseSolutionSchedule.ScheduleDay = scheduleMonthlyDay + ";," + string.Join(",", scheduleMonth) + ",";
                    }

                    if (Convert.ToInt32(scheduleMonthly) == 2 && scheduleMonth != null)
                    {
                        caseSolutionSchedule.ScheduleDay = scheduleMonthlyOrder + ":" + scheduleMonthlyWeekday + ";," + string.Join(",", scheduleMonth) + ",";
                    }
                }
            }
            else
            {
                caseSolutionSchedule = null;
            }

            CaseSolutionInputViewModel caseSolutionInputViewModel = new CaseSolutionInputViewModel();
            caseSolutionInputViewModel.CaseSolution = new CaseSolution();
            string NoMailToNotifier = Convert.ToString(collection["Casesolution.NoMailToNotifier"].ToString());
            NoMailToNotifier = NoMailToNotifier.Replace(",", ".");
            int pos = NoMailToNotifier.IndexOf(".");
            if (pos > 0)
            {
                NoMailToNotifier = NoMailToNotifier.Substring(0, pos);
            }
            if (NoMailToNotifier == string.Empty)
            {
                NoMailToNotifier = "0";
            }
            // Positive: Send Mail to...

            if (Convert.ToInt32(NoMailToNotifier) == 0)
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 1;
            else
                caseSolutionInputViewModel.CaseSolution.NoMailToNotifier = 0;


            caseSolutionInputViewModel.CaseSolution.Id = 0;

            string PerformerUser_Id = Convert.ToString(collection["Casesolution.PerformerUser_Id"].ToString());
            PerformerUser_Id = PerformerUser_Id.Replace(",", ".");
            if (PerformerUser_Id == string.Empty)
            {
                PerformerUser_Id = "0";
            }
            if (Convert.ToInt32(PerformerUser_Id) == -1)
            {
                caseSolutionInputViewModel.CaseSolution.PerformerUser_Id = null;
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = 1;
            }
            else
            {
                caseSolutionInputViewModel.CaseSolution.SetCurrentUserAsPerformer = null;
            }

            string CaseWorkingGroup_Id = Convert.ToString(collection["Casesolution.CaseWorkingGroup_Id"].ToString());
            CaseWorkingGroup_Id = CaseWorkingGroup_Id.Replace(",", ".");
            if (CaseWorkingGroup_Id == string.Empty)
            {
                CaseWorkingGroup_Id = "0";
            }
            if (Convert.ToInt32(CaseWorkingGroup_Id) == -1)
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



            /////SAve fields


            if (collection["CaseSolution.Status"].ToString().Trim() != string.Empty)
            {
                string status = Convert.ToString(collection["CaseSolution.Status"].ToString());

                pos = status.IndexOf(",");
                if (pos > 0)
                {
                    status = status.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.Status = Convert.ToInt32(status.Substring(0, pos));

                }
            }



            if (collection["CaseSolution.ShowInsideCase"].ToString().Trim() != string.Empty)
            {
                string ShowInsideCase = Convert.ToString(collection["CaseSolution.ShowInsideCase"].ToString());

                pos = ShowInsideCase.IndexOf(",");
                if (pos > 0)
                {
                    ShowInsideCase = ShowInsideCase.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.ShowInsideCase = Convert.ToInt32(ShowInsideCase.Substring(0, pos));

                }
            }



            string Caption = Convert.ToString(collection["Casesolution.Caption"].ToString());
            caseSolutionInputViewModel.CaseSolution.Caption = Caption ?? string.Empty;

            string Description = Convert.ToString(collection["Casesolution.Description"].ToString());
            caseSolutionInputViewModel.CaseSolution.Description = Description ?? string.Empty;

            string Miscellaneous = Convert.ToString(collection["Casesolution.Miscellaneous"].ToString());
            caseSolutionInputViewModel.CaseSolution.Miscellaneous = Miscellaneous ?? string.Empty;

            string ReportedBy = Convert.ToString(collection["Casesolution.ReportedBy"].ToString());
            caseSolutionInputViewModel.CaseSolution.ReportedBy = ReportedBy ?? string.Empty;

            string Text_External = Convert.ToString(collection["Casesolution.Text_External"].ToString());
            caseSolutionInputViewModel.CaseSolution.Text_External = Text_External ?? string.Empty;

            string Text_Internal = Convert.ToString(collection["Casesolution.Text_Internal"].ToString());
            caseSolutionInputViewModel.CaseSolution.Text_Internal = Text_Internal ?? string.Empty;

            string PersonsName = Convert.ToString(collection["Casesolution.PersonsName"].ToString());
            caseSolutionInputViewModel.CaseSolution.PersonsName = PersonsName ?? string.Empty;

            string PersonsPhone = Convert.ToString(collection["Casesolution.PersonsPhone"].ToString());
            caseSolutionInputViewModel.CaseSolution.PersonsPhone = PersonsPhone ?? string.Empty;

            string PersonsEmail = Convert.ToString(collection["Casesolution.PersonsEmail"].ToString());
            caseSolutionInputViewModel.CaseSolution.PersonsEmail = PersonsEmail ?? string.Empty;

            string Place = Convert.ToString(collection["Casesolution.Place"].ToString());
            caseSolutionInputViewModel.CaseSolution.Place = Place ?? string.Empty;

            string UserCode = Convert.ToString(collection["Casesolution.UserCode"].ToString());
            caseSolutionInputViewModel.CaseSolution.UserCode = UserCode ?? string.Empty;

            string InvoiceNumber = Convert.ToString(collection["Casesolution.InvoiceNumber"].ToString());
            caseSolutionInputViewModel.CaseSolution.InvoiceNumber = InvoiceNumber ?? string.Empty;

            string ReferenceNumber = Convert.ToString(collection["Casesolution.ReferenceNumber"].ToString());
            caseSolutionInputViewModel.CaseSolution.ReferenceNumber = ReferenceNumber ?? string.Empty;

            string VerifiedDescription = Convert.ToString(collection["Casesolution.VerifiedDescription"].ToString());
            caseSolutionInputViewModel.CaseSolution.VerifiedDescription = VerifiedDescription ?? string.Empty;

            string SolutionRate = Convert.ToString(collection["Casesolution.SolutionRate"].ToString());
            caseSolutionInputViewModel.CaseSolution.SolutionRate = SolutionRate ?? string.Empty;

            string InventoryNumber = Convert.ToString(collection["Casesolution.InventoryNumber"].ToString());
            caseSolutionInputViewModel.CaseSolution.InventoryNumber = InventoryNumber ?? string.Empty;

            string InventoryType = Convert.ToString(collection["Casesolution.InventoryType"].ToString());
            caseSolutionInputViewModel.CaseSolution.InventoryType = InventoryType ?? string.Empty;

            string InventoryLocation = Convert.ToString(collection["Casesolution.InventoryLocation"].ToString());
            caseSolutionInputViewModel.CaseSolution.InventoryLocation = InventoryLocation ?? string.Empty;

            string Available = Convert.ToString(collection["Casesolution.Available"].ToString());
            caseSolutionInputViewModel.CaseSolution.Available = Available ?? string.Empty;

            string Currency = Convert.ToString(collection["Casesolution.Currency"].ToString());
            caseSolutionInputViewModel.CaseSolution.Currency = Currency ?? string.Empty;

            if (caseSolutionInputViewModel.CaseSolution.Text_External != null && caseSolutionInputViewModel.CaseSolution.Text_External.Length > 3000)
                caseSolutionInputViewModel.CaseSolution.Text_External = caseSolutionInputViewModel.CaseSolution.Text_External.Substring(0, 3000);

            if (caseSolutionInputViewModel.CaseSolution.Text_Internal != null && caseSolutionInputViewModel.CaseSolution.Text_Internal.Length > 3000)
                caseSolutionInputViewModel.CaseSolution.Text_Internal = caseSolutionInputViewModel.CaseSolution.Text_Internal.Substring(0, 3000);



            if (collection["CaseSolution.AgreedDate"].ToString().Trim() != string.Empty)
            {
                string AgreedDate = Convert.ToString(collection["CaseSolution.AgreedDate"].ToString());

                caseSolutionInputViewModel.CaseSolution.AgreedDate = Convert.ToDateTime(AgreedDate);
            }

            if (collection["CaseSolution.CaseSolutionCategory_Id"].ToString().Trim() != string.Empty)
            {
                int CaseSolutionCategory_Id = Convert.ToInt32(collection["CaseSolution.CaseSolutionCategory_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.CaseSolutionCategory_Id = CaseSolutionCategory_Id;
            }

            if (collection["CaseSolution.CaseType_Id"].ToString().Trim() != string.Empty)
            {
                int CaseType_Id = Convert.ToInt32(collection["CaseSolution.CaseType_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.CaseType_Id = CaseType_Id;
            }

            if (collection["CaseSolution.Category_Id"].ToString().Trim() != string.Empty)
            {
                int Category_Id = Convert.ToInt32(collection["CaseSolution.Category_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Category_Id = Category_Id;
            }

            if (collection["CaseSolution.CausingPartId"].ToString().Trim() != string.Empty)
            {
                int CausingPartId = Convert.ToInt32(collection["CaseSolution.CausingPartId"].ToString());
                caseSolutionInputViewModel.CaseSolution.CausingPartId = CausingPartId;
            }

            if (collection["CaseSolution.Change_Id"].ToString().Trim() != string.Empty)
            {
                int Change_Id = Convert.ToInt32(collection["CaseSolution.Change_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Change_Id = Change_Id;
            }

            caseSolutionInputViewModel.CaseSolution.ChangedDate = DateTime.Now;

            if (collection["CaseSolution.ConnectedButton"] != null)
            {
                if (collection["CaseSolution.ConnectedButton"].ToString().Trim() != string.Empty)
                {
                    int ConnectedButton = Convert.ToInt32(collection["CaseSolution.ConnectedButton"].ToString());
                    caseSolutionInputViewModel.CaseSolution.ConnectedButton = ConnectedButton;
                }
            }


            if (collection["CaseSolution.ContactBeforeAction"].ToString().Trim() != string.Empty)
            {
                string ContactBeforeAction = Convert.ToString(collection["CaseSolution.ContactBeforeAction"].ToString());

                pos = ContactBeforeAction.IndexOf(",");
                if (pos > 0)
                {
                    ContactBeforeAction = ContactBeforeAction.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.ContactBeforeAction = Convert.ToInt32(ContactBeforeAction.Substring(0, pos));

                }
            }



            if (collection["CaseSolution.Cost"].ToString().Trim() != string.Empty)
            {
                int Cost = Convert.ToInt32(collection["CaseSolution.Cost"].ToString());
                caseSolutionInputViewModel.CaseSolution.Cost = Cost;
            }

            if (collection["CaseSolution.CostCentre"].ToString().Trim() != string.Empty)
            {
                string CostCentre = Convert.ToString(collection["CaseSolution.CostCentre"].ToString());
                caseSolutionInputViewModel.CaseSolution.CostCentre = CostCentre;
            }



            caseSolutionInputViewModel.CaseSolution.CreatedDate = DateTime.Now;


            if (collection["CaseSolution.Customer_Id"].ToString().Trim() != string.Empty)
            {
                int Customer_Id = Convert.ToInt32(collection["CaseSolution.Customer_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Customer_Id = Customer_Id;
            }

            if (collection["CaseSolution.Department_Id"].ToString().Trim() != string.Empty)
            {
                int Department_Id = Convert.ToInt32(collection["CaseSolution.Department_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Department_Id = Department_Id;
            }

            if (collection["CaseSolution.FinishingCause_Id"].ToString().Trim() != string.Empty)
            {
                int FinishingCause_Id = Convert.ToInt32(collection["CaseSolution.FinishingCause_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.FinishingCause_Id = FinishingCause_Id;
            }

            if (collection["CaseSolution.FinishingDate"].ToString().Trim() != string.Empty)
            {
                string FinishingDate = Convert.ToString(collection["CaseSolution.FinishingDate"].ToString());
                caseSolutionInputViewModel.CaseSolution.FinishingDate = Convert.ToDateTime(FinishingDate);
            }

            if (collection["CaseSolution.FinishingDescription"].ToString().Trim() != string.Empty)
            {
                string FinishingDescription = Convert.ToString(collection["CaseSolution.FinishingDescription"].ToString());
                caseSolutionInputViewModel.CaseSolution.FinishingDescription = FinishingDescription;
            }





            if (collection["CaseSolution.Impact_Id"].ToString().Trim() != string.Empty)
            {
                int Impact_Id = Convert.ToInt32(collection["CaseSolution.Impact_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Impact_Id = Impact_Id;
            }

            if (collection["CaseSolution.Information"].ToString().Trim() != string.Empty)
            {
                string Information = Convert.ToString(collection["CaseSolution.Information"].ToString());
                caseSolutionInputViewModel.CaseSolution.Information = Information;
            }

            if (collection["CaseSolution.IsAbout_CostCentre"].ToString().Trim() != string.Empty)
            {
                string IsAbout_CostCentre = Convert.ToString(collection["CaseSolution.IsAbout_CostCentre"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_CostCentre = IsAbout_CostCentre;
            }

            if (collection["CaseSolution.IsAbout_Department_Id"].ToString().Trim() != string.Empty)
            {
                int IsAbout_Department_Id = Convert.ToInt32(collection["CaseSolution.IsAbout_Department_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_Department_Id = IsAbout_Department_Id;
            }

            if (collection["CaseSolution.IsAbout_OU_Id"].ToString().Trim() != string.Empty)
            {
                int IsAbout_OU_Id = Convert.ToInt32(collection["CaseSolution.IsAbout_OU_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_OU_Id = IsAbout_OU_Id;
            }

            if (collection["CaseSolution.IsAbout_PersonsCellPhone"].ToString().Trim() != string.Empty)
            {
                int IsAbout_PersonsCellPhone = Convert.ToInt32(collection["CaseSolution.IsAbout_PersonsCellPhone"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsCellPhone = IsAbout_PersonsCellPhone.ToString();
            }

            if (collection["CaseSolution.IsAbout_PersonsEmail"].ToString().Trim() != string.Empty)
            {
                string IsAbout_PersonsEmail = Convert.ToString(collection["CaseSolution.IsAbout_PersonsEmail"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsEmail = IsAbout_PersonsEmail.ToString();
            }
            if (collection["CaseSolution.IsAbout_PersonsName"].ToString().Trim() != string.Empty)
            {
                string IsAbout_PersonsName = Convert.ToString(collection["CaseSolution.IsAbout_PersonsName"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsName = IsAbout_PersonsName.ToString();
            }

            if (collection["CaseSolution.IsAbout_PersonsPhone"].ToString().Trim() != string.Empty)
            {
                string IsAbout_PersonsPhone = Convert.ToString(collection["CaseSolution.IsAbout_PersonsPhone"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_PersonsPhone = IsAbout_PersonsPhone.ToString();
            }

            if (collection["CaseSolution.IsAbout_Place"].ToString().Trim() != string.Empty)
            {
                string IsAbout_Place = Convert.ToString(collection["CaseSolution.IsAbout_Place"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_Place = IsAbout_Place.ToString();
            }

            if (collection["CaseSolution.IsAbout_Region_Id"].ToString().Trim() != string.Empty)
            {
                int IsAbout_Region_Id = Convert.ToInt32(collection["CaseSolution.IsAbout_Region_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_Region_Id = IsAbout_Region_Id;
            }

            if (collection["CaseSolution.IsAbout_ReportedBy"].ToString().Trim() != string.Empty)
            {
                string IsAbout_ReportedBy = Convert.ToString(collection["CaseSolution.IsAbout_ReportedBy"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_ReportedBy = IsAbout_ReportedBy.ToString();
            }
            if (collection["CaseSolution.IsAbout_UserCode"].ToString().Trim() != string.Empty)
            {
                string IsAbout_UserCode = Convert.ToString(collection["CaseSolution.IsAbout_UserCode"].ToString());
                caseSolutionInputViewModel.CaseSolution.IsAbout_UserCode = IsAbout_UserCode.ToString();
            }


            if (collection["CaseSolution.Name"].ToString().Trim() != string.Empty)
            {
                string Name = Convert.ToString(collection["CaseSolution.Name"].ToString());
                caseSolutionInputViewModel.CaseSolution.Name = Name;
            }

            if (collection["CaseSolution.OU_Id"].ToString().Trim() != string.Empty)
            {
                int OU_Id = Convert.ToInt32(collection["CaseSolution.OU_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.OU_Id = OU_Id;
            }



            if (collection["CaseSolution.OtherCost"].ToString().Trim() != string.Empty)
            {
                int OtherCost = Convert.ToInt32(collection["CaseSolution.OtherCost"].ToString());
                caseSolutionInputViewModel.CaseSolution.OtherCost = OtherCost;
            }

            if (collection["CaseSolution.OverWritePopUp"].ToString().Trim() != string.Empty)
            {
                string OverWritePopUp = Convert.ToString(collection["CaseSolution.OverWritePopUp"].ToString());

                pos = OverWritePopUp.IndexOf(",");
                if (pos > 0)
                {
                    OverWritePopUp = OverWritePopUp.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.OverWritePopUp = Convert.ToInt32(OverWritePopUp.Substring(0, pos));

                }
            }

            if (collection["CaseSolution.PersonsCellPhone"].ToString().Trim() != string.Empty)
            {
                string PersonsCellPhone = Convert.ToString(collection["CaseSolution.PersonsCellPhone"].ToString());
                caseSolutionInputViewModel.CaseSolution.PersonsCellPhone = PersonsCellPhone;
            }

            if (collection["CaseSolution.PlanDate"].ToString().Trim() != string.Empty)
            {
                string PlanDate = Convert.ToString(collection["CaseSolution.PlanDate"].ToString());
                caseSolutionInputViewModel.CaseSolution.PlanDate = Convert.ToDateTime(PlanDate);
            }

            if (collection["CaseSolution.Priority_Id"].ToString().Trim() != string.Empty)
            {
                int Priority_Id = Convert.ToInt32(collection["CaseSolution.Priority_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Priority_Id = Priority_Id;
            }

            if (collection["CaseSolution.Problem_Id"].ToString().Trim() != string.Empty)
            {
                int Problem_Id = Convert.ToInt32(collection["CaseSolution.Problem_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Problem_Id = Problem_Id;
            }
            if (collection["CaseSolution.ProductArea_Id"].ToString().Trim() != string.Empty)
            {
                int ProductArea_Id = Convert.ToInt32(collection["CaseSolution.ProductArea_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.ProductArea_Id = ProductArea_Id;
            }

            if (collection["CaseSolution.Project_Id"].ToString().Trim() != string.Empty)
            {
                int Project_Id = Convert.ToInt32(collection["CaseSolution.Project_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Project_Id = Project_Id;
            }

            if (collection["CaseSolution.Region_Id"].ToString().Trim() != string.Empty)
            {
                int Region_Id = Convert.ToInt32(collection["CaseSolution.Region_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Region_Id = Region_Id;
            }

            if (collection["CaseSolution.RegistrationSource"].ToString().Trim() != string.Empty)
            {
                int RegistrationSource = Convert.ToInt32(collection["CaseSolution.RegistrationSource"].ToString());
                caseSolutionInputViewModel.CaseSolution.RegistrationSource = RegistrationSource;
            }

            //if (collection["CaseSolution.SMS"].ToString().Trim() != string.Empty)
            //{
            //    int SMS = Convert.ToInt32(collection["CaseSolution.SMS"].ToString());
            //    caseSolutionInputViewModel.CaseSolution.SMS = SMS;
            //}

            if (collection["CaseSolution.SMS"].ToString().Trim() != string.Empty)
            {
                string SMS = Convert.ToString(collection["CaseSolution.SMS"].ToString());

                pos = SMS.IndexOf(",");
                if (pos > 0)
                {
                    SMS = SMS.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.SMS = Convert.ToInt32(SMS.Substring(0, pos));

                }
            }



            if (collection["CaseSolution.SaveAndClose"] != null)
            {
                if (collection["CaseSolution.SaveAndClose"].ToString().Trim() != string.Empty)
                {
                    int SaveAndClose = Convert.ToInt32(collection["CaseSolution.SaveAndClose"].ToString());
                    caseSolutionInputViewModel.CaseSolution.SaveAndClose = SaveAndClose;
                }
            }


            if (collection["CaseSolution.ShortDescription"].ToString().Trim() != string.Empty)
            {
                string ShortDescription = Convert.ToString(collection["CaseSolution.ShortDescription"].ToString());
                caseSolutionInputViewModel.CaseSolution.ShortDescription = ShortDescription;
            }
            if (collection["CaseSolution.CaseSolutionDescription"].ToString().Trim() != string.Empty)
            {
                string CaseSolutionDescription = Convert.ToString(collection["CaseSolution.CaseSolutionDescription"].ToString());
                caseSolutionInputViewModel.CaseSolution.CaseSolutionDescription = CaseSolutionDescription;
            }

            if (collection["CaseSolution.ShowInSelfService"].ToString().Trim() != string.Empty)
            {

                if (collection["CaseSolution.ShowInSelfService"].ToString() == "0")
                {
                    caseSolutionInputViewModel.CaseSolution.ShowInSelfService = false;
                }
                else
                {
                    caseSolutionInputViewModel.CaseSolution.ShowInSelfService = true;
                }
            }

            if (collection["CaseSolution.ShowInsideCase"].ToString().Trim() != string.Empty)
            {
                string ShowInsideCase = Convert.ToString(collection["CaseSolution.ShowInsideCase"].ToString());

                pos = ShowInsideCase.IndexOf(",");
                if (pos > 0)
                {
                    ShowInsideCase = ShowInsideCase.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.OverWritePopUp = Convert.ToInt32(ShowInsideCase);

                }


            }

            if (collection["CaseSolution.ShowOnCaseOverview"].ToString().Trim() != string.Empty)
            {
                string ShowOnCaseOverview = Convert.ToString(collection["CaseSolution.ShowOnCaseOverview"].ToString());
                pos = ShowOnCaseOverview.IndexOf(",");
                if (pos > 0)
                {
                    ShowOnCaseOverview = ShowOnCaseOverview.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.ShowOnCaseOverview = Convert.ToInt32(ShowOnCaseOverview);

                }


            }

            if (collection["CaseSolution.SortOrder"].ToString().Trim() != string.Empty)
            {
                int SortOrder = Convert.ToInt32(collection["CaseSolution.SortOrder"].ToString());
                caseSolutionInputViewModel.CaseSolution.SortOrder = SortOrder;
            }
            if (collection["CaseSolution.StateSecondary_Id"].ToString().Trim() != string.Empty)
            {
                int StateSecondary_Id = Convert.ToInt32(collection["CaseSolution.StateSecondary_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.StateSecondary_Id = StateSecondary_Id;
            }
            if (collection["CaseSolution.Status_Id"].ToString().Trim() != string.Empty)
            {
                int Status_Id = Convert.ToInt32(collection["CaseSolution.Status_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Status_Id = Status_Id;
            }
            if (collection["CaseSolution.Supplier_Id"].ToString().Trim() != string.Empty)
            {
                int Supplier_Id = Convert.ToInt32(collection["CaseSolution.Supplier_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Supplier_Id = Supplier_Id;
            }
            if (collection["CaseSolution.System_Id"].ToString().Trim() != string.Empty)
            {
                int System_Id = Convert.ToInt32(collection["CaseSolution.System_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.System_Id = System_Id;
            }



            if (collection["CaseSolution.TemplatePath"].ToString().Trim() != string.Empty)
            {
                string TemplatePath = Convert.ToString(collection["CaseSolution.TemplatePath"].ToString());
                caseSolutionInputViewModel.CaseSolution.TemplatePath = TemplatePath;
            }

            if (collection["CaseSolution.UpdateNotifierInformation"].ToString().Trim() != string.Empty)
            {
                int UpdateNotifierInformation = Convert.ToInt32(collection["CaseSolution.UpdateNotifierInformation"].ToString());
                caseSolutionInputViewModel.CaseSolution.UpdateNotifierInformation = UpdateNotifierInformation;
            }
            if (collection["CaseSolution.Urgency_Id"].ToString().Trim() != string.Empty)
            {
                int Urgency_Id = Convert.ToInt32(collection["CaseSolution.Urgency_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.Urgency_Id = Urgency_Id;
            }


            if (collection["CaseSolution.Verified"].ToString().Trim() != string.Empty)
            {
                string Verified = Convert.ToString(collection["CaseSolution.Verified"].ToString());
                pos = Verified.IndexOf(",");
                if (pos > 0)
                {
                    Verified = Verified.Substring(0, pos);

                    caseSolutionInputViewModel.CaseSolution.Verified = Convert.ToInt32(Verified);

                }


            }







            if (collection["CaseSolution.WatchDate"].ToString().Trim() != string.Empty)
            {
                string WatchDate = Convert.ToString(collection["CaseSolution.WatchDate"].ToString());
                caseSolutionInputViewModel.CaseSolution.WatchDate = Convert.ToDateTime(WatchDate);
            }
            if (collection["CaseSolution.WorkingGroup_Id"].ToString().Trim() != string.Empty)
            {
                int WorkingGroup_Id = Convert.ToInt32(collection["CaseSolution.WorkingGroup_Id"].ToString());
                caseSolutionInputViewModel.CaseSolution.WorkingGroup_Id = WorkingGroup_Id;
            }


            caseSolutionInputViewModel.DefaultTab = "case-tab";
            caseSolutionInputViewModel.CaseSolution.DefaultTab = "case-tab";
            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            CaseSettingsSolutionAggregate settingsSolutionAggregate = this.CreateCaseSettingsSolutionAggregate(caseSolutionInputViewModel.CaseSolution.Id, caseSolutionSettingModels);
            this.caseSolutionSettingService.AddCaseSolutionSettings(settingsSolutionAggregate);


            int casesoilutionid = caseSolutionInputViewModel.CaseSolution.Id;

            List<CaseSolitionConditionListEntity> cse = new List<CaseSolitionConditionListEntity>();


            string[] selectedSplit = selectedValues.Split('~');
            foreach (string s in selectedSplit)
            {
                string[] cap = s.Split(':');
                string text = cap[0].ToString();
                string values = string.Empty;
                if (cap.Count() > 1)
                {
                    values = cap[1].ToString();
                }
                bool exists = false;

                exists = cse.Any(u => u.CaseSolutionConditionCaption == text);
                if (exists == false)
                {
                    CaseSolitionConditionListEntity cSc = new CaseSolitionConditionListEntity
                    {
                        CaseSolutionConditionCaption = text,
                        CaseSolutionConditionValues = values.Replace('_', ',')
                    };

                    cse.Add(cSc);
                }
                else
                {



                    string exvalues = cse.Where(x => x.CaseSolutionConditionCaption == text).FirstOrDefault().CaseSolutionConditionValues;
                    string exid = cse.Where(x => x.CaseSolutionConditionCaption == text).FirstOrDefault().CaseSolutionConditionCaption;

                    string[] existarray = exvalues.Split(',');
                    string[] newarray = values.Split(',');
                    string[] result = existarray.Union(newarray).ToArray();
                    string final = string.Empty;
                    foreach (string word in result)
                    {
                        if (final == string.Empty)
                        {
                            final = word;
                        }
                        else
                        {
                            final = final + "," + word;
                        }
                    }



                    foreach (var item in cse.Where(w => w.CaseSolutionConditionCaption == text))
                    {
                        item.CaseSolutionConditionValues = final;
                    }



                }


            }
            if (cse != null)
            {
                foreach (CaseSolitionConditionListEntity lk in cse)
                {
                    CaseSolutionConditionEntity o = new CaseSolutionConditionEntity
                    {
                        CaseSolution_Id = caseSolutionInputViewModel.CaseSolution.Id,
                        Property_Name = lk.CaseSolutionConditionCaption,
                        Values = lk.CaseSolutionConditionValues
                    };
                    this._caseSolutionConditionService.Save(o);
                }
            }


            return Convert.ToInt32(casesoilutionid);



            //ModelState.Clear();
            //ModelState.Remove("Id");
            //var caseSolution = this._caseSolutionService.GetCaseSolution(casesoilutionid);


            //int? backToPageId = null;
            //if (backToPageId == null)
            //    ViewBag.PageId = 0;
            //else
            //    ViewBag.PageId = backToPageId;


            //var caseSolution1 = this._caseSolutionService.GetCaseSolution(Convert.ToInt32(casesoilutionid));


            //var model = this.CreateInputViewModel(caseSolution);
            //model.isCopy = true;

            //return this.View("/Views/CaseSolution/Edit.cshtml", model);

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
                SortOrder = cs.SortOrder,
                CaseSolutionDescription = cs.CaseSolutionDescription,
                IsShowOnlyActive = caseSolutionSearch.OnlyActive,
                ImageClass = caseSolutionSearch.ImageClass == null ? "icon-plus-sign" : caseSolutionSearch.ImageClass
            }).ToArray();

            var activeTab = SessionFacade.FindActiveTab("CaseSolution");
            activeTab = activeTab ?? "CaseTemplate";

            IList<Priority> p = this._priorityService.GetPriorities(customerId);
            p = p.OrderBy(x => x.Name).ToList();



            IList<ProductArea> pa = this._productAreaService.GetWithHierarchy(customerId);
            pa = pa.OrderBy(x => x.Name).ToList();
            foreach (var k in pa)
            {
                k.Name = Translation.Get(k.Name, Enums.TranslationSource.TextTranslation);
            }

            IList<Application> a = this._caseSolutionService.GetAllApplications(customerId);
            a = a.OrderBy(x => x.Name).ToList();
            foreach (var k in a)
            {
                k.Name = Translation.Get(k.Name, Enums.TranslationSource.TextTranslation);
            }

            IList<WorkingGroupEntity> w = this._workingGroupService.GetAllWorkingGroupsForCustomer(customerId, false);
            w = w.OrderBy(x => x.WorkingGroupName).ToList();
            foreach (var k in w)
            {
                k.WorkingGroupName = Translation.Get(k.WorkingGroupName, Enums.TranslationSource.TextTranslation);
            }

            IList<Status> s = this._statusService.GetStatuses(customerId);
            s = s.OrderBy(x => x.Name).ToList();
            foreach (var k in s)
            {
                k.Name = Translation.Get(k.Name, Enums.TranslationSource.TextTranslation);
            }

            IList<StateSecondary> ss = this._stateSecondaryService.GetStateSecondaries(customerId);
            ss = ss.OrderBy(x => x.Name).ToList();
            foreach (var k in ss)
            {
                k.Name = Translation.Get(k.Name, Enums.TranslationSource.TextTranslation);
            }

            IList<CaseSolutionCategory> csc = this._caseSolutionService.GetCaseSolutionCategories(customerId);
            csc = csc.OrderBy(x => x.Name).ToList();
            foreach (var k in csc)
            {
                k.Name = Translation.Get(k.Name, Enums.TranslationSource.TextTranslation);
            }

            var model = new CaseSolutionIndexViewModel(activeTab)
            {
                Rows = _rows,
                CaseSolutionCategories = csc,
                CaseSolutionSubStatus = ss,
                CaseSolutionWGroup = w,
                CaseSolutionPriorities = p,
                CaseSolutionStatuses = s,
                CaseSolutionProductArea = pa,
                CaseSolutionUserWGroup = w,
                CaseSolutionCTemplateProductArea = pa,
                CaseSolutionApplication = a
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

            //Get not selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSetting = new List<CaseSolutionSettingsField>();
            //lFieldSetting = _caseSolutionConditionService.GetCaseSolutionFieldSetting(caseSolution.Id);
            List<SelectListItem> feildSettings = null;
            //feildSettings = lFieldSetting
            //      .Select(x => new SelectListItem
            //      {
            //          Text = x.Text,
            //          Value = x.CaseSolutionConditionId.ToString(),
            //          Selected = false
            //      }).ToList();
            //Get not selected case solution conditions

            //Get selected case solution conditions
            IEnumerable<CaseSolutionSettingsField> lFieldSettingSelected = new List<CaseSolutionSettingsField>();
            lFieldSettingSelected = _caseSolutionConditionService.GetSelectedCaseSolutionFieldSetting(caseSolution.Id, Convert.ToInt32(curCustomerId));
            foreach (var k in lFieldSettingSelected)
            {
                if (k.SelectList != null)
                {
                    foreach (var l in k.SelectList)
                    {
                        l.Text = Translation.Get(l.Text, Enums.TranslationSource.TextTranslation);
                    }
                }
            }

            string selval = string.Empty;
            string bagresult = string.Empty;


            foreach (var jj in lFieldSettingSelected.OrderBy(z => z.PropertyName))
            {
                selval = string.Empty;
                if (jj.SelectedValues != null)
                {
                    foreach (var nn in jj.SelectedValues)
                    {
                        if (selval == string.Empty)
                        {
                            selval = jj.PropertyName + ":" + nn.ToString();
                        }
                        else
                        {
                            selval = selval + "_" + nn.ToString();
                        }

                    }
                }

                if (bagresult == string.Empty)
                {
                    if (selval != string.Empty)
                    {
                        bagresult = selval;
                    }
                }
                else
                {
                    if (selval != string.Empty)
                    {
                        bagresult = bagresult + "~" + selval;
                    }
                }

            }
            ViewBag.selectedValues = bagresult;
            //Get selected case solution conditions






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

            IList<CaseSolution> splitToCaseSolutions = _caseSolutionService.GetCaseSolutions(curCustomerId).Where(z => z.Status == 1).ToList();

            //TODO: better naming here
            //TODO: make a setting per customer if they should be able to choose only from customer, or all customers 
            IList<SelectListItem> splitToAllCaseSolutions = _caseSolutionService.GetCaseSolutions().Select(x => new SelectListItem
            {
                Text = x.Customer.Name + " " + x.Name.ToString(),
                Value = x.Id.ToString(),
                Selected = (caseSolution.SplitToCaseSolutionDescendants != null ? (caseSolution.SplitToCaseSolutionDescendants.Where(a => a.SplitToCaseSolution_Id == x.Id).Any() == true ? true : false) : false)
             }).ToList();

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

                CaseSolutionFieldSettings = feildSettings,
                CSSettingsField = lFieldSetting.ToList(),
                CSSelectedSettingsField = lFieldSettingSelected.ToList(),
                SplitToCaseSolutions = splitToCaseSolutions,
                SplitToAllCaseSolutions = splitToAllCaseSolutions


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
            model.CustomerSetting = _settingService.GetCustomerSetting(model.CaseSolution.Customer_Id);

            model.isCopy = false;

            model.SplitToCaseSolutionIds = caseSolution.SplitToCaseSolutionDescendants.Select(x => x.SplitToCaseSolution_Id).ToArray();



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
