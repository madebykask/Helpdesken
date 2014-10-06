// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Areas.Admin.Models.CausingPart
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The causing part view model.
    /// </summary>
    public sealed class CausingPartViewModel
    {
        /// <summary>
        /// Gets or sets the customer.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the causing part.
        /// </summary>
        public CausingPartOverview CausingPart { get; set; }
    }
}