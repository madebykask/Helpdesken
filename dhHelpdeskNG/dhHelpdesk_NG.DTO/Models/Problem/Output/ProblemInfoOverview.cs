// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemInfoOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProblemInfoOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Problem.Output
{
    using System;

    /// <summary>
    /// The problem info overview.
    /// </summary>
    public class ProblemInfoOverview
    {
        /// <summary>
        /// Gets or sets the customer_ id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the problem number.
        /// </summary>
        public int ProblemNumber { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }
    }
}