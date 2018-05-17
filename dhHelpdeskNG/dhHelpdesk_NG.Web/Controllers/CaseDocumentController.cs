using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using Winnovative;

namespace DH.Helpdesk.Web.Controllers
{
    using DH.Helpdesk.BusinessData.Models.CaseDocument;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions;
    using Infrastructure.Attributes;

    public class CaseDocumentController : BaseController
    {
        private readonly IMasterDataService _masterDataService;
        private readonly ICaseDocumentService _caseDocumentService;

        #region ***Constructor***

        public CaseDocumentController(
            IMasterDataService masterDataService, ICaseDocumentService caseDocumentService)
            : base(masterDataService)
        {
            this._masterDataService = masterDataService;
            this._caseDocumentService = caseDocumentService;
        }

        #endregion

        private CaseDocumentModel _model;
        private string _headerAlternativeText;
        private string _footerAlternativeText;
        private string _baseUrl;

        private float _footerHeight;
        private float _headerHeight;


        public byte[] GeneratePdf(CaseDocumentModel model, string baseUrl, string footerText, string draftText, string headerText, string headerAlternativeText, string footerAlternativeText)
        {
            // Create the PDF document where to add the HTML documents
            Winnovative.Document pdfDocument = new Winnovative.Document();

            //TODO: move this to Database
            pdfDocument.LicenseKey = "xUtbSltKWkpbW0RaSllbRFtYRFNTU1M=";

            HtmlToPdfElement contractHtml = null;

            byte[] outPdfBuffer;

            try
            {
                Winnovative.PdfPage contractPdfPage = null;

                contractPdfPage = pdfDocument.AddPage();
                contractPdfPage.Margins.Left = Convert.ToSingle(model.CaseDocumentTemplate.MarginLeft);
                contractPdfPage.Margins.Right = Convert.ToSingle(model.CaseDocumentTemplate.MarginRight);
                contractPdfPage.Margins.Top = Convert.ToSingle(model.CaseDocumentTemplate.MarginTop);
                contractPdfPage.Margins.Bottom = Convert.ToSingle(model.CaseDocumentTemplate.MarginBottom);

                _model = model;
                _headerAlternativeText = headerAlternativeText;
                _footerAlternativeText = footerAlternativeText;
                _baseUrl = baseUrl;

                _footerHeight = Convert.ToSingle(_model.CaseDocumentTemplate.FooterHeight);
                _headerHeight = Convert.ToSingle(_model.CaseDocumentTemplate.HeaderHeight);

                if (!string.IsNullOrEmpty(footerText))
                {
                    pdfDocument.AddFooterTemplate(Convert.ToSingle(model.CaseDocumentTemplate.FooterHeight));
                    AddFooter(model, pdfDocument, baseUrl, footerText);
                }

                if (!string.IsNullOrEmpty(headerText))
                {
                    pdfDocument.AddHeaderTemplate(Convert.ToSingle(model.CaseDocumentTemplate.HeaderHeight));
                    AddHeader(model, pdfDocument, baseUrl, headerText);
                }

                if (!string.IsNullOrEmpty(draftText))
                {
                    // Get the draft width and height
                    float draftWidth = pdfDocument.Pages[0].ClientRectangle.Width;
                    float draftHeight = Convert.ToSingle(model.CaseDocumentTemplate.DraftHeight);

                    // Center the draft at the top of PDF page based on PDF width
                    float draftXLocation = (pdfDocument.Pages[0].ClientRectangle.Width - draftWidth) / 2;
                    float draftYLocation = Convert.ToSingle(model.CaseDocumentTemplate.DraftYLocation);

                    RectangleF draftRectangle = new RectangleF(draftXLocation, draftYLocation, draftWidth, draftHeight);
                    // Create the draft template to be repeated in each PDF page
                    Winnovative.Template draftTemplate = pdfDocument.AddTemplate(draftRectangle);
                    
                    // Create the HTML element to add in draft template
                    HtmlToPdfElement draftHtmlElement = new HtmlToPdfElement(0, 0, draftText, baseUrl);
                    draftHtmlElement.Rotate(Convert.ToSingle(model.CaseDocumentTemplate.DraftRotateAngle));

                    // Set the HTML viewer width for the HTML added in draft
                    draftHtmlElement.HtmlViewerWidth = model.CaseDocumentTemplate.HtmlViewerWidth;
                    // Fit the HTML content in draft template
                    draftHtmlElement.FitWidth = true;
                    draftHtmlElement.FitHeight = true;

                    // Add HTML to draft template
                    draftTemplate.AddElement(draftHtmlElement);
                }

                var contractHtmlToConvert = RenderRazorViewToString("~/Views/Shared/_CaseDocumentPrint.cshtml", model, false);

                // Create the contract HTML to PDF element
                contractHtml = new HtmlToPdfElement(0, 0, contractHtmlToConvert, baseUrl);
                contractHtml.FitWidth = false;
                contractHtml.HtmlViewerWidth = model.CaseDocumentTemplate.HtmlViewerWidth; ;
                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                contractHtml.ConversionDelay = 0;
                // Install a handler where to change the header and footer in pages generated by the HTML to PDF element
                contractHtml.PrepareRenderPdfPageEvent += new PrepareRenderPdfPageDelegate(htmlToPdfElement_PrepareRenderPdfPageEvent);

                // Add the HTML to PDF document
                contractPdfPage.AddElement(contractHtml);
                pdfDocument.CompressionLevel = PdfCompressionLevel.NoCompression;

                // Save the PDF document in a memory buffer
                outPdfBuffer = pdfDocument.Save();

                // Send the PDF as response to browser
                // Set response content type
                Response.AddHeader("Content-Type", "application/pdf");

                // Instruct the browser to open the PDF file as an attachment or inline
                Response.AddHeader("Content-Disposition", String.Format("inline; filename=" + model.Name + ".pdf; size={0}", outPdfBuffer.Length.ToString()));

                // Write the PDF document buffer to HTTP response
                Response.BinaryWrite(outPdfBuffer);

                // End the HTTP response and stop the current page processing
                Response.End();

            }
            finally
            {
                // Close the PDF document
                pdfDocument.Close();

                // uninstall handler
                contractHtml.PrepareRenderPdfPageEvent -= new PrepareRenderPdfPageDelegate(htmlToPdfElement_PrepareRenderPdfPageEvent);
            }

            return outPdfBuffer;
        }

