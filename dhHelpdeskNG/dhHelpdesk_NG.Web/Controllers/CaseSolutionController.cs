
namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;

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
        private readonly IDepartmentService _departmentService;

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
            IMasterDataService masterDataService)
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
        }

        public ActionResult Index()
        {            
            var model = this.IndexInputViewModel();
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
            {                
                CS = SessionFacade.CurrentCaseSolutionSearch;
                model.CaseSolutions = this._caseSolutionService.SearchAndGenerateCaseSolutions(SessionFacade.CurrentCustomer.Id, CS);
                model.SearchCss = CS.SearchCss;
            }
            else
            {
                model.CaseSolutions = this._caseSolutionService.GetCaseSolutions(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.Name).ToList();
                CS.SortBy = "Name";
                CS.Ascending = true;
                SessionFacade.CurrentCaseSolutionSearch = CS;
            }
                                     
        

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Index(CaseSolutionSearch SearchCaseSolutions)
        {
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                CS = SessionFacade.CurrentCaseSolutionSearch;

            CS.SearchCss = SearchCaseSolutions.SearchCss;

            var caseSol = this._caseSolutionService.SearchAndGenerateCaseSolutions(SessionFacade.CurrentCustomer.Id, CS);

            if (SearchCaseSolutions != null)
                SessionFacade.CurrentCaseSolutionSearch = CS;

            var model = this.IndexInputViewModel();

            model.CaseSolutions = caseSol;
            model.SearchCss = CS.SearchCss;

            return this.View(model);
        }

        public ActionResult Sort(string FieldName)
        {
            var model = this.IndexInputViewModel();
            CaseSolutionSearch CS = new CaseSolutionSearch();
            if (SessionFacade.CurrentCaseSolutionSearch != null)
                CS = SessionFacade.CurrentCaseSolutionSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = FieldName;
            SessionFacade.CurrentCaseSolutionSearch = CS;
            return this.View(model);
        }

        public ActionResult New(int? backToPageId)
        {
           
            //var model = CreateInputViewModel(new CaseSolution { Customer_Id = SessionFacade.CurrentCustomer.Id });

            //return View(model);

            var caseSolution = new CaseSolution { Customer_Id = SessionFacade.CurrentCustomer.Id };

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
        public ActionResult New(CaseSolution caseSolution, CaseSolutionInputViewModel caseSolutionInputViewModel, int PageId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            IList<CaseFieldSetting> CheckMandatory = null;//_caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id);
            this.TempData["RequiredFields"] = null;

            var caseSolutionSchedule = this.CreateCaseSolutionSchedule(caseSolutionInputViewModel);
                       
            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

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

            if (backToPageId == null)
                ViewBag.PageId = 0;
            else
                ViewBag.PageId = backToPageId;

            var model = this.CreateInputViewModel(caseSolution);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(CaseSolutionInputViewModel caseSolutionInputViewModel, int PageId)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            IList<CaseFieldSetting> CheckMandatory = null; //_caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id); 
            this.TempData["RequiredFields"] = null;

            var caseSolutionSchedule = this.CreateCaseSolutionSchedule(caseSolutionInputViewModel);
            this._caseSolutionService.SaveCaseSolution(caseSolutionInputViewModel.CaseSolution, caseSolutionSchedule, CheckMandatory, out errors);

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
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "casesolution", new {id = id});
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            if (this._caseSolutionService.DeleteCaseSolutionCategory(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "casesolution");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("editcategory", "casesolution", new { id = id });
            }
        }

        private CaseSolutionIndexViewModel IndexInputViewModel()
        {
            var model = new CaseSolutionIndexViewModel
            {
                CaseSolutions = this._caseSolutionService.GetCaseSolutions(SessionFacade.CurrentCustomer.Id),
                CaseSolutionCategories = this._caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id)
            };

            return model;
        }

        private CaseSolutionInputViewModel CreateInputViewModel(CaseSolution caseSolution)
        {
            var model = new CaseSolutionInputViewModel
            {
                CaseSolution = caseSolution,
                CaseFieldSettings = this._caseFieldSettingService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id),
                CsCategories = this._caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseTypes = this._caseTypeService.GetCaseTypes(SessionFacade.CurrentCustomer.Id),
                
                CaseWorkingGroups = this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
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
                
                PerformerUsers = this._userService.GetUsers().Select(x => new SelectListItem
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

                Projects = this._projectService.GetCustomerProjects(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                WorkingGroups = this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),

                Departments = this._departmentService.GetDepartments(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.DepartmentName,
                    Value = x.Id.ToString()
                }).ToList()
                               
            };

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
