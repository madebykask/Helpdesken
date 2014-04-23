// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Case.Output
{
    using DH.Helpdesk.BusinessData.Interfaces;

    /// <summary>
    /// The causing type overview.
    /// </summary>
    public sealed class CausingPartOverview : IHierarchyItem
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

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}