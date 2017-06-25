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
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.BusinessData.Interfaces;
    using DH.Helpdesk.Common.ValidationAttributes;
    using System;

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
        [Required(ErrorMessage = "Causing Part is required.")]
        [DH.Helpdesk.Common.ValidationAttributes.MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DH.Helpdesk.Common.ValidationAttributes.MaxLength(300)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public CausingPartOverview Parent { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public IEnumerable<CausingPartOverview> Children { get; set; }
    }
}