        /// <summary>
        /// The handler for HtmlToPdfElement.PrepareRenderPdfPageEvent event where you can set the visibility of header and footer
        /// in each page or you can add a custom header or footer in a page
        /// </summary>
        /// <param name="eventParams">The event parameter containin the PDF page to customize before rendering</param>
        void htmlToPdfElement_PrepareRenderPdfPageEvent(PrepareRenderPdfPageParams eventParams)
        {
            // The PDF page being rendered
            PdfPage pdfPage = eventParams.Page;

     
            if (eventParams.PageNumber <= _model.CaseDocumentTemplate.ShowFooterFromPageNr)
            {
                // The default document footer will be replaced in this page
                pdfPage.AddFooterTemplate(_footerHeight);
                // Draw the page header elements
                DrawEmptyPageFooter(pdfPage.Footer);
            }

            if (eventParams.PageNumber <= _model.CaseDocumentTemplate.ShowHeaderFromPageNr)
            {
                // The default document header will be replaced in this page
                pdfPage.AddHeaderTemplate(_headerHeight);
                // Draw the page header elements
                DrawEmptyPageHeader(pdfPage.Header);
            }

            if (eventParams.PageNumber == 1 && _model.CaseDocumentTemplate.ShowAlternativeHeaderOnFirstPage && !string.IsNullOrEmpty(_headerAlternativeText))
            {
                // The default document header will be replaced in this page
                pdfPage.AddHeaderTemplate(_headerHeight);
                // Draw the page header elements
                DrawAlternativeHeaderOnFirstPage(pdfPage.Header, _headerAlternativeText, _baseUrl);
            }

            if (eventParams.PageNumber == 1 && _model.CaseDocumentTemplate.ShowAlternativeFooterOnFirstPage && !string.IsNullOrEmpty(_footerAlternativeText))
            {
                // The default document header will be replaced in this page
                pdfPage.AddFooterTemplate(_footerHeight);
                // Draw the page header elements
                DrawAlternativeHeaderOnFirstPage(pdfPage.Footer, _footerAlternativeText, _baseUrl);
            }
        }

