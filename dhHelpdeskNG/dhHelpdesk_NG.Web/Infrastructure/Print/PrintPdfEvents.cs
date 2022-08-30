// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrintPdfEvents.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the PrintPdfEvents type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Web.Infrastructure.Print
{
    using System.Collections;
    using System.IO;

    using iTextSharp.text;
    using iTextSharp.text.html.simpleparser;
    using iTextSharp.text.pdf;

    /// <summary>
    /// The print events.
    /// </summary>
    public class PrintPdfEvents : PdfPageEventHelper
    {
        /// <summary>
        /// The header.
        /// </summary>
        private readonly string header;

        /// <summary>
        /// The footer.
        /// </summary>
        private readonly string footer;

        /// <summary>
        /// The header elements.
        /// </summary>
        /// 
#pragma warning disable 0618
        private readonly ArrayList headerElements;
#pragma warning restore 0618

        /// <summary>
        /// The footer elements.
        /// </summary>
        private readonly ArrayList footerElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPdfEvents"/> class.
        /// </summary>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="footer">
        /// The footer.
        /// </param>
        public PrintPdfEvents(string header, string footer)
        {
            this.header = header;
            this.footer = footer;
        }

        /// <summary>
        /// The on open document.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="document">
        /// The document.
        /// </param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            if (!string.IsNullOrEmpty(this.header))
            {
                using (var sr = new StringReader(this.header))
                {
                    //this.headerElements = HTMLWorker.ParseToList(sr, new StyleSheet());
                }                
            }
            if (!string.IsNullOrEmpty(this.footer))
            {
                using (var sr = new StringReader(this.footer))
                {
                    //this.footerElements = HTMLWorker.ParseToList(sr, new StyleSheet());
                }                
            }
            base.OnOpenDocument(writer, document);
        }

        /// <summary>
        /// The on start page.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="document">
        /// The document.
        /// </param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            if (this.headerElements != null)
            {
                foreach (var element in this.headerElements)
                {
                    document.Add((IElement)element);
                }
            }
            base.OnStartPage(writer, document);
        }

        /// <summary>
        /// The on end page.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="document">
        /// The document.
        /// </param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (this.footerElements != null)
            {
                foreach (var element in this.footerElements)
                {
                    document.Add((IElement)element);
                }
            }
            base.OnEndPage(writer, document);
        }
    }
}