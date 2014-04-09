// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPdfMergeData.cs" company="">
//   
// </copyright>
// <summary>
//   The PdfMergeData interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Reports.Infrastructure
{
    using System.Collections.Generic;

    /// <summary>
    /// The MergeData interface.
    /// </summary>
    public interface IPdfMergeData
    {
        /// <summary>
        /// Gets the merge field values.
        /// </summary>
        IDictionary<string, string> Fields { get; }
    }
}