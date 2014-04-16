//PrintPdfView.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the PrintPdfView type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.SelfService.Infrastructure.Print
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;
    using System.Xml;

    using iTextSharp.text;
    using iTextSharp.text.html;
    using iTextSharp.text.pdf;
    using iTextSharp.text.xml;

    /// <summary>
    /// The print view.
    /// </summary>
    internal sealed class PrintPdfView : IView, IViewEngine
    {
        /// <summary>
        /// The result.
        /// </summary>
        private readonly ViewEngineResult result;

        /// <summary>
        /// The header.
        /// </summary>
        private readonly ViewEngineResult header;

        /// <summary>
        /// The footer.
        /// </summary>
        private readonly ViewEngineResult footer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPdfView"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="header">
        /// The header.
        /// </param>
        /// <param name="footer">
        /// The footer.
        /// </param>
        public PrintPdfView(ViewEngineResult result, ViewEngineResult header, ViewEngineResult footer)
        {
            this.result = result;
            this.header = header;
            this.footer = footer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPdfView"/> class.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        public PrintPdfView(ViewEngineResult result) : this(result, null, null)
        {
            
        }

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="viewContext">
        /// The view context.
        /// </param>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            XmlParser parser;
            string source = this.RenderView(viewContext, this.result.View);
            using (var reader = this.GetXmlReader(source))
            {
                while (reader.Read() && (reader.NodeType != XmlNodeType.Element))
                {
                }
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "itext"))
                {
                    parser = new XmlParser();
                }
                else
                {
                    parser = new HtmlParser();
                }
            }
            var document = new Document();
            document.Open();
            PdfWriter instance = PdfWriter.GetInstance(document, viewContext.HttpContext.Response.OutputStream);

            string headerString = null;
            if (this.header != null)
            {
                headerString = this.RenderView(viewContext, this.header.View);
            }
            string footerString = null;
            if (this.footer != null)
            {
                footerString = this.RenderView(viewContext, this.footer.View);
            }

            instance.PageEvent = new PrintPdfEvents(headerString, footerString);
            instance.CloseStream = false;
            viewContext.HttpContext.Response.ContentType = "application/pdf";
            using (var reader2 = this.GetXmlReader(source))
            {
                parser.Go(document, reader2);
            }
            instance.Close();
        }

        /// <summary>
        /// The find partial view.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="partialViewName">
        /// The partial view name.
        /// </param>
        /// <param name="useCache">
        /// The use cache.
        /// </param>
        /// <returns>
        /// The <see cref="ViewEngineResult"/>.
        /// </returns>
        public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The find view.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="viewName">
        /// The view name.
        /// </param>
        /// <param name="masterName">
        /// The master name.
        /// </param>
        /// <param name="useCache">
        /// The use cache.
        /// </param>
        /// <returns>
        /// The <see cref="ViewEngineResult"/>.
        /// </returns>
        public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The release view.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="view">
        /// The view.
        /// </param>
        public void ReleaseView(ControllerContext controllerContext, IView view)
        {
            this.result.ViewEngine.ReleaseView(controllerContext, this.result.View);
        }

        /// <summary>
        /// The get xml reader.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <returns>
        /// The <see cref="XmlTextReader"/>.
        /// </returns>
        private XmlTextReader GetXmlReader(string source)
        {
            return new XmlTextReader(new MemoryStream(Encoding.UTF8.GetBytes(source))) { WhitespaceHandling = WhitespaceHandling.None };
        }

        /// <summary>
        /// The render view result.
        /// </summary>
        /// <param name="viewContext">
        /// The view Context.
        /// </param>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string RenderView(ViewContext viewContext, IView view)
        {
            if (view == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            TextWriter writer = new StringWriter(sb);
            view.Render(viewContext, writer);
            return sb.ToString();
        }
    }
}