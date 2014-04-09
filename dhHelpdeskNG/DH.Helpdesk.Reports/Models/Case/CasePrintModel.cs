// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CasePrintModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CasePrintModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Reports.Models.Case
{
    using System;

    /// <summary>
    /// The case model.
    /// </summary>
    public sealed class CasePrintModel
    {
        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        public decimal Number { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        public DateTime RegistrationDate { get; set; }
    }
}