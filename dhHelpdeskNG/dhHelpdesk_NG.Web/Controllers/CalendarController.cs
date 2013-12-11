using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;
using dhHelpdesk_NG.Service;
using dhHelpdesk_NG.Web.Infrastructure;
using dhHelpdesk_NG.Web.Models;

namespace dhHelpdesk_NG.Web.Controllers
{
    public class CalendarController : BaseController
    {
        private readonly ICalendarService _calendarService;
        private readonly ISystemService _systemService;
        private readonly IWorkingGroupService _workingGroupService;

        public CalendarController(
            ICalendarService calendarService,
            ISystemService systemService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService)
            : base(masterDataService)
        {
            _calendarService = calendarService;
            _systemService = systemService;
            _workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = IndexViewModel();

            model.Calendars = _calendarService.GetCalendars(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.ChangedDate).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CalendarSearch SearchCalendars)
        {
            var c = _calendarService.SearchAndGenerateCalendar(SessionFacade.CurrentCustomer.Id, SearchCalendars);
            var model = IndexViewModel();

            model.Calendars = c;

            return View(model);
        }

        public ActionResult New()
        {
            var model = CreateInputViewModel(new Calendar { Customer_Id = SessionFacade.CurrentCustomer.Id, CalendarDate = DateTime.Now, ShowUntilDate = DateTime.Now });

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Calendar calendar, int[] WGsSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            calendar.ChangedByUser_Id = SessionFacade.CurrentUser.Id;   
            _calendarService.SaveCalendar(calendar, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "calendar");

            var model = CreateInputViewModel(calendar);

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var calendar = _calendarService.GetCalendar(id);

            if (calendar == null)
                return new HttpNotFoundResult("No calendar found...");

            var model = CreateInputViewModel(calendar);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, Calendar calendar, int[] WGsSelected)
        {
            var calendarToSave = _calendarService.GetCalendar(id);
            UpdateModel(calendarToSave, "Calendar");

            calendarToSave.ShowOnStartPage = calendar.ShowOnStartPage;
            calendarToSave.PublicInformation = calendar.PublicInformation;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            _calendarService.SaveCalendar(calendarToSave, WGsSelected, out errors);

            if (errors.Count == 0)
                return RedirectToAction("index", "calendar");

            var model = CreateInputViewModel(calendarToSave);

            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (_calendarService.DeleteCalendar(id) == DeleteMessage.Success)
                return RedirectToAction("index", "calendar");
            else
            {
                TempData.Add("Error", "");
                return RedirectToAction("edit", "calendar", new { id = id });
            }
        }

        private CalendarIndexViewModel IndexViewModel()
        {
            var model = new CalendarIndexViewModel { };

            return model;
        }

        private CalendarInputViewModel CreateInputViewModel(Calendar calendar)
        {
            var wgsSelected = calendar.WGs ?? new List<WorkingGroup>();
            var wgsAvailable = new List<WorkingGroup>();

            foreach (var wg in _workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id))
            {
                if (!wgsSelected.Contains(wg))
                    wgsAvailable.Add(wg);
            }

            var model = new CalendarInputViewModel
            {
                Calendar = calendar,
                WGsAvailable = wgsAvailable.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
                WGsSelected = wgsSelected.Select(x => new SelectListItem
                {
                    Text = x.WorkingGroupName,
                    Value = x.Id.ToString()
                }).ToList(),
            };

            return model;
        }
    }
}
