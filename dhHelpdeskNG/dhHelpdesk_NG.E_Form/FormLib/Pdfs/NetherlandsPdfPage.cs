using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECT.Model.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using ECT.Core.Cache;
using System.IO;
using ECT.FormLib.Models;
using Winnovative;
using System.Drawing;
using ECT.FormLib.Controllers;

namespace ECT.FormLib.Pdfs
{

    public class ContractPage
    {
        public string ViewName { get; set; }
        public float MarginTop {get;set;}
        public float MarginBottom {get;set;}
        public string Html { get; set; }
    }

    public class ContractsConfiguration
    {
         public bool FooterUse { get; set; }

         public string LastParagraphText { get; set; }
         //public string EmployerSignatureText1 { get; set; }
         //public string EmployerSignatureText2 { get; set; }
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

    public class WinnovativePdfPage : FormLibBaseController
    {
        public static byte[] GeneratePdf(FormModel model, string id, List<ContractPage> contractPages, ContractsConfiguration cc, string currentPageUrl, string baseUrl)
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

           //     if (ECT.FormLib.Pdfs.CustomPdfPage.ShowDraft(model.Contract.StateSecondaryId) && cc.DraftTextUse == true)
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

                    var stamp = "<!DOCTYPE html><html><head><style>body{-ms-filter:\"progid:DXImageTransform.Microsoft.Alpha(Opacity=50)\";filter: alpha(opacity=50);-moz-opacity: 0.5;-khtml-opacity: 0.5;opacity: 0.5;}</style></head><body class=\"draft\"><br /><br /><br /><br /><br /><br /><p style=\"color:#ccc; font-size:150px; font-family:Verdana;\">DRAFT</p></body></html>"; // RenderRazorViewToString("~/FormLibContent/Areas/Netherlands/Views/Shared/Pdfs/_Draft.cshtml", model, true);

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

                //LÄGG TILLBAKA
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

        

    }



    public class NetherlandsPdfPage: CustomPdfPage 
    {
        public Department department { get; set; }

        public string id { get; set; }
        public string path { get; set; }
        public string prefixes { get; set; }

 
        public ContractsConfiguration contractsConfig { get; set; }

        public static ContractsConfiguration Get(string id)
        {
            ContractsConfiguration contractsConfig;

            //open configuration
            #region configuration settings

            var path = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory)).FullName;
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(Path.Combine(path, "netherlands/ContractsConfiguration.xml"));

            //get info from config where id ex: "ContractDutch" 
            var contracts = from contract in xmlDocument.ToXDocument().Descendants("contract")
                            where (string)contract.Attribute("id") == id
                            // orderby contract.Element("id").Value ascending
                            select new
                            {

                                //EmployerSignatureText1 = contract.Element("employerSignatureText1").Value,
                                //EmployerSignatureText2 = contract.Element("employerSignatureText2").Value,
                                EmployerSignatureText = contract.Element("employerSignatureText").Value,
                                EmployeeSignatureText = contract.Element("employeeSignatureText").Value,
                                DocumentTypeCode = contract.Element("documentTypeCode").Value,
                                BarcodeUse = bool.Parse(contract.Attribute("barcodeUse").Value),
                                DraftTextUse = bool.Parse(contract.Attribute("draftTextUse").Value),
                                Process = contract.Attribute("process").Value,
                                BarcodeShowOnMinStateSecondaryId = int.Parse(contract.Attribute("barcodeShowOnMinStateSecondaryId").Value),
                                DraftShowOnMinStateSecondaryId = int.Parse(contract.Attribute("draftShowOnMinStateSecondaryId").Value),
                                DraftShowOnMaxStateSecondaryId = int.Parse(contract.Attribute("draftShowOnMaxStateSecondaryId").Value),
                                Pages = contract.Descendants("page")
                            };


            var contractsList = contracts.ToList();


