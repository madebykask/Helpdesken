// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListItem.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ListItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    /// <summary>
    /// The list item.
    /// </summary>
    internal class ListItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }
    }
}