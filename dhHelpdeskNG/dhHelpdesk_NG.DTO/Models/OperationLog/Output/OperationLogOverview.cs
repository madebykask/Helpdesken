// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationLogOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the OperationLogOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.OperationLog.Output
{
    using System;

    /// <summary>
    /// The operation log overview.
    /// </summary>
    public sealed class OperationLogOverview
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the changed date.
        /// </summary>
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the log text.
        /// </summary>
        public string LogText { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public OperationLogCategoryOverview Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }
    }
}