            contractsConfig = new ContractsConfiguration();
            //contractsConfig.EmployerSignatureText1 = contractsList[0].EmployerSignatureText1;
            //contractsConfig.EmployerSignatureText2 = contractsList[0].EmployerSignatureText2;
            contractsConfig.EmployerSignatureText = new HtmlString(contractsList[0].EmployerSignatureText);
            contractsConfig.EmployeeSignatureText = new HtmlString(contractsList[0].EmployeeSignatureText);
            contractsConfig.DocumentTypeCode = contractsList[0].DocumentTypeCode;
            contractsConfig.BarcodeUse = contractsList[0].BarcodeUse;
            contractsConfig.DraftTextUse = contractsList[0].DraftTextUse;
            contractsConfig.Process = contractsList[0].Process;
            contractsConfig.BarcodeShowOnMinStateSecondaryId = contractsList[0].BarcodeShowOnMinStateSecondaryId;

            //contractsConfig.MarginLeft = float.Parse(xmlDocument.ToXDocument().Root.Attribute("MarginLeft").Value);
            //contractsConfig.MarginRight = float.Parse(xmlDocument.ToXDocument().Root.Attribute("MarginRight").Value);
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


       


        public override void OnOpenDocument(PdfWriter writer, iTextSharp.text.Document doc)
        {
            //open configuration
            #region configuration settings

            var path = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory)).FullName;
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(Path.Combine(path, "netherlands/ContractsConfiguration.xml"));

            //get info from config where id ex: "ContractDutch" 
            var contracts = from contract in xmlDocument.ToXDocument().Descendants("contract")
                            where (string)contract.Attribute("id") ==  id
                            // orderby contract.Element("id").Value ascending
                            select new
                            {
                                FooterUse = bool.Parse(contract.Attribute("footerUse").Value),
                               
                                LastParagraphText = contract.Element("lastParagraphText").Value,
                                EmployerSignatureText1 = contract.Element("employerSignatureText1").Value,
                                EmployerSignatureText2 = contract.Element("employerSignatureText2").Value,
                                EmployeeSignatureText = contract.Element("employeeSignatureText").Value,
                                DocumentTypeCode = contract.Element("documentTypeCode").Value,
                                BarcodeUse = bool.Parse(contract.Attribute("barcodeUse").Value),
                                SignatureUse = bool.Parse(contract.Attribute("signatureUse").Value),
                                CityDateUse = bool.Parse(contract.Attribute("cityDateUse").Value),
                                EmployeeSignatureTextUse = bool.Parse(contract.Attribute("employeeSignatureTextUse").Value),
                                Process = contract.Attribute("process").Value,
                                BarcodeShowOnMinStateSecondaryId = int.Parse(contract.Attribute("barcodeShowOnMinStateSecondaryId").Value)
                                
                                
                            };


            var contractsList = contracts.ToList();

       
                contractsConfig = new ContractsConfiguration();
                contractsConfig.FooterUse = contractsList[0].FooterUse;
                contractsConfig.LastParagraphText = contractsList[0].LastParagraphText;
                //contractsConfig.EmployerSignatureText1 = contractsList[0].EmployerSignatureText1;
                //contractsConfig.EmployerSignatureText2 = contractsList[0].EmployerSignatureText2;

                contractsConfig.EmployeeSignatureText = new HtmlString(contractsList[0].EmployeeSignatureText);
                contractsConfig.DocumentTypeCode = contractsList[0].DocumentTypeCode;
                contractsConfig.BarcodeUse = contractsList[0].BarcodeUse;
                contractsConfig.SignatureUse = contractsList[0].SignatureUse;
                contractsConfig.CityDateUse = contractsList[0].CityDateUse;
                contractsConfig.EmployeeSignatureTextUse = contractsList[0].EmployeeSignatureTextUse;
                contractsConfig.Process = contractsList[0].Process;
                contractsConfig.BarcodeShowOnMinStateSecondaryId = contractsList[0].BarcodeShowOnMinStateSecondaryId;

            #endregion

            //open configuration

