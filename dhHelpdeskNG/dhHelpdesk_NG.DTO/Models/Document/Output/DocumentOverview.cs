// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentOverview.cs" company="">
//   
// </copyright>
// <summary>
//   The document overview.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Document.Output
{
    using System;

    /// <summary>
    /// The document overview.
    /// </summary>
    public sealed class DocumentOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the changes date.
        /// </summary>
        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }
    }
}