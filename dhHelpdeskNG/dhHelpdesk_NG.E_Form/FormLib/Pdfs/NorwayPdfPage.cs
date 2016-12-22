using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.EForm.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using DH.Helpdesk.EForm.Core.Cache;
using System.IO;
using DH.Helpdesk.EForm.FormLib.Models;
using Winnovative;
using System.Drawing;
using DH.Helpdesk.EForm.FormLib.Controllers;

namespace DH.Helpdesk.EForm.FormLib.Pdfs
{


    public class NorwayWinovativePdfPage : GlobalWinnovativePdfPage
    {
        public override byte[] GeneratePdf(FormModel model, string id, List<ContractPage> contractPages, GlobalContractsConfiguration cc, string currentPageUrl, string baseUrl)
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
                //Loop through Pages and add Views if they exist (Appendix etc)
                if (contractPages.Count > 0)
                {
                    foreach (ContractPage coP in contractPages)
                    {

                        Winnovative.PdfPage contractPdfPage = null;
                        PointF contractHtmlLocation = Point.Empty;

                        contractPdfPage = pdfDocument.AddPage();

                        pdfDocument.AddFooterTemplate(coP.MarginBottom);
                        pdfDocument.AddHeaderTemplate(coP.MarginTop);
                        //contractPdfPage.Margins.Left = coP.MarginLeft;
                        //contractPdfPage.Margins.Right = coP.MarginRight;
                        //contractPdfPage.Margins.Top = coP.MarginTop;
                        contractPdfPage.Margins.Bottom = coP.MarginBottom;

                        contractHtmlLocation = PointF.Empty;

                        var contractHtmlToConvert = coP.Html;

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

                // If the Header is needed
                if (cc.HeaderUse == true)
                {
                    // Enable header in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowHeader = true;                    

                    pdfDocument.AddHeaderTemplate(cc.MarginTop + 20);

                    AddHeader(pdfDocument, baseUrl);
                }

                // If the Header is needed
                if (cc.FooterUse == true)
                {
                    // Enable footer in the generated PDF document
                    htmlToPdfConverter.PdfDocumentOptions.ShowFooter = true;                    

                    pdfDocument.AddFooterTemplate(cc.MarginBottom);

                    AddFooter(pdfDocument, baseUrl);

                }

                //Show Draft or not
                if (cc.DraftTextUse == true)
                {

                    if (model.Contract.StateSecondaryId >= cc.DraftShowOnMinStateSecondaryId && model.Contract.StateSecondaryId <= cc.DraftShowOnMaxStateSecondaryId)
                    {
                     
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
            // The result of adding elements to PDF document
            AddElementResult addElementResult = null;

            // The position on X anf Y axes where to add the next element
            float yLocation = 30;
            float xLocation = 240;

            string imagePath = System.Web.HttpContext.Current.Server.MapPath("~/FormLibContent/assets/img/IKEA_logo_RGB_2.jpg");

            // Add transparent images      

            ImageElement trasparentImageElement = new ImageElement(xLocation, yLocation, 120, imagePath);
            addElementResult = pdfDocument.Header.AddElement(trasparentImageElement);
        }

        private void AddFooter(Winnovative.Document pdfDocument, string baseURL)
        {
            PdfConverter pdfConverter = new PdfConverter();

            // Create a text element with page numbering place holders &p; and & P;
            TextElement footerText = new TextElement(260, pdfDocument.Pages[0].Margins.Bottom - 20, "Side &p; av &P;  ",
            new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 9, System.Drawing.GraphicsUnit.Point));

            footerText.ForeColor = Color.Gray;

            pdfConverter.PdfFooterOptions.AddElement(footerText);

            // Add variable HTML element with page numbering to footer
            pdfDocument.Footer.AddElement(footerText);

        }
    }


    public class NorwayPdfPage : CustomPdfPage 
    {
        public Department department { get; set; }

        public string id { get; set; }
        public string path { get; set; }
        public string prefixes { get; set; }


        public  GlobalContractsConfiguration contractsConfig { get; set; }

        public static GlobalContractsConfiguration Get(string id)
        {
            GlobalContractsConfiguration contractsConfig;

            //open configuration
            #region configuration settings

            var path = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory)).FullName;
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(Path.Combine(path, "Norway/ContractsConfiguration.xml"));

            //get info from config where id ex: "ContractDutch" 
            var contracts = from contract in xmlDocument.ToXDocument().Descendants("contract")
                            where (string)contract.Attribute("id") == id
                            // orderby contract.Element("id").Value ascending
                            select new
                            {

                                EmployerSignatureText = contract.Element("employerSignatureText").Value,
                                EmployeeSignatureText = contract.Element("employeeSignatureText").Value,                             
                                BarcodeUse = bool.Parse(contract.Attribute("barcodeUse").Value),
                                FooterUse = bool.Parse(contract.Attribute("footerUse").Value),
                                HeaderUse = bool.Parse(contract.Attribute("headerUse").Value),
                                DraftTextUse = bool.Parse(contract.Attribute("draftTextUse").Value),
                                Process = contract.Attribute("process").Value,
                                //BarcodeShowOnMinStateSecondaryId = int.Parse(contract.Attribute("barcodeShowOnMinStateSecondaryId").Value),
                                DraftShowOnMinStateSecondaryId = int.Parse(contract.Attribute("draftShowOnMinStateSecondaryId").Value),
                                DraftShowOnMaxStateSecondaryId = int.Parse(contract.Attribute("draftShowOnMaxStateSecondaryId").Value),
                                Pages = contract.Descendants("page")
                            };


            var contractsList = contracts.ToList();


            contractsConfig = new GlobalContractsConfiguration();
            contractsConfig.EmployerSignatureText = new HtmlString(contractsList[0].EmployerSignatureText);
            contractsConfig.EmployeeSignatureText = new HtmlString(contractsList[0].EmployeeSignatureText);
            contractsConfig.BarcodeUse = contractsList[0].BarcodeUse;
            contractsConfig.HeaderUse = contractsList[0].HeaderUse;
            contractsConfig.FooterUse = contractsList[0].FooterUse;
            contractsConfig.DraftTextUse = contractsList[0].DraftTextUse;
            contractsConfig.Process = contractsList[0].Process;
           
            contractsConfig.MarginTop = float.Parse(xmlDocument.ToXDocument().Root.Attribute("MarginTop").Value);
            contractsConfig.MarginBottom = float.Parse(xmlDocument.ToXDocument().Root.Attribute("MarginBottom").Value);

            contractsConfig.DraftShowOnMinStateSecondaryId = contractsList[0].DraftShowOnMinStateSecondaryId;
            contractsConfig.DraftShowOnMaxStateSecondaryId = contractsList[0].DraftShowOnMaxStateSecondaryId;

            contractsConfig.Pages = new List<ContractPage>();

            foreach (XElement e in contractsList[0].Pages)
            {
                ContractPage coP = new ContractPage();

                coP.ViewName = e.Attribute("viewName").Value;
                //coP.MarginLeft = float.Parse(e.Attribute("MarginLeft").Value);
                //coP.MarginRight = float.Parse(e.Attribute("MarginRight").Value);
                coP.MarginTop = float.Parse(e.Attribute("MarginTop").Value);
                coP.MarginBottom = float.Parse(e.Attribute("MarginBottom").Value);
                contractsConfig.Pages.Add(coP);
            }
            #endregion

            //open configuration

            return contractsConfig;
        }
    }
   
}