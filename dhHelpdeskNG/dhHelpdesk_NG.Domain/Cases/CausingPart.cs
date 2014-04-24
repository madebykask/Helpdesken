// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPart.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPart type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Domain.Cases
{
    using global::System.Collections.Generic;

    /// <summary>
    /// The causing type.
    /// </summary>
    public class CausingPart : Entity
    {
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
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public virtual CausingPart Parent { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        public virtual ICollection<CausingPart> Children { get; set; }
    }
}