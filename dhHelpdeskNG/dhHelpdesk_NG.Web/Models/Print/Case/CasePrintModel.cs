// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CasePrintModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CasePrintModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Models.Print.Case
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;

    /// <summary>
    /// The case print model.
    /// </summary>
    public sealed class CasePrintModel
    {
        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the case.
        /// </summary>
        public CaseOverview Case { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is reported by visible.
        /// </summary>
        public bool IsReportedByVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons name visible.
        /// </summary>
        public bool IsPersonsNameVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons email visible.
        /// </summary>
        public bool IsPersonsEmailVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons phone visible.
        /// </summary>
        public bool IsPersonsPhoneVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is persons cell phone visible.
        /// </summary>
        public bool IsPersonsCellPhoneVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is region visible.
        /// </summary>
        public bool IsRegionVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is department visible.
        /// </summary>
        public bool IsDepartmentVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is field visible.
        /// </summary>
        public bool IsOuVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is place visible.
        /// </summary>
        public bool IsPlaceVisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is user code visible.
        /// </summary>
        public bool IsUserCodeVisible { get; set; }        
    }
}