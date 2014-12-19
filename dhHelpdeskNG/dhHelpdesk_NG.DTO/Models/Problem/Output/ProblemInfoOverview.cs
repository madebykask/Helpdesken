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

        public string CustomerName { get; set; }
    }
}