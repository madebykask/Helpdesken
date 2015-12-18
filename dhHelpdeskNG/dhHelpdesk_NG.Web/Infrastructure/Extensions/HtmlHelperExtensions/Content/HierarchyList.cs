// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HierarchyList.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the HierarchyList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
namespace DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content
{
    /// <summary>
    /// The hierarchy list.
    /// </summary>
    internal sealed class HierarchyList
    {
        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        public ListGroup[] Groups { get; set; }

        public static HierarchyList GetEmpty()
        {
            var ret = new HierarchyList();
            var listItems = new List<ListItem>();
            listItems.Add(new ListItem()
            {
                Id = 0,
                Name = string.Empty
            });

            var emptyListGroups = new List<ListGroup>();
            var listGroup = new ListGroup();
            listGroup.Items = listItems.ToArray();
            emptyListGroups.Add(listGroup);
            ret.Groups = emptyListGroups.ToArray();
            return ret;
        }
    }
}