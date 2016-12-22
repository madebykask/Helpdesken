using DH.Helpdesk.EForm.FormLib.Controllers;
using DH.Helpdesk.EForm.FormLib.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Winnovative;

namespace DH.Helpdesk.EForm.FormLib.Pdfs
{  

    public class GlobalContractsConfiguration
    {
        public bool HeaderUse { get; set; }
        public bool FooterUse { get; set; }

        public string LastParagraphText { get; set; }   
        public HtmlString EmployerSignatureText { get; set; }
        public HtmlString EmployeeSignatureText { get; set; }
        public string DocumentTypeCode { get; set; }

        public bool BarcodeUse { get; set; }

        public bool SignatureUse { get; set; }
        public bool CityDateUse { get; set; }
        public bool EmployeeSignatureTextUse { get; set; }

        public string Process { get; set; }
        public int BarcodeShowOnMinStateSecondaryId { get; set; }

        // UK & IE
        public bool AddressBoxUse { get; set; }

        //UK
        public string LastParagraphText2 { get; set; }
        public string EncTextLeft { get; set; }
        public string EncTextRight { get; set; }

        //Print UK Change Terms Conditions
        public bool PageNumberUse { get; set; }
        public bool DraftTextUse { get; set; }

        public string TextAlign { get; set; }
        public int FontSize { get; set; }
        public string Style { get; set; }

        public List<ContractPage> Pages { get; set; }

        //public float MarginLeft { get; set; }
        //public float MarginRight { get; set; }
        public float MarginTop { get; set; }
        public float MarginBottom { get; set; }

        public int DraftShowOnMinStateSecondaryId { get; set; }
        public int DraftShowOnMaxStateSecondaryId { get; set; }

    }

    public class GlobalWinnovativePdfPage : FormLibBaseController
    {
        public virtual byte[] GeneratePdf(FormModel model, string id, List<ContractPage> contractPages, GlobalContractsConfiguration cc, string currentPageUrl, string baseUrl)
        {
            // Create the PDF document where to add the HTML documents
            Winnovative.Document pdfDocument = new Winnovative.Document();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            pdfDocument.LicenseKey = "xUtbSltKWkpbW0RaSllbRFtYRFNTU1M=";

            byte[] outPdfBuffer;

            try
            {
                //Loop through Pages and add Views if they exist (Appendix etc)
                if (contractPages.Count > 0)
                {
                    foreach (ContractPage coP in contractPages)
                    {

                        Winnovative.PdfPage contractPdfPage = null;
                        PointF contractHtmlLocation = Point.Empty;

                        contractPdfPage = pdfDocument.AddPage();
                        
                        pdfDocument.AddFooterTemplate(coP.MarginBottom);
                        //contractPdfPage.Margins.Left = coP.MarginLeft;
                        //contractPdfPage.Margins.Right = coP.MarginRight;
                        contractPdfPage.Margins.Top = coP.MarginTop;
                        contractPdfPage.Margins.Bottom = coP.MarginBottom;

                        contractHtmlLocation = PointF.Empty;

                        var contractHtmlToConvert = coP.Html; // RenderRazorViewToString(coP.ViewName, model, false);

                        // Create the contract HTML to PDF element
                        HtmlToPdfElement contractHtml = new HtmlToPdfElement(0, 0, contractHtmlToConvert, baseUrl);
                        contractHtml.FitWidth = false;
                        contractHtml.HtmlViewerWidth = 793;
                        // Optionally set a delay before conversion to allow asynchonous scripts to finish
                        contractHtml.ConversionDelay = 0;
                        // Add the HTML to PDF document
                        contractPdfPage.AddElement(contractHtml);

  
                    }


                }

                //Show Draft or not

                if (cc.DraftTextUse == true)
                {

                    if (model.Contract.StateSecondaryId >= cc.DraftShowOnMinStateSecondaryId && model.Contract.StateSecondaryId <= cc.DraftShowOnMaxStateSecondaryId)
                    {

                        //     if (DH.Helpdesk.EForm.FormLib.Pdfs.CustomPdfPage.ShowDraft(model.Contract.StateSecondaryId) && cc.DraftTextUse == true)
                        //                {
                        // Get the stamp width and height
                        float stampWidth = pdfDocument.Pages[0].ClientRectangle.Width; // float.Parse("600");
                        float stampHeight = float.Parse("600");

                        // Center the stamp at the top of PDF page
                        float stampXLocation = (pdfDocument.Pages[0].ClientRectangle.Width - stampWidth) / 2;
                        float stampYLocation = 200;

                        RectangleF stampRectangle = new RectangleF(stampXLocation, stampYLocation, stampWidth, stampHeight);

                        // Create the stamp template to be repeated in each PDF page
                        Template stampTemplate = pdfDocument.AddTemplate(stampRectangle);

                        var stamp = "<!DOCTYPE html><html><head><style>body{-ms-filter:\"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)\";filter: alpha(opacity=50);-moz-opacity: 0.5;-khtml-opacity: 0.5;opacity: 0.5;}</style></head><body class=\"draft\"><br /><br /><br /><br /><br /><br /><p style=\"color:#ccc; font-size:150px; font-family:Verdana;\">" + model.Translate("Draft").ToUpper() + "</p></body></html>"; // RenderRazorViewToString("~/FormLibContent/Areas/Netherlands/Views/Shared/Pdfs/_Draft.cshtml", model, true);

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
                }


                pdfDocument.CompressionLevel = PdfCompressionLevel.NoCompression;

                // Save the PDF document in a memory buffer
                outPdfBuffer = pdfDocument.Save();                

               // System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
               
               // //LÄGG TILLBAKA
               // // // Send the PDF as response to browser
               // // // Set response content type
               // Response.AddHeader("Content-Type", baseUrl);

               // // // Instruct the browser to open the PDF file as an attachment or inline
               //// Response.AddHeader("Content-Disposition", String.Format("inline; filename=" + id + ".pdf; size={0}", outPdfBuffer.Length.ToString()));

               // // // Write the PDF document buffer to HTTP response
               //  Response.BinaryWrite(outPdfBuffer);

               // // // End the HTTP response and stop the current page processing
               //  Response.End();

            }
            finally
            {
                // Close the PDF document
                pdfDocument.Close();
            }

            return outPdfBuffer;

        }       

    }   

}
