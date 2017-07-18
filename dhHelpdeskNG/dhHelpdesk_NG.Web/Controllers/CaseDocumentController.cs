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


        public  byte[] GeneratePdf(CaseDocumentModel model, string baseUrl, string footerText, string draftText, string headerText)
        {
            // Create the PDF document where to add the HTML documents
            Winnovative.Document pdfDocument = new Winnovative.Document();
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            //TODO: move this to Database
            pdfDocument.LicenseKey = "xUtbSltKWkpbW0RaSllbRFtYRFNTU1M=";

            byte[] outPdfBuffer;

            try
            {
                Winnovative.PdfPage contractPdfPage = null;
                //WinnovativePointF contractHtmlLocation = Point.Empty;

                contractPdfPage = pdfDocument.AddPage();
                contractPdfPage.Margins.Left = model.CaseDocumentTemplate.MarginLeft;
                contractPdfPage.Margins.Right = model.CaseDocumentTemplate.MarginRight;
                contractPdfPage.Margins.Top = model.CaseDocumentTemplate.MarginTop;
                contractPdfPage.Margins.Bottom = model.CaseDocumentTemplate.MarginBottom;

                // contractHtmlLocation = PointF.Empty;
                var contractHtmlToConvert = RenderRazorViewToString("~/Views/Shared/_CaseDocumentPrint.cshtml", model, false);

                // Create the contract HTML to PDF element
                HtmlToPdfElement contractHtml = new HtmlToPdfElement(0, 0, contractHtmlToConvert, baseUrl);
                contractHtml.FitWidth = false;
                //TODO: Move this to Database
                contractHtml.HtmlViewerWidth = 793;
                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                contractHtml.ConversionDelay = 0;

                // Add the HTML to PDF document
                contractPdfPage.AddElement(contractHtml);

                if (!string.IsNullOrEmpty(footerText))
                { 
                    //// Enable footer in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;
                    
                    pdfDocument.AddFooterTemplate(model.CaseDocumentTemplate.FooterHeight);
                    AddFooter(model, pdfDocument, baseUrl, footerText);
                }

                if (!string.IsNullOrEmpty(headerText))
                {
                    //// Enable header in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                    pdfDocument.AddHeaderTemplate(model.CaseDocumentTemplate.HeaderHeight);
                    AddHeader(model, pdfDocument, baseUrl, headerText);
                }

                if (!string.IsNullOrEmpty(draftText))
                { 
                    // Get the stamp width and height
                    float stampWidth = pdfDocument.Pages[0].ClientRectangle.Width; // float.Parse("600");
                    float stampHeight = float.Parse("600");

                    // Center the stamp at the top of PDF page
                    //TODO: Move this to Database
                    float stampXLocation = (pdfDocument.Pages[0].ClientRectangle.Width - stampWidth) / 2;
                    float stampYLocation = 200;

                    RectangleF stampRectangle = new RectangleF(stampXLocation, stampYLocation, stampWidth, stampHeight);

                    // Create the stamp template to be repeated in each PDF page
                    Winnovative.Template stampTemplate = pdfDocument.AddTemplate(stampRectangle);

                    var stamp = draftText; 

                    // Create the HTML element to add in stamp template
                    HtmlToPdfElement stampHtmlElement = new HtmlToPdfElement(0, 0, stamp, baseUrl);

                    stampHtmlElement.Rotate(-20);
                    
                    // Set the HTML viewer width for the HTML added in stamp
                    stampHtmlElement.HtmlViewerWidth = 793;
                    // Fit the HTML content in stamp template
                    stampHtmlElement.FitWidth = true;
                    stampHtmlElement.FitHeight = true;

                     // Add HTML to stamp template
                     stampTemplate.AddElement(stampHtmlElement);
                }


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
            }

            return outPdfBuffer;
        }


        private void AddHeader(CaseDocumentModel model, Winnovative.Document pdfDocument, string baseURL, string headerText)
        {

            var headerHtml = headerText;

            // Create the HTML element to add in stamp template
            HtmlToPdfVariableElement headerHtmlElement = new HtmlToPdfVariableElement(0,0,headerHtml, baseURL);

            headerHtmlElement.WebFontsEnabled = true;

            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfHeaderOptions.AddElement(headerHtmlElement);

            // Add variable HTML element with page numbering to Header
            pdfDocument.Header.AddElement(headerHtmlElement);
        }

        private void AddFooter(CaseDocumentModel model, Winnovative.Document pdfDocument, string baseURL, string footerText)
        {
            var footerHtml = footerText;

            // Create the HTML element to add in stamp template
            var y = pdfDocument.Pages[0].Margins.Bottom - model.CaseDocumentTemplate.FooterHeight;
            HtmlToPdfVariableElement footerHtmlElement = new HtmlToPdfVariableElement(0, 0, footerHtml, baseURL);

            footerHtmlElement.WebFontsEnabled = true;
            
            //footerHtmlElement.FitWidth = true;
          // footerHtmlElement.FitHeight = true;

         
            PdfConverter pdfConverter = new PdfConverter();
            pdfConverter.PdfFooterOptions.AddElement(footerHtmlElement);

            // Add variable HTML element with page numbering to footer
            pdfDocument.Footer.AddElement(footerHtmlElement);
        }



        public ActionResult CaseDocumentGet(Guid caseDocumentGUID, int caseId)
        {

            //TODO: Check if user have access to this case?

            string footerText = "";
            CaseDocumentModel m = _caseDocumentService.GetCaseDocument(caseDocumentGUID, caseId);
            try
            {
                footerText = m.CaseDocumentParagraphs.Where(x => x.CaseDocumentParagraph.ParagraphType == 5).FirstOrDefault().CaseDocumentParagraph.CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {
               
            }

            string draftText = "";
             try
            {
                draftText = m.CaseDocumentParagraphs.Where(x => x.CaseDocumentParagraph.ParagraphType == 6).FirstOrDefault().CaseDocumentParagraph.CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {

            }

            string headerText = "";
            try
            {
                headerText = m.CaseDocumentParagraphs.Where(x => x.CaseDocumentParagraph.ParagraphType == 7).FirstOrDefault().CaseDocumentParagraph.CaseDocumentTexts.FirstOrDefault().Text;
            }
            catch (Exception)
            {

            }

            //// Get the base URL
            string baseUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;

            byte[] outPdfBuffer = GeneratePdf(m,baseUrl, footerText, draftText, headerText).ToArray();

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
