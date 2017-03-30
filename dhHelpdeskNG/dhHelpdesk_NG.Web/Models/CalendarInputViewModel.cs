// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarInputViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CalendarIndexViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Calendar.Output;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The calendar index view model.
    /// </summary>
    public class CalendarIndexViewModel
    {
        /// <summary>
        /// Gets or sets the search cs.
        /// </summary>
        public string SearchCs { get; set; }

        /// <summary>
        /// Gets or sets the calendar.
        /// </summary>
        public Calendar Calendar { get; set; }

        /// <summary>
        /// Gets or sets the calendars.
        /// </summary>
        public IEnumerable<CalendarOverview> Calendars { get; set; }
        public bool UserHasCalendarAdminPermission { get; set; }
    }

    /// <summary>
    /// The calendar input view model.
    /// </summary>
    public class CalendarInputViewModel
    {
        /// <summary>
        /// Gets or sets the calendar.
        /// </summary>
        public CalendarOverview Calendar { get; set; }

        /// <summary>
        /// Gets or sets the work groups available.
        /// </summary>
        public IList<SelectListItem> WGsAvailable { get; set; }

        /// <summary>
        /// Gets or sets the work groups selected.
        /// </summary>
        public IList<SelectListItem> WGsSelected { get; set; }

        public CalendarInputViewModel() { }
        public bool UserHasCalendarAdminPermission { get; set; }

        public User CurrentUser { get; set; }
    }
}