namespace DH.Helpdesk.Web.Infrastructure.Print
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web.Mvc;

    using iTextSharp.text;
    using iTextSharp.text.html.simpleparser;
    using iTextSharp.text.pdf;

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

        static PrintPdfView()
        {
//            var fontpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
            var fontpath = AppDomain.CurrentDomain.BaseDirectory + "\\Content\\fonts\\ARIALUNI.TTF";
            if (!FontFactory.IsRegistered("ARIALUNI"))
            {
                FontFactory.Register(fontpath);
            }            
        }

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
            string source = this.RenderView(viewContext, this.result.View);
            var document = new Document();

            PdfWriter instance = PdfWriter.GetInstance(document, viewContext.HttpContext.Response.OutputStream);
            
            document.Open();

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

            var list = HTMLWorker.ParseToList(new StringReader(source), new StyleSheet());
            foreach (var element in list)
            {
                document.Add((IElement)element);
            }

            document.Close();

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