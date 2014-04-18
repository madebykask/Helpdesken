// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BulletinBoardOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the BulletinBoardOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.BulletinBoard.Output
{
    using System;

    /// <summary>
    /// The bulletin board overview.
    /// </summary>
    public sealed class BulletinBoardOverview
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
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }
    }
}