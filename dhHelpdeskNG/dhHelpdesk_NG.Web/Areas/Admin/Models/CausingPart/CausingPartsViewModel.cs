// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartsViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartsViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Areas.Admin.Models.CausingPart
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The causing parts view model.
    /// </summary>
    public sealed class CausingPartsViewModel
    {
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the causing parts.
        /// </summary>
        public IEnumerable<CausingPartOverview> CausingParts { get; set; }
    }
}