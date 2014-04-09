// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdfMergeStreamer.cs" company="">
//   http://www.codeproject.com/Articles/691723/Csharp-Generate-and-Deliver-PDF-Files-On-Demand-fr
// </copyright>
// <summary>
//   Defines the PdfMergeStreamer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Reports.Infrastructure
{ 
    using System.Collections.Generic;
    using System.IO;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    /// <summary>
    /// The document merge streamer.
    /// </summary>
    internal sealed class PdfMergeStreamer
    {
        /// <summary>
        /// The fill document.
        /// </summary>
        /// <param name="templatePath">
        /// The template path.
        /// </param>
        /// <param name="mergeDataItems">
        /// The merge data items.
        /// </param>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        public void FillPdf(
            string templatePath,
            IEnumerable<IPdfMergeData> mergeDataItems,
            MemoryStream outputStream)
        {
            // Agggregate successive pages here:
            var pagesAll = new List<byte[]>();

            // Hold individual pages Here:
            byte[] pageBytes = null;

            foreach (var mergeItem in mergeDataItems)
            {
                // Read the form template for each item to be output:
                var templateReader = new PdfReader(templatePath);
                using (var tempStream = new MemoryStream())
                {
                    var stamper = new PdfStamper(templateReader, tempStream);
                    stamper.FormFlattening = false;
                    AcroFields fields = stamper.AcroFields;
                    stamper.Writer.CloseStream = false;

                    // Grab a reference to the Dictionary in the current merge item:
                    var fieldVals = mergeItem.Fields;

                    // Walk the Dictionary keys, fnid teh matching AcroField, 
                    // and set the value:
                    foreach (string name in fieldVals.Keys)
                    {
                        fields.SetField(name, fieldVals[name]);
                    }

                    // If we had not set the CloseStream property to false, 
                    // this line would also kill our memory stream:
                    stamper.Close();

                    // Reset the stream position to the beginning before reading:
                    tempStream.Position = 0;

                    // Grab the byte array from the temp stream . . .
                    pageBytes = tempStream.ToArray();

                    // And add it to our array of all the pages:
                    pagesAll.Add(pageBytes);
                }
            }

            // Create a document container to assemble our pieces in:
            var mainDocument = new Document(PageSize.A4);

            // Copy the contents of our document to our output stream:
            var pdfCopier = new PdfSmartCopy(mainDocument, outputStream);

            // Once again, don't close the stream when we close the document:
            pdfCopier.CloseStream = false;

            mainDocument.Open();
            foreach (var pageByteArray in pagesAll)
            {
                // Copy each page into the document:
                mainDocument.NewPage();
                pdfCopier.AddPage(pdfCopier.GetImportedPage(new PdfReader(pageByteArray), 1));
            }
            pdfCopier.Close();

            // Set stream position to the beginning before returning:
            outputStream.Position = 0;
        }
    }
}