// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CalendarOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CalendarOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace DH.Helpdesk.BusinessData.Models.Calendar.Output
{
    using System;

    /// <summary>
    /// The calendar overview.
    /// </summary>
    public sealed class CalendarOverview
    {
        /// <summary>
        /// Gets or sets the customer_ id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the calendar date.
        /// </summary>
        public DateTime CalendarDate { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }

        /// <summary>
        /// Gets or sets the show until date.
        /// </summary>
        public DateTime ShowUntilDate { get; set; }
    }
}