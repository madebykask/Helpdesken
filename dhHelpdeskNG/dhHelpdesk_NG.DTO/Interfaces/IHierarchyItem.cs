// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchyItem.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IHierarchyItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Interfaces
{
    /// <summary>
    /// The HierarchyItem interface.
    /// </summary>
    public interface IHierarchyItem
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        string Description { get; set; } 
    }
}