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
    /// <summary>
    /// The causing type.
    /// </summary>
    public sealed class CausingPart : Entity
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
    }
}