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
using System.Globalization;
using System.Threading;
using DH.Helpdesk.EForm.FormLib.Models;
using DH.Helpdesk.EForm.Model.Abstract;
using DH.Helpdesk.EForm.Model.Contrete;


namespace DH.Helpdesk.EForm.FormLib.Pdfs
{
    public class SouthKoreaPdfPage : CustomPdfPage
    {
        public Department department { get; set; }
        public FormModel model { get; set; }

        public string id { get; set; }
        public string path { get; set; }

        PdfContentByte cb;
        PdfTemplate template;

        //for centering the page number
        const int leftMargin = 234;

        public static ContractsConfiguration contractsConfig { get; set; }

        BaseFont customfont = BaseFont.CreateFont("C:\\Windows\\Fonts\\Malgun.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        BaseFont customVerdanafont = BaseFont.CreateFont("C:\\Windows\\Fonts\\verdana.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);

       

        /// <summary>
        /// Get TextAlign from contractsconfig.xml, Default = Left
        /// </summary>
        public int TextAlign()
        {

            if (contractsConfig.TextAlign.ToLower() == "justify")
            {
                return Element.ALIGN_JUSTIFIED;
            }

            if (contractsConfig.TextAlign.ToLower() == "justifyall")
            {
                return Element.ALIGN_JUSTIFIED_ALL;
            }

            return Element.ALIGN_LEFT;

            
        }

        /// <summary>
        /// Get FontSize from contractsconfig.xml, Default = 10
        /// </summary>
        public int FontSize()
        {
            return contractsConfig.FontSize;
        }

        public static string StyleValue()
        {

            return contractsConfig.Style.ToString();
        }

        // Print Date on the First Page and Special Position
        private void PrintDate(PdfWriter writer, Document doc)
        {
            PdfPTable headerTbl = new PdfPTable(1);
            var xpointsValue = iTextSharp.text.Utilities.MillimetersToPoints(150);
            var ypointsValue = iTextSharp.text.Utilities.MillimetersToPoints(22);
            headerTbl.TotalWidth = doc.PageSize.Width - (xpointsValue);

            Font font = new Font(customfont, 10);

            DateTime dt = DateTime.Now;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");


            var c = new PdfPCell(new Phrase(dt.ToString("d MMMMMM yyyy"), font));
            c.VerticalAlignment = Element.ALIGN_LEFT;
            c.HorizontalAlignment = Element.ALIGN_LEFT;

            c.PaddingTop = -11;
            c.Border = 0;
            c.PaddingLeft = 0;
            headerTbl.AddCell(c);

            headerTbl.WriteSelectedRows(0, -1, xpointsValue, (doc.PageSize.Height - ypointsValue), writer.DirectContent); 
        }

