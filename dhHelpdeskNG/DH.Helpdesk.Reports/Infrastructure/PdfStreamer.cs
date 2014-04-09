// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfStreamer.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the PdfStreamer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Reports.Streamers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using DH.Helpdesk.Reports.Infrastructure;

    /// <summary>
    /// The pdf streamer.
    /// </summary>
    /// <typeparam name="TPrintModel">
    /// </typeparam>
    /// <typeparam name="TMergeData">
    /// </typeparam>
    public sealed class PdfStreamer<TPrintModel, TMergeData> 
        where TPrintModel : class, new() 
        where TMergeData : IPdfMergeData, new()
    {
        /// <summary>
        /// The get stream.
        /// </summary>
        /// <param name="printModels">
        /// The cases.
        /// </param>
        /// <param name="templatePath">
        /// The template path.
        /// </param>
        /// <returns>
        /// The <see cref="MemoryStream"/>.
        /// </returns>
        public MemoryStream GetPdfStream(IEnumerable<TPrintModel> printModels, string templatePath)
        {
            var util = new PdfMergeStreamer();
            var pdfMemoryStream = new MemoryStream();

            var mergeData = new List<IPdfMergeData>();
            foreach (var printModel in printModels)
            {
                mergeData.Add((IPdfMergeData)Activator.CreateInstance(typeof(TMergeData), printModel));
            }
            util.FillPdf(templatePath, mergeData, pdfMemoryStream);
            return pdfMemoryStream;
        }

        /// <summary>
        /// The get stream.
        /// </summary>
        /// <param name="printModel">
        /// The print model.
        /// </param>
        /// <param name="templatePath">
        /// The template path.
        /// </param>
        /// <returns>
        /// The <see cref="MemoryStream"/>.
        /// </returns>
        public MemoryStream GetPdfStream(TPrintModel printModel, string templatePath)
        {
            return this.GetPdfStream(new[] { printModel }, templatePath);
        }
    }
}