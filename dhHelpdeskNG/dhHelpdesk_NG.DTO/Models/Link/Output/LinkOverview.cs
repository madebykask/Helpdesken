// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the LinkOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Link.Output
{
    /// <summary>
    /// The link overview.
    /// </summary>
    public sealed class LinkOverview
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int? CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the url name.
        /// </summary>
        public string UrlName { get; set; }

        /// <summary>
        /// Gets or sets the url address.
        /// </summary>
        public string UrlAddress { get; set; }

        /// <summary>
        /// Gets or sets the link group id.
        /// </summary>
        public int? LinkGroupId { get; set; }

        /// <summary>
        /// Gets or sets the link group name.
        /// </summary>
        public string LinkGroupName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether show on start page.
        /// </summary>
        public bool ShowOnStartPage { get; set; }

        public string SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the casesolution id.
        /// </summary>
        public int? CaseSolutionId { get; set; }

        /// <summary>
        /// Gets or sets Casesolution name.
        /// </summary>
        public string CaseSolutionName { get; set; }

        /// <summary>
        /// Gets or sets the document id.
        /// </summary>
        public int? DocumentId { get; set; }

        /// <summary>
        /// Gets or sets document name.
        /// </summary>
        public string DocumentName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it should open in new window or not.
        /// </summary>
        public bool OpenInNewWindow { get; set; }

        /// <summary>
        /// Gets or sets a Window Height
        /// </summary>
        public int NewWindowHeight { get; set; }

        /// <summary>
        /// Gets or sets a Window Width
        /// </summary>
        public int NewWindowWidth { get; set; }
    }
}