        public override void OnStartPage(PdfWriter writer, Document doc)
        {
           
                PdfPTable headerTbl = new PdfPTable(3);
                headerTbl.TotalWidth = doc.PageSize.Width;

                var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/FormLibContent/assets/img/IKEA_logo_RGB_2.jpg"));
                logo.ScalePercent(14);
                int w = (int)headerTbl.TotalWidth - (200);
                headerTbl.SetWidths(new int[] { 100, w, 100 });

                var c1 = new PdfPCell(logo);
                c1.PaddingLeft = 40;
                c1.Border = 0;
                headerTbl.AddCell(c1);

                var c2 = new PdfPCell(new Phrase(""));
                c2.VerticalAlignment = Element.ALIGN_BOTTOM;
                c2.HorizontalAlignment = Element.ALIGN_CENTER;
                c2.Border = 0;
                c2.PaddingLeft = 15;
                headerTbl.AddCell(c2);

                var c3 = new PdfPCell(new Phrase("")); // Should be to cell adjustment
                c3.Border = 0;
                headerTbl.AddCell(c3);

                headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 35), writer.DirectContent);             
        }



        public override void OnOpenDocument(PdfWriter writer, Document doc)
        {
                                               
            cb = writer.DirectContent;                                    
           
            template = cb.CreateTemplate(doc.PageSize.Width, 50);

            //open configuration
            #region configuration settings

            var path = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory)).FullName;
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(Path.Combine(path, "SouthKorea/ContractsConfiguration.xml"));

            //get info from config where id ex: "ContractDutch" 
            var contracts = from contract in xmlDocument.ToXDocument().Descendants("contract")
                            where (string)contract.Attribute("id") == id

                            select new
                            {
                                SignatureUse = bool.Parse(contract.Attribute("signatureUse").Value),
                                Process = contract.Attribute("process").Value,
                                LastParagraphText = contract.Element("lastParagraphText").Value,
                                LastParagraphText2 = contract.Element("lastParagraphText2").Value,
                                EncTextLeft = contract.Element("encTextLeft").Value,
                                EncTextRight = contract.Element("encTextRight").Value,
                                PageNumberUse = bool.Parse(contract.Attribute("pageNumberUse").Value),
                                DraftTextUse = bool.Parse(contract.Attribute("draftTextUse").Value),
                                AddressBoxUse = bool.Parse(contract.Attribute("addressBoxUse").Value),

                            };


            var contractsList = contracts.ToList();
            contractsConfig = new ContractsConfiguration();

            contractsConfig.LastParagraphText = contractsList[0].LastParagraphText;
            contractsConfig.SignatureUse = contractsList[0].SignatureUse;
            contractsConfig.Process = contractsList[0].Process;
            contractsConfig.LastParagraphText2 = contractsList[0].LastParagraphText2;
            contractsConfig.EncTextLeft = contractsList[0].EncTextLeft;
            contractsConfig.EncTextRight = contractsList[0].EncTextRight;
            contractsConfig.PageNumberUse = contractsList[0].PageNumberUse;
            contractsConfig.DraftTextUse = contractsList[0].DraftTextUse;
            contractsConfig.AddressBoxUse = contractsList[0].AddressBoxUse;
            contractsConfig.TextAlign = xmlDocument.ToXDocument().Root.Attribute("textAlign") != null ? xmlDocument.ToXDocument().Root.Attribute("textAlign").Value : "left";
            contractsConfig.FontSize = xmlDocument.ToXDocument().Root.Attribute("fontSize") != null ? int.Parse(xmlDocument.ToXDocument().Root.Attribute("fontSize").Value) : 11;
            contractsConfig.Style = xmlDocument.ToXDocument().Root.Attribute("style") != null ? xmlDocument.ToXDocument().Root.Attribute("style").Value : "";

            #endregion

            //open configuration         
        }


        public override void OnParagraph(PdfWriter writer, Document doc, float currentLocation)
        {
            var remainingspace = writer.GetVerticalPosition(false) - doc.BottomMargin;
            if (doc.IsMarginMirroring() && (remainingspace <= 100))
            {
                doc.NewPage();
                writer.Flush();
            }
        }

        //private void Signature(PdfWriter writer, Document doc, float currentLocation)
        //{

        //    //if (department == null)
        //    //{
        //    //    department = new Department();
        //    //}

        //    var sigTbl = new PdfPTable(3);
        //    sigTbl.TotalWidth = doc.PageSize.Width;
        //    sigTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            
        //    float[] columns = new float[] { 10f, 20f, 30f };
        //    sigTbl.SetWidths(columns);

        //    //var imgPath = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.ImageDirectory)).FullName;

        //    #region LastParagraph
        //    /** LAST PARAGRAPH **/

        //    //check against config file
        //    string lastParagraphText = contractsConfig.LastParagraphText;

        //    //check for empty values
        //    string TelNrbr = CustomPdfPage.IsEmpty(department.TelNbr, model.GetDocumentText("TelNbr"));
        //    string HrManager = CustomPdfPage.IsEmpty(department.HrManager, model.GetDocumentText("Human Resources Manager"));
        //    string FirstName = CustomPdfPage.IsEmpty(Contract.FirstName, model.GetDocumentText("First Name"));
        //    string LastName = CustomPdfPage.IsEmpty(Contract.Surname, model.GetDocumentText("Last Name"));

        //    //Telnr
        //    lastParagraphText = lastParagraphText.Replace("{TelNbr}", TelNrbr);

        //    if (!string.IsNullOrEmpty(lastParagraphText))
        //    {
        //        var paragraphLast = new Paragraph(lastParagraphText, SignatureFont(FontSize()));

        //        var cellLastParagraph = new PdfPCell(paragraphLast);
        //        cellLastParagraph.HorizontalAlignment = TextAlign();
        //        cellLastParagraph.VerticalAlignment = Element.ALIGN_LEFT;
        //        cellLastParagraph.Border = 0;
        //        cellLastParagraph.PaddingTop = 0;
        //        cellLastParagraph.PaddingLeft = 40;
        //        cellLastParagraph.PaddingRight = 35;
        //        cellLastParagraph.PaddingBottom = 0;
        //        cellLastParagraph.Colspan = 3;
        //        cellLastParagraph.SetLeading(0.0f, 1.9f);

        //        sigTbl.AddCell(cellLastParagraph);
        //    }

        //    //** LAST PARAGRAPH **/
        //    #endregion

        //    if (contractsConfig.SignatureUse == true && contractsConfig.Process.ToLower() == "Hiring".ToLower())
        //    {

        //        var paragraphLast2 = new Paragraph(Environment.NewLine, SignatureFontBold(FontSize()));
        //        paragraphLast2.Add(Environment.NewLine);
        //        paragraphLast2.Add(model.GetDocumentText("WELCOME TO IKEA"));
        //        paragraphLast2.Add(Environment.NewLine);
        //        paragraphLast2.Add(Environment.NewLine);

        //        var cellLastParagraph2 = new PdfPCell(paragraphLast2);
        //        cellLastParagraph2.HorizontalAlignment = Element.ALIGN_LEFT;
        //        cellLastParagraph2.VerticalAlignment = Element.ALIGN_LEFT;
        //        cellLastParagraph2.Border = 0;
        //        cellLastParagraph2.PaddingTop = 0;
        //        cellLastParagraph2.PaddingLeft = 40;
        //        cellLastParagraph2.PaddingRight = 35;
        //        cellLastParagraph2.PaddingBottom = 0;
        //        cellLastParagraph2.Colspan = 3;
        //        //cellLastParagraph2.SetLeading(0.0f, 1.9f);

        //        sigTbl.AddCell(cellLastParagraph2);

        //    }

        //    var paragraphLast3 = new Paragraph(model.GetDocumentText("Yours sincerely"), SignatureFont(FontSize()));
        //    paragraphLast3.Add(Environment.NewLine);
        //    paragraphLast3.Add(Environment.NewLine);
        //    paragraphLast3.Add(Environment.NewLine);
        //    paragraphLast3.Add(Environment.NewLine);
        //    var cellLastParagraph3 = new PdfPCell(paragraphLast3);
        //    cellLastParagraph3.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
        //    cellLastParagraph3.Border = 0;
        //    cellLastParagraph3.PaddingTop = 0;
        //    cellLastParagraph3.PaddingLeft = 40;
        //    cellLastParagraph3.PaddingRight = 35;
        //    cellLastParagraph3.PaddingBottom = 0;
        //    cellLastParagraph3.Colspan = 3;
        //    //cellLastParagraph2.SetLeading(0.0f, 1.9f);

        //    sigTbl.AddCell(cellLastParagraph3);

        //    //Name of the HrManager
        //    paragraphLast3 = new Paragraph(HrManager, SignatureFont(FontSize()));

        //    cellLastParagraph3 = new PdfPCell(paragraphLast3);
        //    cellLastParagraph3.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
        //    cellLastParagraph3.Border = 0;
        //    cellLastParagraph3.PaddingTop = 0;
        //    cellLastParagraph3.PaddingLeft = 40;
        //    cellLastParagraph3.PaddingRight = 35;
        //    cellLastParagraph3.PaddingBottom = 0;
        //    cellLastParagraph3.Colspan = 3;
        //    cellLastParagraph3.SetLeading(0.0f, 1.9f);

        //    sigTbl.AddCell(cellLastParagraph3);

        //    //Human Resource Manager
        //    paragraphLast3 = new Paragraph(model.GetDocumentText("Human Resources Manager"), SignatureFontBold(FontSize()));

        //    cellLastParagraph3 = new PdfPCell(paragraphLast3);
        //    cellLastParagraph3.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
        //    cellLastParagraph3.Border = 0;
        //    cellLastParagraph3.PaddingTop = 0;
        //    cellLastParagraph3.PaddingLeft = 40;
        //    cellLastParagraph3.PaddingRight = 35;
        //    cellLastParagraph3.PaddingBottom = 0;
        //    cellLastParagraph3.Colspan = 3;
        //    cellLastParagraph3.SetLeading(0.0f, 1.9f);

        //    sigTbl.AddCell(cellLastParagraph3);

        //    ////Image
        //    //iTextSharp.text.Image paragraphLast3Image = iTextSharp.text.Image.GetInstance(Path.Combine(imgPath, "Personnel_Today_Awards_2012_Winner.jpg"));
        //    //PdfPCell imageCell = new PdfPCell(paragraphLast3Image);
        //    //imageCell.Colspan = 2; // either 1 if you need to insert one cell
        //    //imageCell.Border = 0;
        //    //imageCell.HorizontalAlignment = TextAlign();
        //    //imageCell.PaddingTop = 20;
        //    //imageCell.PaddingLeft = 40;
        //    //imageCell.PaddingRight = 35;
        //    //imageCell.PaddingBottom = 0;
        //    //imageCell.Colspan = 3;
        //    //imageCell.SetLeading(0.0f, 1.9f);

        //    //sigTbl.AddCell(imageCell);



        //    if (contractsConfig.SignatureUse == true)
        //    {

        //        paragraphLast3.Add(Environment.NewLine);
        //        paragraphLast3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
        //        paragraphLast3.Add(Environment.NewLine);

        //        cellLastParagraph3 = new PdfPCell(paragraphLast3);
        //        cellLastParagraph3.HorizontalAlignment = TextAlign();
        //        cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
        //        cellLastParagraph3.Border = 0;
        //        cellLastParagraph3.PaddingTop = 0;
        //        cellLastParagraph3.PaddingLeft = 40;
        //        cellLastParagraph3.PaddingRight = 35;
        //        cellLastParagraph3.PaddingBottom = 0;
        //        cellLastParagraph3.Colspan = 3;
        //        cellLastParagraph3.SetLeading(0.0f, 1.9f);

        //        sigTbl.AddCell(cellLastParagraph3);

        //        string lastParagraphText2 = contractsConfig.LastParagraphText2;

        //        //FirstName
        //        lastParagraphText2 = lastParagraphText2.Replace("{FirstName}", FirstName);

        //        //LastName
        //        lastParagraphText2 = lastParagraphText2.Replace("{LastName}", LastName);

        //        paragraphLast3 = new Paragraph(lastParagraphText2, SignatureFont(FontSize()));
        //        paragraphLast3.Add(Environment.NewLine);
        //        paragraphLast3.Add(Environment.NewLine);

        //        cellLastParagraph3 = new PdfPCell(paragraphLast3);
        //        cellLastParagraph3.HorizontalAlignment = TextAlign();
        //        cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
        //        cellLastParagraph3.Border = 0;
        //        cellLastParagraph3.PaddingTop = 0;
        //        cellLastParagraph3.PaddingLeft = 40;
        //        cellLastParagraph3.PaddingRight = 35;
        //        cellLastParagraph3.PaddingBottom = 0;
        //        cellLastParagraph3.Colspan = 3;
        //        cellLastParagraph3.SetLeading(0.0f, 1.9f);

        //        sigTbl.AddCell(cellLastParagraph3);

        //        //Your Signature
        //        paragraphLast3 = new Paragraph(model.GetDocumentText("Your signature"), SignatureFont(FontSize()));

        //        cellLastParagraph3 = new PdfPCell(paragraphLast3);
        //        cellLastParagraph3.HorizontalAlignment = TextAlign();
        //        cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
        //        cellLastParagraph3.Border = 0;
        //        cellLastParagraph3.PaddingTop = 0;
        //        cellLastParagraph3.PaddingLeft = 40;
        //        cellLastParagraph3.PaddingRight = 35;
        //        cellLastParagraph3.PaddingBottom = 0;
        //        cellLastParagraph3.Colspan = 3;
        //        cellLastParagraph3.SetLeading(0.0f, 1.9f);

        //        sigTbl.AddCell(cellLastParagraph3);

        //        //Date

        //        paragraphLast3 = new Paragraph(model.GetDocumentText("Date signature"), SignatureFont(FontSize()));

        //        cellLastParagraph3 = new PdfPCell(paragraphLast3);
        //        cellLastParagraph3.HorizontalAlignment = TextAlign();
        //        cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
        //        cellLastParagraph3.Border = 0;
        //        cellLastParagraph3.PaddingTop = 0;
        //        cellLastParagraph3.PaddingLeft = 40;
        //        cellLastParagraph3.PaddingRight = 35;
        //        cellLastParagraph3.PaddingBottom = 0;
        //        cellLastParagraph3.Colspan = 3;
        //        cellLastParagraph3.SetLeading(0.0f, 1.9f);

        //        sigTbl.AddCell(cellLastParagraph3);

        //    }

        //    string EncTextLeft = contractsConfig.EncTextLeft;
        //    string EncTextRight = contractsConfig.EncTextRight;

        //    //Diffrent text for Contact Centre
        //    if (!string.IsNullOrEmpty(EncTextLeft) && !string.IsNullOrEmpty(EncTextRight))
        //    {
        //        //Enc
        //        //left
        //        var paraSignature = new Paragraph("", SignatureFont(FontSize()));
        //        paraSignature.Add(EncTextLeft);

        //        var cellSignature = new PdfPCell(paraSignature);
        //        cellSignature.HorizontalAlignment = TextAlign();
        //        cellSignature.VerticalAlignment = Element.ALIGN_LEFT;
        //        cellSignature.Border = 0;
        //        cellSignature.PaddingTop = 20;
        //        cellSignature.PaddingLeft = 40;
        //        cellSignature.PaddingBottom = 10;
        //        cellSignature.SetLeading(0.0f, 1.5f);

        //        sigTbl.AddCell(cellSignature);

        //        //Right

        //        paraSignature = new Paragraph(EncTextRight, SignatureFont(FontSize()));

        //        paraSignature.Alignment = Element.ALIGN_LEFT;

        //        cellSignature = new PdfPCell(paraSignature);
        //        cellSignature.HorizontalAlignment = TextAlign();
        //        cellSignature.Border = 0;
        //        cellSignature.PaddingTop = 20;
        //        cellSignature.PaddingLeft = 0;
        //        cellSignature.PaddingRight = 35;
        //        cellSignature.PaddingBottom = 10;
        //        cellSignature.Colspan = 2;
        //        cellSignature.SetLeading(0.0f, 1.5f);

        //        sigTbl.AddCell(cellSignature);
        //    }

        //    var signatureHeight = sigTbl.CalculateHeights();
        //    var documentHeight = doc.PageSize.Height;
        //    var signatureX = currentLocation;

        //    var remainingspace = writer.GetVerticalPosition(false) - doc.BottomMargin;
        //    //remainingspace += 10;  *** SG Kommenterade detta

        //    if (signatureHeight > remainingspace)
        //    {
        //        doc.NewPage();
        //        sigTbl.WriteSelectedRows(0, -1, 0, doc.PageSize.Height - doc.TopMargin, writer.DirectContent);
        //        //signatureX = documentHeight - (signatureHeight - doc.TopMargin);
        //        //add to the top
        //        // signatureX = 777; *** SG Kommenterade detta
        //    }
        //    //sigTbl.WriteSelectedRows(0, -1, 0, signatureX, writer.DirectContent); *** SG Kommenterade detta

        //        // Added By SG to Place the signature at the end of the Text if there is Space
        //    else
        //    {
        //        sigTbl.WriteSelectedRows(0, -1, 0, currentLocation - 15 , writer.DirectContent);
        //    }
        //}

        public static string ConvertStringToDateFormat(string dateInString, string desiredFormat)
        {
            string convertedDate;

            DateTime outDatum;

            if (!string.IsNullOrEmpty(dateInString) && DateTime.TryParseExact(dateInString, "dd.MM.yyyy",
                              new CultureInfo("en-GB"),
                              DateTimeStyles.None,
                              out outDatum))
            {
                DateTime dt = DateTime.ParseExact(dateInString, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                return convertedDate = dt.ToString(desiredFormat);

            }
            else
                return convertedDate = dateInString;                       
        }


        private void Signature(PdfWriter writer, Document doc, float currentLocation)
        {

            var sigTbl = new PdfPTable(3);
            sigTbl.TotalWidth = doc.PageSize.Width;
            sigTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            
            var languageFont = customfont;
            string eployerName = CustomPdfPage.IsEmpty(model.Company.CountryEmployer, model.GetDocumentText("Employer Name")); 
            if (Language == "en")
            {
                // To set the Font and Size for English contract
                languageFont = customVerdanafont;
                eployerName =  CustomPdfPage.IsEmpty(model.Company.EmployerName, model.GetDocumentText("Employer Name"));
            }

            Font fontBold = new Font(languageFont, 9, Font.BOLD);
            Font fnt = new Font(languageFont, 9, Font.NORMAL);

            #region LastParagraph
            /** LAST PARAGRAPH **/

            //check against config file
            string lastParagraphText = contractsConfig.LastParagraphText;

          
            if (!string.IsNullOrEmpty(lastParagraphText))
            {
                var paragraphLast = new Paragraph(lastParagraphText, fnt);
                paragraphLast.Alignment = Element.ALIGN_JUSTIFIED;
                paragraphLast.Add(Environment.NewLine);
                paragraphLast.Add(Environment.NewLine);
                paragraphLast.Add(Environment.NewLine);
                paragraphLast.Add(Environment.NewLine);


                var cellLastParagraph = new PdfPCell(paragraphLast);
                cellLastParagraph.HorizontalAlignment = TextAlign();
                cellLastParagraph.VerticalAlignment = Element.ALIGN_JUSTIFIED;
                cellLastParagraph.Border = 0;
                cellLastParagraph.PaddingTop = 0;
                cellLastParagraph.PaddingLeft = doc.LeftMargin + 23;
                cellLastParagraph.PaddingRight = doc.RightMargin;
                cellLastParagraph.PaddingBottom = 0;
                cellLastParagraph.Colspan = 3;
                cellLastParagraph.SetLeading(0.0f, 1.9f);

                sigTbl.AddCell(cellLastParagraph);
            }

            //** LAST PARAGRAPH **/
                #endregion
                      

            Chunk cSD = null;
            string constantKoreanTxt = " 에 체결되었습니다.";
            string colon = " : ";
            string companyName = model.Company.HeaderName;


            if (Language == "KR")
            {
                colon = "  ";
                companyName = "이케아코리아유한회사";
            }


            // To Convert the Contract start date format to "yyyy.MM.dd" for English

            string convertedStartDate = model.GetAnswer("Contractstartdate");
            
            //string koreanConvertedStartDate;

            //DateTime Startdatum;         

            //// Convert start date if cannot convert Print the value exists in Textbox
            if (!string.IsNullOrEmpty(model.GetAnswer("Contractstartdate")))
            {

                convertedStartDate = ConvertStringToDateFormat(convertedStartDate, "yyyy.MM.dd");
                //koreanConvertedStartDate = dt.ToString(dt.Year + " 년 " + dt.Month + " 월 " + dt.Day + " 일");

            }
            else
            {

                convertedStartDate = model.GetDocumentAnswer("Contractstartdate", true);
                //koreanConvertedStartDate = model.GetDocumentAnswer("Contractstartdate", true);

            }

            if (!string.IsNullOrEmpty(model.GetDocumentAnswer("Contractstartdate", true)))
            {
                // to add some constant Text to the Korean Template
                if (Language == "KR")
                {
                    cSD = new Chunk(model.GetDocumentAnswer("Contractstartdate", true) + constantKoreanTxt, fnt);
                }
                else
                    cSD = new Chunk(convertedStartDate, fnt);

            }
            else
            {
                if (Language == "KR")
                {
                    cSD = new Chunk(" " + constantKoreanTxt, fnt);
                }
                else
                cSD = new Chunk(" ", fnt);
            }

            var para = new Paragraph(model.Translate("Date of Contract", Language) + colon + cSD, fnt);
            para.Font = fnt;
            para.Alignment = Element.ALIGN_JUSTIFIED;
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);

            para.Alignment = Element.ALIGN_CENTER;
            para.Add(Environment.NewLine);
           

            var cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.Border = 0;
            cell.PaddingTop = 10;
            cell.PaddingLeft = 15;
            cell.PaddingBottom = 10;
            cell.Colspan = 3;

            sigTbl.AddCell(cell);



            //Chunk ph = new Chunk("이케아코리아유한회사", korean);

            //Översättning: Company  회사
            para = new Paragraph(model.Translate("Company", Language), fontBold);
            para.Font = fnt;
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);

            //Översättning: IKEA Korea Co., Ltd.   이케아코리아유한회사
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(companyName);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);

            cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 45;
            cell.PaddingBottom = 10;

            sigTbl.AddCell(cell);

            //Översättning: Co-worker 직원
            para = new Paragraph(model.Translate("Co-worker", Language), fontBold);
            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_LEFT;
            para.Add(FormLibI18N.Translate("", Language));
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);

            cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 150;
            cell.PaddingRight = 15;
            cell.PaddingBottom = 10;
            cell.Colspan = 2;

            sigTbl.AddCell(cell);



            para = new Paragraph("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯", Malgun(9 , BaseColor.BLACK));
            para.Alignment = Element.ALIGN_CENTER;
            para.Font = fnt;
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);

            //Översättning: Name 이름  -- Person's name : 안드레슈미트갈
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(model.Translate("Name", Language) + colon + eployerName);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);

            //Översättning:Title 직위  CEO(President)  대표이사
            para.Add(model.Translate("Title", Language) + colon + model.Translate("President", Language));

            cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 45;
            cell.PaddingBottom = 10;

            sigTbl.AddCell(cell);

            para = new Paragraph("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯", Malgun(9, BaseColor.BLACK));
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Font = fnt;
            para.Alignment = Element.ALIGN_LEFT;

            //Översättning: Name 이름
            para.Add(model.Translate("Name", Language) + colon);
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            //Översättning:Address 주소
            para.Add(model.Translate("Address", Language) + colon);

            cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 150;
            cell.PaddingRight = 15;
            cell.PaddingBottom = 10;
            cell.Colspan = 2;

            sigTbl.AddCell(cell);


            var signatureHeight = sigTbl.CalculateHeights();
            var remainingPageSpace = writer.GetVerticalPosition(false) - doc.BottomMargin;

            // TO place the signature with Last paragrapg to next page 
            if (signatureHeight > remainingPageSpace)
            {
                doc.NewPage();                
                sigTbl.WriteSelectedRows(0, -1, 0, doc.PageSize.Height - doc.TopMargin, writer.DirectContent);
                CustomePagefooterDesign(writer, doc);          
            }
           
            else
            {
                sigTbl.WriteSelectedRows(0, -1, 0, currentLocation + 30, writer.DirectContent);
            }

        }

        private void CustomePagefooterDesign(PdfWriter writer, Document doc)
        {
            var footerTbl = new PdfPTable(3);
                footerTbl.TotalWidth = doc.PageSize.Width - 58;
                footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;


                Font font = new Font(customVerdanafont, 7.5f, Font.NORMAL, BaseColor.GRAY);
                Font fontGrayBold = new Font(customVerdanafont, 8, Font.BOLD, BaseColor.GRAY);

                Chunk c1 = null;
                if (!string.IsNullOrEmpty(model.Company.footerName))
                {
                    c1 = new Chunk(model.Company.footerName, fontGrayBold);
                }
                else
                    c1 = new Chunk(" ", fontGrayBold);

                // Address has changed from : 15F Banpo Hyosung Bldg., 235 Banpo-daero, Seocho-gu, Seoul, 137-804, Korea
                // for Korea Company.Regon has the footer address
                Phrase ph2 = new Phrase("#" + model.Company.Regon, font);

                // TEl Has Removed  Tel: +82 2 310 8700 / Fax: +82 2 771 7701
                Phrase ph3 = new Phrase("", font);              

                var para = new Paragraph();
                para.Add(Environment.NewLine);

                PdfPCell empty = new PdfPCell(para);
                empty.Border = 0;

                PdfPCell header = new PdfPCell(new Phrase(c1.SetCharacterSpacing(0.8f)));
                header.Colspan = 3;
                header.Border = 0;
                header.PaddingTop = 10;

                header.BorderColorTop = new BaseColor(System.Drawing.Color.Black);
                header.BorderWidthTop = 1f;

                footerTbl.AddCell(header);
            
                PdfPCell address = new PdfPCell(new Phrase(ph2));
                address.Colspan = 2;
                address.Border = 0;
                address.PaddingTop = 2;
                footerTbl.AddCell(address);
            
                PdfPCell telFax = new PdfPCell(new Phrase(ph3));
                telFax.Border = 0;
                telFax.PaddingLeft = -18;
                telFax.PaddingTop = 2;
                footerTbl.AddCell(telFax);
                footerTbl.AddCell(empty);
           
                PdfPTable nested = new PdfPTable(1);
                nested.HorizontalAlignment = Element.ALIGN_MIDDLE;
                nested.AddCell(empty);
                nested.AddCell(empty);

                PdfPCell nesthousing = new PdfPCell(nested);
                nesthousing.Border = 0;
                nesthousing.Padding = 0f;
                footerTbl.AddCell(nesthousing);
                footerTbl.AddCell(empty);               
            
                PdfPCell bottom = new PdfPCell(new Phrase("",Verdana(8, BaseColor.GRAY)));
                bottom.Border = 0;
                bottom.Colspan = 3;
                bottom.PaddingTop = -8;
                bottom.PaddingBottom = -25;
                bottom.HorizontalAlignment = Element.ALIGN_RIGHT;
                footerTbl.AddCell(bottom);
             
                footerTbl.WriteSelectedRows(0, -1, 30, doc.BottomMargin - 10, writer.DirectContent);                                                                  

        }



        public override void OnEndPage(PdfWriter writer, Document doc)
        {


            CustomePagefooterDesign(writer, doc);

           
            var currentLocation = writer.GetVerticalPosition(false);

            if (doc.IsMarginMirroring())
            {
                doc.SetMarginMirroring(false);              
                Signature(writer, doc, currentLocation);
            }

            //Show Draft or not
            if (ShowDraft(Contract.StateSecondaryId) && contractsConfig.DraftTextUse == true)
            {
                Phrase watermark;            
                //Översättning: DRAFT

                if (Language == "en")
                {
                    // To set the Font and Size for English Watermark
                    watermark = new Phrase(model.GetDocumentText("DRAFT"), Verdana(80, BaseColor.LIGHT_GRAY));
                }

                else
                watermark = new Phrase(model.Translate("DRAFT", Language), Malgun(100, BaseColor.LIGHT_GRAY));
                 
                PdfContentByte canvas = writer.DirectContentUnder;
                ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, watermark, 298, 421, 45);
            }

            //use page number or not 
            if (contractsConfig.PageNumberUse == true)
            {

                //Font pNFont = new Font(customVerdanafont, 9, Font.NORMAL, BaseColor.GRAY);
                //string  text = model.GetDocumentText("Page of ", writer.PageNumber.ToString()); 
                string text = writer.PageNumber.ToString() + " / ";

                float len = customVerdanafont.GetWidthPoint(text, 9);
                
                iTextSharp.text.Rectangle pageSize = doc.PageSize;

                cb.SetColorFill(BaseColor.GRAY);
                cb.BeginText();
                cb.SetFontAndSize(customVerdanafont, 9);                
                cb.SetTextMatrix(leftMargin + doc.LeftMargin, pageSize.GetBottom(doc.BottomMargin - 60));
                cb.ShowText(text);
                cb.EndText();

                cb.AddTemplate(template, doc.LeftMargin + len, pageSize.GetBottom(doc.BottomMargin - 60));
            }
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            if (contractsConfig.PageNumberUse == true)
            {

                template.BeginText();
                template.SetFontAndSize(customVerdanafont, 9);
                
                template.SetTextMatrix(leftMargin, 0);
                template.ShowText("" + (writer.PageNumber - 1));
                template.EndText();
            }
        }


    }

}