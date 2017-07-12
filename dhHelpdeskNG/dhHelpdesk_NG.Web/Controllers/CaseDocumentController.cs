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


        public  byte[] GeneratePdf(CaseDocumentModel model, string baseUrl)
        {
            // Create the PDF document where to add the HTML documents
            Winnovative.Document pdfDocument = new Winnovative.Document();
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            pdfDocument.LicenseKey = "xUtbSltKWkpbW0RaSllbRFtYRFNTU1M=";

            byte[] outPdfBuffer;

            try
            {


                Winnovative.PdfPage contractPdfPage = null;
                //WinnovativePointF contractHtmlLocation = Point.Empty;

                contractPdfPage = pdfDocument.AddPage();
                //contractPdfPage.Margins.Left = coP.MarginLeft;
                //contractPdfPage.Margins.Right = coP.MarginRight;
                //TODO: FROM DB
                 contractPdfPage.Margins.Top = 30;
                // contractPdfPage.Margins.Bottom = coP.MarginBottom;

                // contractHtmlLocation = PointF.Empty;

                //var contractHtmlToConvert = "<div>Hej</div>"; // RenderRazorViewToString(coP.ViewName, model, false);

                var contractHtmlToConvert = RenderRazorViewToString("~/Views/Shared/_CaseDocumentPrint.cshtml", model, false);

                // Create the contract HTML to PDF element
                HtmlToPdfElement contractHtml = new HtmlToPdfElement(0, 0, contractHtmlToConvert, baseUrl);
                contractHtml.FitWidth = false;
                contractHtml.HtmlViewerWidth = 793;
                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                contractHtml.ConversionDelay = 0;

                // Add the HTML to PDF document
                contractPdfPage.AddElement(contractHtml);



                //// Enable footer in the generated PDF document
                //htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;

                //pdfDocument.AddFooterTemplate(60);

                //AddFooter(pdfDocument, baseUrl);

                ////// Enable header in the generated PDF document
                //htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;

                //pdfDocument.AddHeaderTemplate(60);

                //AddHeader(pdfDocument, baseUrl);


                ////Show Draft or not
                //if (cc.DraftTextUse == true)
                //{

                //    if (model.Contract.StateSecondaryId >= cc.DraftShowOnMinStateSecondaryId && model.Contract.StateSecondaryId <= cc.DraftShowOnMaxStateSecondaryId)
                //    {

                // Get the stamp width and height
                float stampWidth = pdfDocument.Pages[0].ClientRectangle.Width; // float.Parse("600");
                        float stampHeight = float.Parse("600");

                        // Center the stamp at the top of PDF page
                        float stampXLocation = (pdfDocument.Pages[0].ClientRectangle.Width - stampWidth) / 2;
                        float stampYLocation = 200;

                        RectangleF stampRectangle = new RectangleF(stampXLocation, stampYLocation, stampWidth, stampHeight);

                        // Create the stamp template to be repeated in each PDF page
                        Winnovative.Template stampTemplate = pdfDocument.AddTemplate(stampRectangle);

                        var stamp = "<!DOCTYPE html><html><head><style>body{-ms-filter:\"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)\";filter: alpha(opacity=50);-moz-opacity: 0.5;-khtml-opacity: 0.5;opacity: 0.5;}</style></head><body class=\"draft\"><br /><br /><br /><br /><br /><br /><p style=\"color:#ccc; font-size:150px; font-family:Verdana;\">" + "Draft" + "</p></body></html>"; // RenderRazorViewToString("~/FormLibContent/Areas/Netherlands/Views/Shared/Pdfs/_Draft.cshtml", model, true);

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
                //    }
                //}



                pdfDocument.CompressionLevel = PdfCompressionLevel.NoCompression;

                // Save the PDF document in a memory buffer
                outPdfBuffer = pdfDocument.Save();

                //TODO: ADD
                // // Send the PDF as response to browser
                // // Set response content type
                //  Response.AddHeader("Content-Type", "application/pdf");

                // // Instruct the browser to open the PDF file as an attachment or inline
                // Response.AddHeader("Content-Disposition", String.Format("inline; filename=" + id + ".pdf; size={0}", outPdfBuffer.Length.ToString()));

                // // Write the PDF document buffer to HTTP response
                // Response.BinaryWrite(outPdfBuffer);

                // // End the HTTP response and stop the current page processing
                // Response.End();

            }
            finally
            {
                // Close the PDF document
                pdfDocument.Close();
            }

            return outPdfBuffer;


        }


        private void AddHeader(Winnovative.Document pdfDocument, string baseURL)
        {
            PdfConverter pdfConverter = new PdfConverter();

            // Create a text element with page numbering place holders &p; and & P;
            //  TextElement footerText = new TextElement(260, pdfDocument.Pages[0].Margins.Bottom - 20, "Side &p; av &P;  ",
            //TextElement footerText = new TextElement(0, 0, "&p;",
            TextElement footerText = new TextElement(0, 0, "",
            new Font(new FontFamily("Verdana"), 9, GraphicsUnit.Point));

            footerText.ForeColor = Color.Gray;

            pdfConverter.PdfFooterOptions.AddElement(footerText);

            // Add variable HTML element with page numbering to footer
            pdfDocument.Header.AddElement(footerText);

        }

        private void AddFooter(Winnovative.Document pdfDocument, string baseURL)
        {

            var footerHtml = "<!DOCTYPE html><html><head></head><body class=\"footer-au\" style=\"text-align:center !important;\"><p style=\"color:#000; font-size:12px; font-family:Verdana !important;\">" + "<p><strong>Confidential</strong></p><p>Contract with [Co-worker First Name] [Co-worker Last Name] dated [Today's date]</p><p>Initials:  _____</p>" + "</p></body></html>"; // RenderRazorViewToString("~/FormLibContent/Areas/Netherlands/Views/Shared/Pdfs/_Draft.cshtml", model, true);


            // Create the HTML element to add in stamp template
            var y = pdfDocument.Pages[0].Margins.Bottom - 60;
            HtmlToPdfElement footerHtmlElement = new HtmlToPdfElement(0, 0, footerHtml, baseURL);

            footerHtmlElement.WebFontsEnabled = true;
            
            //HtmlToPdfElement stampHtmlElement =  new HtmlToPdfElement(0, 0, 0, 60,
            //stamp, 1024, 0);

            // Set the HTML viewer width for the HTML added in stamp
            //footerHtmlElement.HtmlViewerWidth = 793;
            //// Fit the HTML content in stamp template
            footerHtmlElement.FitWidth = true;
            footerHtmlElement.FitHeight = true;

            ////// Add HTML to stamp template
            //stampTemplate.AddElement(stampHtmlElement);

            PdfConverter pdfConverter = new PdfConverter();

            //// Create a text element with page numbering place holders &p; and & P;
            //TextElement footerText = new TextElement(260, pdfDocument.Pages[0].Margins.Bottom - 20, "Side &p; av &P;  ",
            //new Font(new FontFamily("Verdana"), 9, GraphicsUnit.Point));

            //footerText.ForeColor = Color.Gray;

            pdfConverter.PdfFooterOptions.AddElement(footerHtmlElement);

            // Add variable HTML element with page numbering to footer
            pdfDocument.Footer.AddElement(footerHtmlElement);

        }



        //Byt namn
        //public ActionResult CaseDocumentGet(Guid caseDocumentGUID)
        public ActionResult CaseDocumentGet(int caseDocumentId, int caseId)
        {

            //TODO: Check if user have access to this case?

            CaseDocumentModel m = _caseDocumentService.GetCaseDocument(caseDocumentId, caseId);

            //// Get the base URL
            string baseUrl = this.ControllerContext.HttpContext.Request.Url.AbsoluteUri;

            byte[] outPdfBuffer = GeneratePdf(m,baseUrl).ToArray();

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
