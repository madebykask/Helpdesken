// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchyListExtension.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the HierarchyListExtension type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions
{
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// The hierarchy list extension.
    /// </summary>
    public static class HierarchyListExtension
    {
        /// <summary>
        /// The product area list.
        /// </summary>
        /// <param name="html">
        /// The html.
        /// </param>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <param name="productArea">
        /// The product area.
        /// </param>
        /// <param name="modelId">
        /// The model id.
        /// </param>
        /// <returns>
        /// The <see cref="MvcHtmlString"/>.
        /// </returns>
        public static MvcHtmlString ProductAreaList(this HtmlHelper html, int customerId, int? productArea, string modelId)
        {
            var result = new StringBuilder();
            var tag = new TagBuilder("input");
            tag.MergeAttribute("type", "hidden");
            tag.MergeAttribute("data-hierarchylist-list", null);
            tag.MergeAttribute("data-hierarchylist-list-selectedvalue", productArea.HasValue ? productArea.Value.ToString(CultureInfo.InvariantCulture) : null);
            tag.MergeAttribute(
                "data-hierarchylist-list-getitemsurl",
                string.Format("/Ajax/ProductArea?customerId={0}", customerId));
            tag.MergeAttribute("data-hierarchylist-list-modelid", modelId);
            result.Append(tag);
            return MvcHtmlString.Create(result.ToString());
        }
    }
}