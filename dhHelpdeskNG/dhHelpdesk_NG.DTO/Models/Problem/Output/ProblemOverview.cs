// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProblemOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Problem.Output
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    /// <summary>
    /// The problem overview.
    /// </summary>
    public class ProblemOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [IsId]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the problem number.
        /// </summary>
        public int ProblemNumber { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the responsible user name.
        /// </summary>
        public string ResponsibleUserName { get; set; }

        public string ResponsibleUserSurName { get; set; }

        /// <summary>
        /// Gets or sets the finishing date.
        /// </summary>
        public DateTime? FinishingDate { get; set; }

        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Gets or sets the responsible user id.
        /// </summary>
        public int? ResponsibleUserId { get; set; }

        /// <summary>
        /// Gets or sets the inventory number.
        /// </summary>
        public string InventoryNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is exist connected cases.
        /// </summary>
        public bool IsExistConnectedCases { get; set; }
    }
}