        private void AddHeader(CaseDocumentModel model, Winnovative.Document pdfDocument, string baseURL, string headerText)
        {
            var headerHtml = headerText;

            // Create the HTML element to add in  template
            HtmlToPdfVariableElement headerHtmlElement = new HtmlToPdfVariableElement(0, 0, headerHtml, baseURL);

            headerHtmlElement.WebFontsEnabled = true;

            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfHeaderOptions.AddElement(headerHtmlElement);

            // Add variable HTML element with page numbering to Header
            pdfDocument.Header.AddElement(headerHtmlElement);
        }

        private void AddFooter(CaseDocumentModel model, Winnovative.Document pdfDocument, string baseURL, string footerText)
        {
            var footerHtml = footerText;
            HtmlToPdfVariableElement footerHtmlElement = new HtmlToPdfVariableElement(footerHtml, baseURL);
            footerHtmlElement.WebFontsEnabled = true;

            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfFooterOptions.AddElement(footerHtmlElement);
            // Add variable HTML element with page numbering to footer
            pdfDocument.Footer.AddElement(footerHtmlElement);
        }

        /// <summary>
        /// Draw an empty page footer elements
        /// </summary>
        private void DrawEmptyPageFooter(Template footerTemplate)
        {
            // Create a HTML element to be added in footer
            HtmlToPdfElement html = new HtmlToPdfElement("", "");

            // Add HTML element to footer
            footerTemplate.AddElement(html);
        }

        /// <summary>
        /// Draw an empty page footer elements
        /// </summary>
        private void DrawEmptyPageHeader(Template headerTemplate)
        {
            // Create a HTML element to be added in footer
            HtmlToPdfElement html = new HtmlToPdfElement("", "");

            // Add HTML element to header
            headerTemplate.AddElement(html);
        }

        /// <summary>
        /// Draw an alternative Page Header
        /// </summary>
        private void DrawAlternativeHeaderOnFirstPage(Template headerTemplate, string headerAlternativeText, string baseUrl)
        {
            // Create a HTML element to be added in footer
            HtmlToPdfVariableElement html = new HtmlToPdfVariableElement(0, 0, headerAlternativeText, baseUrl);

            // Add HTML element to header
            headerTemplate.AddElement(html);
        }

        private void DrawAlternativeFooterOnFirstPage(Template footerTemplate, string footerAlternativeText, string baseUrl)
        {
            // Create a HTML element to be added in footer
            HtmlToPdfVariableElement html = new HtmlToPdfVariableElement(0, 0, footerAlternativeText, baseUrl);

            // Add HTML element to header
            footerTemplate.AddElement(html);
        }


        [UserCasePermissions]
        public ActionResult CaseDocumentGet(Guid caseDocumentGUID, int id)
        {
            string footerText = "";
            var caseDoc = _caseDocumentService.GetCaseDocument(caseDocumentGUID, id);
            try
            {
                footerText = caseDoc.CaseDocumentParagraphs.Where(x => x.ParagraphType == 5).FirstOrDefault().CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {
            }

            string draftText = "";
            try
            {
                draftText = caseDoc.CaseDocumentParagraphs.Where(x => x.ParagraphType == 6).FirstOrDefault().CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {
            }

            string headerText = "";
            try
            {
                headerText = caseDoc.CaseDocumentParagraphs.Where(x => x.ParagraphType == 7).FirstOrDefault().CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {
            }

            string headerAlternativeText = "";
            try
            {
                headerAlternativeText = caseDoc.CaseDocumentParagraphs.Where(x => x.ParagraphType == 8).FirstOrDefault().CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {
            }

            string footerAlternativeText = "";
            try
            {
                footerAlternativeText = caseDoc.CaseDocumentParagraphs.Where(x => x.ParagraphType == 9).FirstOrDefault().CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {
            }

            //// Get the base URL
            string baseUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;
            byte[] outPdfBuffer = GeneratePdf(caseDoc, baseUrl, footerText, draftText, headerText, headerAlternativeText, footerAlternativeText).ToArray();
            return this.Pdf(outPdfBuffer);
        }

    }

    public class FormLibPdfContentResult : FileContentResult
    {
        public FormLibPdfContentResult(byte[] data) : base(data, "application/pdf") { }

        public FormLibPdfContentResult(byte[] data, string fileName)
            : this(data)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            this.FileDownloadName = fileName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ClearHeaders();

            base.ExecuteResult(context);

            context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.Private);
        }
    }
}
