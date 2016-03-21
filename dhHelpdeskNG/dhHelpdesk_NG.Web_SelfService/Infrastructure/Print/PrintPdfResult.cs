//PrintPdfResult.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the PrintPdfResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.NewSelfService.Infrastructure.Print
{
    using System.Web.Mvc;

    /// <summary>
    /// The print result.
    /// </summary>
    internal sealed class PrintPdfResult : ViewResult
    {
        /// <summary>
        /// The header.
        /// </summary>
        private readonly ViewResult header;

        /// <summary>
        /// The footer.
        /// </summary>
        private readonly ViewResult footer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPdfResult"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="header">
        /// The header url.
        /// </param>
        /// <param name="footer">
        /// The footer url.
        /// </param>
        public PrintPdfResult(
            object model, 
            string name,
            ViewResult header,
            ViewResult footer)
        {
            this.header = header;
            this.footer = footer;
            this.ViewData = new ViewDataDictionary(model);
            this.ViewName = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintPdfResult"/> class.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public PrintPdfResult(object model, string name) : this(model, name, null, null)
        {
            
        }

        /// <summary>
        /// The find view.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="ViewEngineResult"/>.
        /// </returns>
        protected override ViewEngineResult FindView(ControllerContext context)
        {
            ViewEngineResult result = base.FindView(context);
            if (result.View == null)
            {
                return result;
            }

            ViewEngineResult headerResult = null;
            if (this.header != null)
            {
                headerResult = this.header.ViewEngineCollection.FindView(context, this.header.ViewName, this.header.MasterName);
            }

            ViewEngineResult footerResult = null;
            if (this.footer != null)
            {
                footerResult = this.footer.ViewEngineCollection.FindView(context, this.footer.ViewName, this.footer.MasterName);
            }

            var view = new PrintPdfView(result, headerResult, footerResult);
            return new ViewEngineResult(view, view);
        }
    }
}