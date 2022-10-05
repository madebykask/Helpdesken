// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemLogOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProblemLogOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Problem.Output
{
    using System;

    /// <summary>
    /// The problem log overview.
    /// </summary>
    public class ProblemLogOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the problem id.
        /// </summary>
        public int ProblemId { get; set; }

        public int FinnishConnectedCases { get; set; }

        /// <summary>
        /// Gets or sets the changed date.
        /// </summary>
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the changed by user name.
        /// </summary>
        public string ChangedByUserName { get; set; }

        public string ChangedByUserSurName { get; set; }

        /// <summary>
        /// Gets or sets the log text.
        /// </summary>
        public string LogText { get; set; }

        /// <summary>
        /// Gets or sets the show on case.
        /// </summary>
        public int ShowOnCase { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// The is show on case.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsShowOnCase()
        {
            return this.ShowOnCase > 0;
        }

        /// <summary>
        /// The is internal log.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsInternal()
        {
            return this.ShowOnCase == 1;
        }

        /// <summary>
        /// The is external log.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsExternal()
        {
            return this.ShowOnCase == 2;
        }
    }
}