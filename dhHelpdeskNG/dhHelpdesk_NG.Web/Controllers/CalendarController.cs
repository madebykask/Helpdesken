namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;

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
            this._calendarService = calendarService;
            this._systemService = systemService;
            this._workingGroupService = workingGroupService;
        }

        public ActionResult Index()
        {
            var model = this.IndexViewModel();

            CalendarSearch CS = new CalendarSearch();
            if (SessionFacade.CurrentCalenderSearch != null)
            {                
                CS = SessionFacade.CurrentCalenderSearch;
                model.Calendars = this._calendarService.SearchAndGenerateCalendar(SessionFacade.CurrentCustomer.Id, CS);
                model.SearchCs = CS.SearchCs;
            }
            else
            {
                model.Calendars = this._calendarService.GetCalendars(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.CalendarDate).ToList();
                CS.SortBy = "CalendarDate";
                CS.Ascending = true;
                SessionFacade.CurrentCalenderSearch = CS;
            }
            
            
            return this.View(model);
        }


        [HttpPost]       
        public ActionResult Index(CalendarSearch SearchCalendars)
        {
            CalendarSearch CS = new CalendarSearch();
            if (SessionFacade.CurrentCalenderSearch != null)
                CS = SessionFacade.CurrentCalenderSearch;
            
            CS.SearchCs = SearchCalendars.SearchCs;
           
            var c = this._calendarService.SearchAndGenerateCalendar(SessionFacade.CurrentCustomer.Id, CS);

            if (SearchCalendars!=null)
              SessionFacade.CurrentCalenderSearch = CS;

            var model = this.IndexViewModel();

            model.Calendars = c;
            model.SearchCs = CS.SearchCs;

            return this.View(model);
        }

        public ActionResult Sort(string FieldName)
        {
            var model = this.IndexViewModel();
            CalendarSearch CS = new CalendarSearch();
            if (SessionFacade.CurrentCalenderSearch != null)
                CS = SessionFacade.CurrentCalenderSearch;
            CS.Ascending = !CS.Ascending;
            CS.SortBy = FieldName;
            SessionFacade.CurrentCalenderSearch = CS;
            return this.View(model);            
        }

        public ActionResult New()
        {
            var model = this.CreateInputViewModel(new Calendar { Customer_Id = SessionFacade.CurrentCustomer.Id, CalendarDate = DateTime.Now, ShowUntilDate = DateTime.Now });

            return this.View(model);
        }

        [HttpPost]
        public ActionResult New(Calendar calendar, int[] WGsSelected)
        {
            IDictionary<string, string> errors = new Dictionary<string, string>();
            calendar.ChangedByUser_Id = SessionFacade.CurrentUser.Id;   
            this._calendarService.SaveCalendar(calendar, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "calendar");

            var model = this.CreateInputViewModel(calendar);

            return this.View(model);
        }

        public ActionResult Edit(int id)
        {
            var calendar = this._calendarService.GetCalendar(id);

            if (calendar == null)
                return new HttpNotFoundResult("No calendar found...");

            var model = this.CreateInputViewModel(calendar);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, Calendar calendar, int[] WGsSelected)
        {
            var calendarToSave = this._calendarService.GetCalendar(id);
            this.UpdateModel(calendarToSave, "Calendar");

            calendarToSave.ShowOnStartPage = calendar.ShowOnStartPage;
            calendarToSave.PublicInformation = calendar.PublicInformation;

            IDictionary<string, string> errors = new Dictionary<string, string>();
            this._calendarService.SaveCalendar(calendarToSave, WGsSelected, out errors);

            if (errors.Count == 0)
                return this.RedirectToAction("index", "calendar");

            var model = this.CreateInputViewModel(calendarToSave);

            return this.View(model);
        }
 
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this._calendarService.DeleteCalendar(id) == DeleteMessage.Success)
                return this.RedirectToAction("index", "calendar");
            else
            {
                this.TempData.Add("Error", "");
                return this.RedirectToAction("edit", "calendar", new { id = id });
            }
        }

        private CalendarIndexViewModel IndexViewModel()
        {
            var model = new CalendarIndexViewModel { };

            return model;
        }

        private CalendarInputViewModel CreateInputViewModel(Calendar calendar)
        {
            var wgsSelected = calendar.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();

            foreach (var wg in this._workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id))
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
