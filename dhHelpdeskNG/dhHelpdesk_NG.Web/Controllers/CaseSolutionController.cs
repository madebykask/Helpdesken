
namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Common.Enums.Settings;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Requests.Cases;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;
    using DH.Helpdesk.Web.Models.Case;

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

        private readonly ICaseSolutionSettingService caseSolutionSettingService;

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
            ICausingPartService causingPartService)
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
        }

        [HttpPost]
        public void RememberTab(string topic, string tab)
        {
            SessionFacade.SaveActiveTab(topic, tab);
        }

        public ActionResult Index()
        {
            var model = this.IndexInputViewModel();
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
            {
                CS = SessionFacade.CurrentCaseSolutionSearch;
                var CaseSolutions = this._caseSolutionService.SearchAndGenerateCaseSolutions(SessionFacade.CurrentCustomer.Id, CS);
                //Only return casesolution where templatepath is null - these case solutions are E-Forms shown in myhr/linemanager/selfservice
                model.CaseSolutions = CaseSolutions.OrderBy(x => x.Name).Where(x => x.TemplatePath == null).ToList();
                model.SearchCss = CS.SearchCss;
            }
            else
            {
                //Only return casesolution where templatepath is null - these case solutions are E-Forms shown in myhr/linemanager/selfservice
                model.CaseSolutions = this._caseSolutionService.GetCaseSolutions(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.Name).Where(x => x.TemplatePath == null).ToList();
                CS.SortBy = "Name";
                CS.Ascending = true;
                SessionFacade.CurrentCaseSolutionSearch = CS;
            }


            if (string.IsNullOrEmpty(SessionFacade.FindActiveTab("CaseSolution")))
                SessionFacade.SaveActiveTab("CaseSolution", "CaseTemplate");

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Index(CaseSolutionSearch SearchCaseSolutions)
        {
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                CS = SessionFacade.CurrentCaseSolutionSearch;

            CS.SearchCss = SearchCaseSolutions.SearchCss;

            var caseSol = this._caseSolutionService.SearchAndGenerateCaseSolutions(SessionFacade.CurrentCustomer.Id, CS).OrderBy(x => x.Name).Where(x => x.TemplatePath == null).ToList();

            if (SearchCaseSolutions != null)
                SessionFacade.CurrentCaseSolutionSearch = CS;

            var model = this.IndexInputViewModel();

            model.CaseSolutions = caseSol;
            model.SearchCss = CS.SearchCss;

            return this.View(model);
        }

        public void Sort(string fieldName)
        {
            var model = this.IndexInputViewModel();
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                CS = SessionFacade.CurrentCaseSolutionSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = fieldName;
            SessionFacade.CurrentCaseSolutionSearch = CS;
        }

        public ActionResult New(int? backToPageId)
        {
            // Positive: Send Mail to...
            var caseSolution = new CaseSolution (){ Customer_Id = SessionFacade.CurrentCustomer.Id, NoMailToNotifier = 1};

            if (backToPageId == null)
                ViewBag.PageId = 0;
            else
                ViewBag.PageId = backToPageId;

            if (caseSolution == null)
                return new HttpNotFoundResult("No case solution found...");

            var model = this.CreateInputViewModel(caseSolution);

            return this.View(model);
        }


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

            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            CaseSettingsSolutionAggregate settingsSolutionAggregate = this.CreateCaseSettingsSolutionAggregate(caseSolutionInputViewModel.CaseSolution.Id, caseSolutionSettingModels);
            this.caseSolutionSettingService.AddCaseSolutionSettings(settingsSolutionAggregate);

            if (errors.Count == 0)
            {
                switch (PageId) // back to refrence page
                {
                    case 0:
                        return this.RedirectToAction("index", "casesolution");
                        break;

                    case 1:
                        return this.RedirectToAction("index", "Cases");
                        break;
                }
            }

            this.TempData["RequiredFields"] = errors;

            var model = this.CreateInputViewModel(caseSolution);

            return this.View(model);
        }

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

        [HttpPost]
        public ActionResult GetTemplate(int id)
        {
            var caseSolution = this._caseSolutionService.GetCaseSolution(id);

            if (caseSolution == null)
            {
                return new HttpNotFoundResult("No case solution found...");
            }

            /// This strange logic I took from Edit() action
            caseSolution.NoMailToNotifier = caseSolution.NoMailToNotifier == 0 ? 1 : 0;

            return this.Json(new
                                 {
                                     caseSolution.CaseType_Id,
                                     caseSolution.PerformerUser_Id,
                                     caseSolution.Category_Id,
                                     caseSolution.ReportedBy,
                                     caseSolution.Department_Id,
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
                                     caseSolution.FinishingCause_Id
                                 });
        }

        [HttpPost]
        public ActionResult Edit(CaseSolutionInputViewModel caseSolutionInputViewModel, CaseSolutionSettingModel[] CaseSolutionSettingModels, int PageId)
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

            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

            CaseSettingsSolutionAggregate settingsSolutionAggregate =
                this.CreateCaseSettingsSolutionAggregate(
                    caseSolutionInputViewModel.CaseSolution.Id,
                    CaseSolutionSettingModels);
            this.caseSolutionSettingService.UpdateCaseSolutionSettings(settingsSolutionAggregate);

            if (errors.Count == 0)
            {
                switch (PageId) // back to refrence page
                {
                    case 1:
                        return this.RedirectToAction("index", "Cases");
                        break;

                    default:
                        return this.RedirectToAction("index", "casesolution");

                }
            }

            this.TempData["RequiredFields"] = errors;
            var model = this.CreateInputViewModel(caseSolutionInputViewModel.CaseSolution);

            return this.View(model);
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
        public RedirectToRouteResult Delete(int id, int pageId)
        {
            if (this._caseSolutionService.DeleteCaseSolution(id) == DeleteMessage.Success)
            {
                switch (pageId)
                {
                    case 1:
                        return this.RedirectToAction("index", "Cases");
                        break;

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

        private CaseSolutionIndexViewModel IndexInputViewModel()
        {
            var activeTab = SessionFacade.FindActiveTab("CaseSolution");
            activeTab = (activeTab == null) ? "CaseTemplate" : activeTab;
            var model = new CaseSolutionIndexViewModel(activeTab)
            {
                CaseSolutions = this._caseSolutionService.GetCaseSolutions(SessionFacade.CurrentCustomer.Id),
                CaseSolutionCategories = this._caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id)
            };

            return model;
        }

        private CaseSolutionInputViewModel CreateInputViewModel(CaseSolution caseSolution)
        {
            var cs = this._settingService.GetCustomerSetting(SessionFacade.CurrentCustomer.Id);

            var model = new CaseSolutionInputViewModel
            {
                CaseSolution = caseSolution,
                CaseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id),
                Countries = this._countryService.GetCountries(SessionFacade.CurrentCustomer.Id),
                currencies = this._currencyService.GetCurrencies(),
                CsCategories = this._caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseTypes = this._caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id),

                CaseWorkingGroups = this._workingGroupService.GetAllWorkingGroupsForCustomer(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                Categories = this._categoryService.GetCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                FinishingCauses = this._finishingCauseService.GetFinishingCauses(SessionFacade.CurrentCustomer.Id),

                PerformerUsers = this._userService.GetUsers(SessionFacade.CurrentCustomer.Id)
                                 .Where(x => x.IsActive == 1)
                                 .Select(x => new SelectListItem() 
                                 {
                                     Text = x.SurName + " " + x.FirstName,
                                        Value = x.Id.ToString()
                                 }).ToList(),

                Priorities = this._priorityService.GetPriorities(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                ProductAreas = this._productAreaService.GetProductAreas(SessionFacade.CurrentCustomer.Id),

                WorkingGroups = this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                Departments = this._departmentService.GetDepartments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.DepartmentName,
                    Value = x.Id.ToString()
                }).ToList(),

                Regions = this._regionService.GetActiveRegions(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                OUs = this._ouService.GetOUs(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Systems = this._systemService.GetSystems(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.SystemName,
                    Value = x.Id.ToString()
                }).ToList(),

                Urgencies = this._urgencyService.GetUrgencies(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Impacts = this._impactService.GetImpacts(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Status = this._statusService.GetActiveStatuses(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                StateSecondaries = this._stateSecondaryService.GetActiveStateSecondaries(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Suppliers = this._supplierService.GetActiveSuppliers(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CausingParts = this._causingPartService.GetActiveCausingParts(SessionFacade.CurrentCustomer.Id).Where(x => x.ParentId == null).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            if (model.CaseSolution.Id == 0)
            {
                var defaultCat = _caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id).Where(c => c.IsDefault == 1).FirstOrDefault();
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
                    model.ParantPath_ProductArea = c.getProductAreaParentPath();
            }

            if (cs.ModuleProject == 1)
            {
                model.projects = this._projectService.GetCustomerProjects(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
 
            }

            if (cs.ModuleProblem == 1)
            {
                model.problems = this._problemService.GetCustomerProblems(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            }

            if (cs.ModuleChangeManagement == 1)
            {
                model.changes = this._changeService.GetChanges(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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

            return model;
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
    }
}
