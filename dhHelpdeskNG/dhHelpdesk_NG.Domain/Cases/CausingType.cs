// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingType.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingType type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Domain.Cases
{
    /// <summary>
    /// The causing type.
    /// </summary>
    public sealed class CausingType : Entity
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
        public bool IsActive { get; set; }
    }
}