                 //Comment: Barcode for NL should only be shown in status 50 for hiring. In other processes in status 40.
                if (Contract.StateSecondaryId >= contractsConfig.BarcodeShowOnMinStateSecondaryId)
                {

                if (contractsConfig.BarcodeUse == true && (!string.IsNullOrEmpty(Contract.EmployeeNumber)))
                {
                    PdfPTable headerTbl = new PdfPTable(2);
                    headerTbl.TotalWidth = doc.PageSize.Width;

                    var c1 = new PdfPCell(new Phrase("")); // Should be to cell adjustment
                    c1.Border = 0;
                    headerTbl.AddCell(c1);

                    //string code = Contract.EmployeeNumber + documentTypeCode;
                    string code = Contract.EmployeeNumber + contractsConfig.DocumentTypeCode;

                    var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
                    FontFactory.RegisterDirectory(folderPath);

                    iTextSharp.text.Font bcfont = FontFactory.GetFont("IDAutomationHC39M", BaseFont.CP1252, BaseFont.EMBEDDED, 11);


                    bcfont.Color = BaseColor.BLACK;

                    var para = new Paragraph(code, bcfont);
                    para.Alignment = Element.ALIGN_CENTER;
                    para.Add(Environment.NewLine);
                    para.Alignment = Element.ALIGN_CENTER;

                    var cell = new PdfPCell(para);
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Border = 0;
                    cell.PaddingTop = 50;
                    cell.PaddingRight = 45;
                    headerTbl.AddCell(cell);

                    headerTbl.WriteSelectedRows(0, -1, 0, doc.PageSize.Height + 5, writer.DirectContent);
                }

                }
        }

        public override void OnParagraph(PdfWriter writer, iTextSharp.text.Document doc, float currentLocation)
        {
            var remainingspace = writer.GetVerticalPosition(false) - doc.BottomMargin;
            if (doc.IsMarginMirroring() && (remainingspace <= 100))
            {
                doc.NewPage();
                writer.Flush();
            }
        }

        private void Signature(PdfWriter writer, iTextSharp.text.Document doc, float currentLocation)
        {

            //var sigTbl = new PdfPTable(3);
            //sigTbl.TotalWidth = doc.PageSize.Width;
            //sigTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            //#region LastParagraph
            ///** LAST PARAGRAPH **/

            ////check against config file
            //string lastParagraphText = contractsConfig.LastParagraphText;

            //if (!string.IsNullOrEmpty(lastParagraphText))
            //{
            //    var paragraphLast = new Paragraph(lastParagraphText, SignatureFont(11));

            //    var cellLastParagraph = new PdfPCell(paragraphLast);
            //    cellLastParagraph.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellLastParagraph.VerticalAlignment = Element.ALIGN_LEFT;
            //    cellLastParagraph.Border = 0;
            //    cellLastParagraph.PaddingTop = 0;
            //    cellLastParagraph.PaddingLeft = 40;
            //    cellLastParagraph.PaddingRight = 35;
            //    cellLastParagraph.PaddingBottom = 0;
            //    cellLastParagraph.Colspan = 3;
            //    cellLastParagraph.SetLeading(0.0f, 1.9f);

            //    sigTbl.AddCell(cellLastParagraph);
            //}
          
            ////** LAST PARAGRAPH **/
            //#endregion

            ////** CITY/DATE **/
            //if (contractsConfig.CityDateUse == true)
            //{
            //    string dateWithFormat = string.Format(new System.Globalization.CultureInfo("nl-NL"), "{0:dd MMMM yyyy}", DateTime.Now);
            //    var paraCityDate = new Paragraph("", SignatureFont(11));
               

            //    //if (department.HeadOfDepartmentCity != null)
            //    //{
            //    city = CustomPdfPage.IsEmpty(department.HeadOfDepartmentCity, "City");
            //    paraCityDate = new Paragraph(city + ", " + dateWithFormat, SignatureFont(11));
            //    //}
            //    //else
            //    //{
            //    //    paraCityDate = new Paragraph(dateWithFormat, SignatureFont);
            //    //}

            //    var cellCityDate = new PdfPCell(paraCityDate);
            //    cellCityDate.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellCityDate.VerticalAlignment = Element.ALIGN_LEFT;
            //    cellCityDate.Border = 0;
            //    cellCityDate.PaddingTop = 0;
            //    cellCityDate.PaddingLeft = 40;
            //    cellCityDate.PaddingBottom = 0;
            //    cellCityDate.Colspan = 3;

            //    sigTbl.AddCell(cellCityDate);

            //}
            ////** CITY/DATE **/


            ////** HEADLINES **/

            ////Default
            ////arbetstagarens
            //string employeeSignatureText = contractsConfig.EmployeeSignatureText;

            ////arbetsgivaren
            //string employerSignatureTextLine1 = contractsConfig.EmployerSignatureText1;
            //string employerSignatureTextLine2 = contractsConfig.EmployerSignatureText2;
            
            ////left
            //var paraSignature = new Paragraph("", SignatureFont(11));
            //paraSignature.Add(employerSignatureTextLine1);
            //paraSignature.Add(Environment.NewLine);
            //paraSignature.Add(employerSignatureTextLine2);

            //var cellSignature = new PdfPCell(paraSignature);
            //cellSignature.HorizontalAlignment = Element.ALIGN_LEFT;
            //cellSignature.VerticalAlignment = Element.ALIGN_LEFT;
            //cellSignature.Border = 0;
            //cellSignature.PaddingTop = 20;
            //cellSignature.PaddingLeft = 40;
            //cellSignature.PaddingBottom = 10;
            //cellSignature.Colspan = 2;
            //cellSignature.SetLeading(0.0f, 1.5f);

            //sigTbl.AddCell(cellSignature);

            ////Right
            //paraSignature = new Paragraph(employeeSignatureText, SignatureFont(11));
            //paraSignature.Add(Environment.NewLine);
            //paraSignature.Add(Environment.NewLine);
            //paraSignature.Alignment = Element.ALIGN_LEFT;

            //cellSignature = new PdfPCell(paraSignature);
            //cellSignature.HorizontalAlignment = Element.ALIGN_LEFT;
            //cellSignature.Border = 0;
            //cellSignature.PaddingTop = 20;
            //cellSignature.PaddingLeft = 0;
            //cellSignature.PaddingRight = 35;
            //cellSignature.PaddingBottom = 10;
            //cellSignature.SetLeading(0.0f, 1.5f);

            //sigTbl.AddCell(cellSignature);
            ////** HEADLINES **/

            //var para = new Paragraph("", SignatureFont(11));

            ////Check if we should us a signatuer
            //if (contractsConfig.SignatureUse == true)
            //{

            //    //ADD ROW FOR IMAGE
            //    para = new Paragraph("", SignatureFont(11));

            //    var cellImage = new PdfPCell(para);

               
           
            //    if (!string.IsNullOrEmpty(department.HeadOfDepartmentSignature))
            //    {
            //        var HeadOfdeparmentSignature = Image.GetInstance(HttpContext.Current.Server.MapPath(department.HeadOfDepartmentSignature));

            //        var imageHeight = 47;

            //        HeadOfdeparmentSignature.ScaleAbsolute(150, imageHeight);
            //        //set cell height to minimum of image height
            //        if (HeadOfdeparmentSignature != null)
            //        {
            //            cellImage.MinimumHeight = imageHeight;
            //        }

            //        para.Alignment = Element.ALIGN_LEFT;

            //        para.Add(new Chunk(HeadOfdeparmentSignature, 5, -50));

            //        para.Add(Environment.NewLine);
            //        para.Add(Environment.NewLine);
            //    }
                



            //    cellImage.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellImage.VerticalAlignment = Element.ALIGN_LEFT;
            //    cellImage.Border = 0;
            //    cellImage.PaddingTop = 0;
            //    cellImage.PaddingLeft = 40;
            //    cellImage.PaddingBottom = 0;
            //    cellImage.Colspan = 3;
 
            //    sigTbl.AddCell(cellImage);

            //    para = new Paragraph("", SignatureFont(11));
            //    cellImage = new PdfPCell(para);

            //   para.Add(CustomPdfPage.IsEmpty(department.HeadOfDepartmentName, "Name"));
            //   para.Add(Environment.NewLine);
            //   cellImage.SetLeading(0.0f, 1.4f);
               

            //    //function
            //    cellImage.SetLeading(0.0f, 1.4f);
            //    para.Add(CustomPdfPage.IsEmpty(department.HeadOfDepartmentTitle, "Function"));

            //    cellImage.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellImage.VerticalAlignment = Element.ALIGN_LEFT;
            //    cellImage.Border = 0;
            //    cellImage.PaddingTop = 0;
            //    cellImage.PaddingLeft = 40;
            //    cellImage.PaddingBottom = 0;
            //    cellImage.Colspan = 2;

            //    sigTbl.AddCell(cellImage);

            //}
            //else
            //{
            //    //If we don´t add a signature need an empty cell
            //    var paraEmpty = new Paragraph("", SignatureFont(11));

            //    var cellEmpty = new PdfPCell(paraEmpty);
            //    cellEmpty.HorizontalAlignment = Element.ALIGN_LEFT;
            //    cellEmpty.VerticalAlignment = Element.ALIGN_LEFT;
            //    cellEmpty.MinimumHeight = 100;
            //    cellEmpty.Border = 0;
            //    cellEmpty.PaddingTop = 0;
            //    cellEmpty.PaddingLeft = 40;
            //    cellEmpty.PaddingBottom = 0;
            //    cellEmpty.Colspan = 2;
             
            //    sigTbl.AddCell(cellEmpty);
            //}

            //para = new Paragraph(" ", SignatureFont(11));
            //para.Alignment = Element.ALIGN_LEFT;

            ////If we don´t add a signature we need to make room for the employee signature to the right
            //if (contractsConfig.SignatureUse == false)
            //{
            //    para.Add(Environment.NewLine);
            //}


            //if (contractsConfig.EmployeeSignatureTextUse == true)
            //{
            //    para.Add(Contract.FirstName + " " + prefixes +  Contract.Surname);
            //}
            //else
            //{
            //    para.Add(Environment.NewLine);
            //}

            //var cell = new PdfPCell(para);
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.Border = 0;
            //cell.PaddingTop = 0;
            //cell.PaddingLeft = 0;
            //cell.PaddingRight = 35;
            //cell.PaddingBottom = 0;
            //cell.SetLeading(0.0f, 1.4f);

            //sigTbl.AddCell(cell);

            //var signatureHeight = sigTbl.CalculateHeights();
            //var documentHeight = doc.PageSize.Height;
            //var signatureX = currentLocation;

            //var remainingspace = writer.GetVerticalPosition(false) - doc.BottomMargin;
            //remainingspace += 10;

            //if (signatureHeight > remainingspace)
            //{
            //    doc.NewPage();
            //    //signatureX = documentHeight - (signatureHeight - doc.TopMargin);
            //    //add to the top
            //    signatureX = 777;
            //}

            //sigTbl.WriteSelectedRows(0, -1, 0, signatureX, writer.DirectContent);
        }

        public override void OnEndPage(PdfWriter writer, iTextSharp.text.Document doc)
        {
            var currentLocation = writer.GetVerticalPosition(false);

            if (doc.IsMarginMirroring())
            {
                doc.SetMarginMirroring(false);

                if (contractsConfig.FooterUse == true)
                {
                    Signature(writer, doc, currentLocation - 20);
                }
            }

            ////Show Draft or not
            if (ShowDraft(Contract.StateSecondaryId))
            {
                Phrase watermark = new Phrase("DRAFT", Verdana(80, BaseColor.LIGHT_GRAY));
                PdfContentByte canvas = writer.DirectContentUnder;
                ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, watermark, 298, 421, 45);
            }
        }

      
        public static string GetLetter(int value)
        {

            string[] alpha2 = new string[13];

            alpha2[0] = "a";
            alpha2[1] = "b";
            alpha2[2] = "c";
            alpha2[3] = "d";
            alpha2[4] = "e";
            alpha2[5] = "f";
            alpha2[6] = "g";
            alpha2[7] = "h";
            alpha2[8] = "i";
            alpha2[9] = "j";
            alpha2[10] = "k";
            alpha2[11] = "l";
            alpha2[12] = "m";

            return alpha2[value].ToString();
        }




    }
}