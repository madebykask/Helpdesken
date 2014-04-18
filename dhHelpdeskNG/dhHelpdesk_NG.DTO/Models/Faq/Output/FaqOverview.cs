// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaqOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the FaqOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    /// <summary>
    /// The overview.
    /// </summary>
    public class FaqOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [IsId]
        public int Id { get; set; }

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
