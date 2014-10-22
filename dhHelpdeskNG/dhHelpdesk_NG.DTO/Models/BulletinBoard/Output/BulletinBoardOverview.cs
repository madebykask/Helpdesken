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
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }
    }
}