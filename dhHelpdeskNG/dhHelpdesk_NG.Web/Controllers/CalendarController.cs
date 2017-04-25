// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CalendarController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.Common.Extensions.Integer;

namespace DH.Helpdesk.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Models;
using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.BusinessData.Enums.Admin.Users;

    /// <summary>
    /// The calendar controller.
    /// </summary>
    //[CustomAuthorize(UserPermsissions = "CalendarPermission", Roles = "3,4")]
    public class CalendarController : BaseController
    {
        /// <summary>
        /// The _calendar service.
        /// </summary>
        private readonly ICalendarService calendarService;

        /// <summary>
        /// The working group service.
        /// </summary>
        private readonly IWorkingGroupService workingGroupService;
        private readonly IUserPermissionsChecker _userPermissionsChecker;
        private readonly ISettingService _settingService;
        private readonly IUserService _userService;
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarController"/> class.
        /// </summary>
        /// <param name="calendarService">
        /// The calendar service.
        /// </param>
        /// <param name="workingGroupService">
        /// The working group service.
        /// </param>
        /// <param name="masterDataService">
        /// The master data service.
        /// </param>
        public CalendarController(
            ICalendarService calendarService,
            IWorkingGroupService workingGroupService,
            IMasterDataService masterDataService,
            IUserPermissionsChecker userPermissionsChecker,
            ISettingService settingService,
            IUserService userService)
            : base(masterDataService)
        {
            this.calendarService = calendarService;
            this.workingGroupService = workingGroupService;
            this._userPermissionsChecker = userPermissionsChecker;
            this._settingService = settingService;
            this._userService = userService;
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Index()
        {
            var model = this.IndexViewModel();
            var customerSetting = this._settingService.GetCustomerSettings(SessionFacade.CurrentCustomer.Id);
            var calendarWGRestriction = customerSetting.CalendarWGRestriction;

            var cs = new CalendarSearch();
            if (SessionFacade.CurrentCalenderSearch != null)
            {                
                cs = SessionFacade.CurrentCalenderSearch;
                if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                    model.Calendars = this.calendarService.SearchAndGenerateCalendar(SessionFacade.CurrentCustomer.Id, cs, true, calendarWGRestriction);
                else
                    model.Calendars = this.calendarService.SearchAndGenerateCalendar(SessionFacade.CurrentCustomer.Id, cs);
                model.SearchCs = cs.SearchCs;
            }
            else
            {
                if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
                    model.Calendars = this.calendarService.GetCalendars(SessionFacade.CurrentCustomer.Id, true, calendarWGRestriction).OrderBy(x => x.CalendarDate).ToList();
                else
                    model.Calendars = this.calendarService.GetCalendars(SessionFacade.CurrentCustomer.Id).OrderBy(x => x.CalendarDate).ToList();

                cs.SortBy = "CalendarDate";
                cs.Ascending = true;
                SessionFacade.CurrentCalenderSearch = cs;
            }
                        
            return this.View(model);
        }

        /// <summary>
        /// The index.
        /// </summary>
        /// <param name="searchCalendars">
        /// The search calendars.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]       
        public ActionResult Index(CalendarSearch searchCalendars)
        {
            var customerSetting = this._settingService.GetCustomerSettings(SessionFacade.CurrentCustomer.Id);
            var calendarWGRestriction = customerSetting.CalendarWGRestriction;

            var cs = new CalendarSearch();
            if (SessionFacade.CurrentCalenderSearch != null)
            {
                cs = SessionFacade.CurrentCalenderSearch;
            }
            
            cs.SearchCs = searchCalendars.SearchCs;

            var restrictsearch = false;
            if (SessionFacade.CurrentUser.UserGroupId == 1 || SessionFacade.CurrentUser.UserGroupId == 2)
            {
                restrictsearch = true;
            }
            else
                calendarWGRestriction = false;

            var c = this.calendarService.SearchAndGenerateCalendar(SessionFacade.CurrentCustomer.Id, cs, restrictsearch, calendarWGRestriction);
            SessionFacade.CurrentCalenderSearch = cs;

            var model = this.IndexViewModel();
            model.Calendars = c;
            model.SearchCs = cs.SearchCs;

            return this.View(model);
        }

        /// <summary>
        /// The sort.
        /// </summary>
        /// <param name="fieldName">
        /// The field name.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public void Sort(string fieldName)
        {
            var cs = new CalendarSearch();
            if (SessionFacade.CurrentCalenderSearch != null)
            {
                cs = SessionFacade.CurrentCalenderSearch;
            }

            cs.Ascending = !cs.Ascending;
            cs.SortBy = fieldName;
            var c = this.calendarService.SearchAndGenerateCalendar(SessionFacade.CurrentCustomer.Id, cs);
            SessionFacade.CurrentCalenderSearch = cs;

            var model = this.IndexViewModel();
            model.Calendars = c;
            model.SearchCs = cs.SearchCs;

            //return this.View("Index", model);            
        }

        /// <summary>
        /// The new.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult New()
        {
            var today = DateTime.Today;
            var model = this.CreateInputViewModel(new CalendarOverview
                                                      {
                                                          CustomerId = SessionFacade.CurrentCustomer.Id, 
                                                          CalendarDate = today, 
                                                          ShowFromDate = today,
                                                          ShowUntilDate = today
                                                      }, true);

            return this.View(model);
        }

        /// <summary>
        /// The new.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <param name="WGsSelected">
        /// The work groups selected.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult New(CalendarOverview calendar, int[] WGsSelected)
        {
            if (!ModelState.IsValid)
            {
                var model = this.CreateInputViewModel(calendar);
                return this.View(model);
            }

            calendar.ChangedByUserId = SessionFacade.CurrentUser.Id;
            this.calendarService.SaveCalendar(calendar, WGsSelected);

            return this.RedirectToAction("Index", "Calendar");
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var calendar = this.calendarService.GetCalendar(id);

            if (calendar == null)
            {
                return new HttpNotFoundResult("No calendar found...");
            }

            var model = this.CreateInputViewModel(calendar);

            return this.View(model);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <param name="WGsSelected">
        /// The work groups.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, CalendarOverview calendar, int[] WGsSelected)
        {
            if (!ModelState.IsValid)
            {
                var model = this.CreateInputViewModel(calendar);
                return this.View(model);                
            }

            this.calendarService.SaveCalendar(calendar, WGsSelected);

            return this.RedirectToAction("Index", "Calendar");
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (this.calendarService.DeleteCalendar(id) == DeleteMessage.Success)
            {
                return this.RedirectToAction("index", "calendar");
            }

            this.TempData.Add("Error", string.Empty);
            return this.RedirectToAction("Edit", "Calendar", new { id = id });
        }

        /// <summary>
        /// The index view model.
        /// </summary>
        /// <returns>
        /// The <see cref="CalendarIndexViewModel"/>.
        /// </returns>
        private CalendarIndexViewModel IndexViewModel()
        {
            var userHasCalenderAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CalendarPermission);

            var model = new CalendarIndexViewModel();
            model.UserHasCalendarAdminPermission = userHasCalenderAdminPermission;

            return model;
        }

        /// <summary>
        /// The create input view model.
        /// </summary>
        /// <param name="calendar">
        /// The calendar.
        /// </param>
        /// <param name="isNewModel"></param>
        /// <returns>
        /// The <see cref="CalendarInputViewModel"/>.
        /// </returns>
        private CalendarInputViewModel CreateInputViewModel(CalendarOverview calendar, bool isNewModel = false)
        {
            var wgsSelected = calendar.WGs ?? new List<WorkingGroupEntity>();
            var wgsAvailable = new List<WorkingGroupEntity>();

            var user = this._userService.GetUser(SessionFacade.CurrentUser.Id);

            var workingGroups = this.workingGroupService.GetWorkingGroups(SessionFacade.CurrentCustomer.Id, user.Id);
            var wgsSelectedIds = wgsSelected.Select(g => g.Id).ToList();
            if (isNewModel)
            {
                var defaultWg = workingGroups.FirstOrDefault(x => x.IsDefaultCalendar.ToBool());
                if (defaultWg != null && !wgsSelectedIds.Contains(defaultWg.Id))
                {
                    wgsSelectedIds.Add(defaultWg.Id);
                    wgsSelected.Add(defaultWg);
                }
            }

            var userHasCalenderAdminPermission = this._userPermissionsChecker.UserHasPermission(UsersMapper.MapToUser(SessionFacade.CurrentUser), UserPermission.CalendarPermission);

            foreach (var wg in workingGroups)
            {
                if (!wgsSelectedIds.Contains(wg.Id))
                {
                    wgsAvailable.Add(wg);
                }
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
                UserHasCalendarAdminPermission = userHasCalenderAdminPermission,
                CurrentUser = user,
            };
            return model;
        }
    }
}
