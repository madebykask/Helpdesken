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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Domain;

    /// <summary>
    /// The calendar overview.
    /// </summary>
    public sealed class CalendarOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customer_ id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the calendar date.
        /// </summary>
        [DataType(DataType.Date)] 
        public DateTime CalendarDate { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        [Required]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }

        /// <summary>
        /// Gets or sets the show until date.
        /// </summary>
        [DataType(DataType.Date)]
        public DateTime ShowUntilDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether public information.
        /// </summary>
        public bool PublicInformation { get; set; }

        /// <summary>
        /// Gets or sets the changed by user id.
        /// </summary>
        public int ChangedByUserId { get; set; }

        /// <summary>
        /// Gets or sets the changed date.
        /// </summary>
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the working groups.
        /// </summary>
        public ICollection<WorkingGroupEntity> WGs { get; set; }
    }
}