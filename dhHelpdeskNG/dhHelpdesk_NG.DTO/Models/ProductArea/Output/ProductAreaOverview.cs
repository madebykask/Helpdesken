// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductAreaOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProductAreaOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseHistory;

namespace DH.Helpdesk.BusinessData.Models.ProductArea.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Interfaces;

    /// <summary>
    /// The product area overview.
    /// </summary>
    public sealed class ProductAreaOverview : IHierarchyItem
    {
        public ProductAreaOverview()
        {
            WorkingGroups = new List<WorkingGroupOverview>();
            SubProductAreas = new List<ProductAreaOverview>();
        }
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

        public int CustomerId { get; set; }

        public int IsActive { get; set; }

        public List<WorkingGroupOverview> WorkingGroups { get; set; }

        public List<ProductAreaOverview> SubProductAreas { get; set; }
    }
}