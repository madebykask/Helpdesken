using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class CaseSolutionController : BaseController
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
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _caseFieldSettingService = caseFieldSettingService;
            _caseSolutionService = caseSolutionService;
            _caseTypeService = caseTypeService;
            _categoryService = categoryService;
            _finishingCauseService = finishingCauseService;
            _priorityService = priorityService;
            _productAreaService = productAreaService;
            _projectService = projectService;
            _userService = userService;
            _workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = IndexInputViewModel();

            if (SessionFacade.CurrentCaseSolutionSearch != null)
            {
                CaseSolutionSearch CS = new CaseSolutionSearch();
                CS = SessionFacade.CurrentCaseSolutionSearch;
                model.CaseSolutions = _caseSolutionService.SearchAndGenerateCaseSolutions(SessionFacade.CurrentCustomer.Id, CS);
                model.SearchCss = CS.SearchCss;
            }
            else
                model.CaseSolutions = _caseSolutionService.GetCaseSolutions(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.ChangedDate).ToList();
                        
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CaseSolutionSearch SearchCaseSolutions)
        {
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                CS = SessionFacade.CurrentCaseSolutionSearch;

            CS.SearchCss = SearchCaseSolutions.SearchCss;

            var caseSol = _caseSolutionService.SearchAndGenerateCaseSolutions(SessionFacade.CurrentCustomer.Id, CS);

            if (SearchCaseSolutions != null)
                SessionFacade.CurrentCaseSolutionSearch = CS;

            var model = IndexInputViewModel();

            model.CaseSolutions = caseSol;
            model.SearchCss = CS.SearchCss;

            return View(model);
        }

        public ActionResult Sort(string FieldName)
        {
            var model = IndexInputViewModel();
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                CS = SessionFacade.CurrentCaseSolutionSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = FieldName;
            SessionFacade.CurrentCaseSolutionSearch = CS;
            return View(model);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new CaseSolution { Customer_Id = SessionFacade.CurrentCustomer.Id });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(CaseSolution caseSolution, CaseSolutionInputViewModel caseSolutionInputViewModel)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            //_caseSolutionService.SaveCaseSolution(returnCaseSolutionForNew(caseSolutionInputViewModel), out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "casesolution");

            return View(caseSolutionInputViewModel);
        }

        public ActionResult NewCategory()
        {
            return View(new CaseSolutionCategory() { Customer_Id = SessionFacade.CurrentCustomer.Id });
        }

        [HttpPost]
        public ActionResult NewCategory(CaseSolutionCategory caseSolutionCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _caseSolutionService.SaveCaseSolutionCategory(caseSolutionCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "casesolution");

            return View(caseSolutionCategory);
        }

        public ActionResult Edit(int id)
        {
            var caseSolution = _caseSolutionService.GetCaseSolution(id);

            if (caseSolution == null)
                return new HttpNotFoundResult("No case solution found...");

            var model = CreateInputViewModel(caseSolution);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(CaseSolutionInputViewModel caseSolutionInputViewModel)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            var caseSolutionSchedule = CreateCaseSolutionSchedule(caseSolutionInputViewModel);

            _caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "casesolution");

            var model = CreateInputViewModel(caseSolutionInputViewModel.CaseSolution);

            return View(model);
        }

        public ActionResult EditCategory(int id)
        {
            var caseSolutionCategory = _caseSolutionService.GetCaseSolutionCategory(id);

            if (caseSolutionCategory == null)
                return new HttpNotFoundResult("No case solution cateogry found...");

            return View(caseSolutionCategory);
        }

        [HttpPost]
        public ActionResult EditCategory(CaseSolutionCategory caseSolutionCategory)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            _caseSolutionService.SaveCaseSolutionCategory(caseSolutionCategory, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "caseSolution");

            return View(caseSolutionCategory);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_caseSolutionService.DeleteCaseSolution(id) == DeleteMessage.Success)
                return RedirectToAction("index", "casesolution");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "casesolution", new { id = id });
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            if (_caseSolutionService.DeleteCaseSolutionCategory(id) == DeleteMessage.Success)
                return RedirectToAction("index", "casesolution");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("editcategory", "casesolution", new { id = id });
            }
        }

        private CaseSolutionIndexViewModel IndexInputViewModel()
        {
            var model = new CaseSolutionIndexViewModel
            {
                CaseSolutions = _caseSolutionService.GetCaseSolutions(SessionFacade.CurrentCustomer.Id),
                CaseSolutionCategories = _caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id)
            };

            return model;
        }

        private CaseSolutionInputViewModel CreateInputViewModel(CaseSolution caseSolution)
        {
            var model = new CaseSolutionInputViewModel
            {
                CaseSolution = caseSolution,
                CaseFieldSettings = _caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id),
                CsCategories = _caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CaseTypes = _caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                CaseWorkingGroups = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                Categories = _categoryService.GetCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                FinishingCauses = _finishingCauseService.GetFinishingCauses(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                PerformerUsers = _userService.GetUsers().Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.SurName,
                    Value = x.Id.ToString()
                }).ToList(),
                Priorities = _priorityService.GetPriorities(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                ProductAreas = _productAreaService.GetProductAreas(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Projects = _projectService.GetProjects(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                WorkingGroups = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList()                
            };

            model.Schedule = false;

            var schedule = _caseSolutionService.GetCaseSolutionSchedule(caseSolution.Id);

            model.ScheduleDays = string.Empty;
            model.ScheduleMonths = string.Empty;
            model.Schedule = false;

            if (schedule != null)
            {
                model.Schedule = true;
                model.ScheduleTime = (int)schedule.ScheduleTime;
                model.ScheduleType = schedule.ScheduleType;
                model.ScheduleWatchDate = schedule.ScheduleWatchDate;

                model.ScheduleMonths = "";
                model.ScheduleDays = "";

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

            return model;
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

            caseSolutionSchedule.CaseSolution_Id = caseSolutionId;
            caseSolutionSchedule.ScheduleType = scheduleType;
            caseSolutionSchedule.ScheduleTime = scheduleTime;
            caseSolutionSchedule.ScheduleWatchDate = scheduleWatchDate;

            if (scheduleType == 1)
                caseSolutionSchedule.ScheduleDay = null;

            if (scheduleType == 2)
            {
                caseSolutionSchedule.ScheduleDay = "," + string.Join(",", scheduleDay) + ",";
            }

            if (scheduleType == 3)
            {
                if (scheduleMonthly == 1)
                {
                    caseSolutionSchedule.ScheduleDay = scheduleMonthlyDay + ";," + string.Join(",", scheduleMonth) + ",";
                }

                if (scheduleMonthly == 2)
                {
                    caseSolutionSchedule.ScheduleDay = scheduleMonthlyOrder + ":" + scheduleMonthlyWeekday + ";," + string.Join(",", scheduleMonth) + ",";
                }
            }

            return caseSolutionSchedule;
        }
    }
}
