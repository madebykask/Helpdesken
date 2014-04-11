// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupplierOverview.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the SupplierOverview type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.BusinessData.Models.Supplier.Output
{
    /// <summary>
    /// The supplier overview.
    /// </summary>
    public sealed class SupplierOverview
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the contact name.
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the supplier number.
        /// </summary>
        public string SupplierNumber { get; set; }
    }
}