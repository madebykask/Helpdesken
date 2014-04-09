// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CaseMergeData.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CaseMergeData type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Reports.Merge
{
    using System.Collections.Generic;
    using System.Globalization;

    using DH.Helpdesk.Reports.Infrastructure;
    using DH.Helpdesk.Reports.Models.Case;

    /// <summary>
    /// The case merge data.
    /// </summary>
    public sealed class CaseMergeData : IPdfMergeData
    {
        /// <summary>
        /// The case model.
        /// </summary>
        private readonly CasePrintModel casePrintModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaseMergeData"/> class.
        /// </summary>
        /// <param name="casePrintModel">
        /// The case model.
        /// </param>
        public CaseMergeData(CasePrintModel casePrintModel)
        {
            this.casePrintModel = casePrintModel;
        }

        public CaseMergeData()
        {
            
        }

        /// <summary>
        /// Gets the merge field values.
        /// </summary>
        public IDictionary<string, string> Fields
        {
            get
            {
                var output = new Dictionary<string, string>();
                output.Add("Number", this.casePrintModel.Number.ToString(CultureInfo.InvariantCulture));
                output.Add("RegistrationDate", this.casePrintModel.RegistrationDate.ToString(CultureInfo.InvariantCulture));
                return output;
            }
        }
    }
}