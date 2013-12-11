using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Areas.Admin.Models;
using dhHelpdesk_NG.Web.Infrastructure;
using System;

namespace dhHelpdesk_NG.Web.Areas.Admin.Controllers
{
    public class CaseSolutionController : BaseController
    {
        private readonly ICaseService _caseService;
        private readonly ICaseSolutionService _caseSolutionService;
        private readonly ICustomerService _customerService;
        private readonly IFinishingCauseService _finishingCauseService;
        private readonly ISystemService _systemService;
        private readonly IWorkingGroupService _workingGroupService;

        public CaseSolutionController(
            ICaseService caseService,
            ICaseSolutionService caseSolutionService,
            ICustomerService customerService,
            IFinishingCauseService finishingCauseService,
            ISystemService systemService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _caseService = caseService;
            _caseSolutionService = caseSolutionService;
            _customerService = customerService;
            _finishingCauseService = finishingCauseService;
            _systemService = systemService;
            _workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var caseSolutions = _caseSolutionService.GetCaseSolutions(SessionFacade.CurrentCustomer.Id);

            if (caseSolutions == null)
                return new HttpNotFoundResult("No case solution found...");

            return View(caseSolutions);
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
                return RedirectToAction("index", "casesolution", new { area = "admin" });

            return View(caseSolutionInputViewModel);
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
                return RedirectToAction("index", "casesolution", new { area = "admin" });

            var model = CreateInputViewModel(caseSolutionInputViewModel.CaseSolution);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_caseSolutionService.DeleteCaseSolution(id) == DeleteMessage.Success)
                return RedirectToAction("index", "casesolution", new { area = "admin" });
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "casesolution", new { area = "admin", id = id });
            }
        }

        private CaseSolutionInputViewModel CreateInputViewModel(CaseSolution caseSolution)
        {
            var model = new CaseSolutionInputViewModel
            {
                CaseSolution = caseSolution,

                CaseFieldSettings = _caseService.GetCaseFieldSettings(SessionFacade.CurrentCustomer.Id),

                CaseSolutionCategories = _caseSolutionService.GetCaseSolutionCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseTypes = _caseService.GetCaseTypes(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                CaseWorkingGroups = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Categories = _caseService.GetCategories(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                FinishingCauses = _finishingCauseService.GetFinishingCauses(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                PerformerUsers = _systemService.GetUsers().Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.Surname,
                    Value = x.Id.ToString()
                }).ToList(),

                Priorities = _workingGroupService.GetPriorities(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                ProductAreas = _caseService.GetProductAreas(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Projects = _workingGroupService.GetProjects(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),

                Workinggroups = _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };

            model.Schedule = false;

            var schedule = _caseSolutionService.GetCaseSolutionSchedule(caseSolution.Id);
            
            model.ScheduleDays = string.Empty;
            model.ScheduleMonths = string.Empty;
            model.Schedule = false;

            if(schedule != null)
            {
                model.Schedule = true;
                model.ScheduleTime = (int)schedule.ScheduleTime;
                model.ScheduleType = schedule.ScheduleType;
                model.ScheduleWatchDate = schedule.ScheduleWatchDate;
                
                model.ScheduleMonths = "";
                model.ScheduleDays = "";
                
                if(schedule.ScheduleDay != null)
                    model.ScheduleDays = schedule.ScheduleDay;

                int pos = 0;
                pos = model.ScheduleDays.IndexOf(";", 0);

                if(pos > 0)
                {
                    model.ScheduleMonths = model.ScheduleDays.Substring(pos + 1);
                    model.ScheduleDays = model.ScheduleDays.Substring(0, pos);
                }

                pos = model.ScheduleDays.IndexOf(":", 0);

                if(pos > 0)
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

            if(scheduleType == 1)
                caseSolutionSchedule.ScheduleDay = null;                 

            if(scheduleType == 2)
            {
                caseSolutionSchedule.ScheduleDay = "," + string.Join(",", scheduleDay) + ",";
            }

            if(scheduleType == 3)
            {
                if(scheduleMonthly == 1)
                {
                    caseSolutionSchedule.ScheduleDay = scheduleMonthlyDay + ";," + string.Join(",", scheduleMonth) + ",";
                }

                if(scheduleMonthly == 2)
                {
                    caseSolutionSchedule.ScheduleDay = scheduleMonthlyOrder + ":" + scheduleMonthlyWeekday + ";," + string.Join(",", scheduleMonth) + ",";
                }
            }

            return caseSolutionSchedule;
        }
    }
}
