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
using System.Globalization;
using ECT.FormLib.Models;
using System.Threading;

namespace ECT.FormLib.Pdfs
{
    
    public class IrelandPdfPage : CustomPdfPage
    {
        public Department department { get; set; }
        public FormModel model { get; set; }
        public string id { get; set; }
        public string path { get; set; }

        PdfContentByte cb;
        PdfTemplate template;

        //for centering the page number
        const int leftMargin = 228;

        public static ContractsConfiguration contractsConfig { get; set; }

        BaseFont customfont = BaseFont.CreateFont("C:\\Windows\\Fonts\\verdana.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);

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

        private void AddressBox(PdfWriter writer, Document doc)
        {

            Font font = new Font(customfont, 10);
            Font fontBold = new Font(customfont, 10 , Font.BOLD);


            PdfPTable addressTbl = new PdfPTable(1);
            var xpointsValue = iTextSharp.text.Utilities.MillimetersToPoints(17);
            var ypointsValue = iTextSharp.text.Utilities.MillimetersToPoints(44);
            addressTbl.TotalWidth = doc.PageSize.Width - (xpointsValue);


            Chunk c1 = new Chunk("Private & Confidential", fontBold);
            int NewLines = 0;

            var para = new Paragraph();
            para.Font = font;
            para.Alignment = Element.ALIGN_RIGHT;
            para.SetLeading(0, 2);
            if (contractsConfig.AddressBoxUse == true && contractsConfig.Process.ToLower() != "Hiring".ToLower())
            {
                para.Add(Environment.NewLine);
                para.Add(c1);
            }
            // First Name & Last Name 
            para.Add(Environment.NewLine);
            para.Add((model.GetDocumentAnswer("FirstName", true) + " " + model.GetDocumentAnswer("LastName", true)));


            if (!string.IsNullOrEmpty(model.GetAnswer("PermanentAddressLine1")))
            {
                para.Add(Environment.NewLine);
                para.Add(model.GetAnswer("PermanentAddressLine1"));
            }
            else
                NewLines = NewLines + 1;

            if (!string.IsNullOrEmpty(model.GetAnswer("PermanentAddressLine2")))
            {
                para.Add(Environment.NewLine);
                para.Add(model.GetAnswer("PermanentAddressLine2"));
            }

            else
                NewLines = NewLines + 1;

            if (!string.IsNullOrEmpty(model.GetAnswer("PermanentAddressLine3")))
            {
                para.Add(Environment.NewLine);
                para.Add(model.GetAnswer("PermanentAddressLine3"));
            }

            else
                NewLines = NewLines + 1;

            if (!string.IsNullOrEmpty(model.GetAnswer("PermanentCity")))
            {
                para.Add(Environment.NewLine);
                para.Add(model.GetAnswer("PermanentCity"));
            }

            else
                NewLines = NewLines + 1;

            if (!string.IsNullOrEmpty(model.GetAnswer("PermanentPostalCode")))
            {
                para.Add(Environment.NewLine);
                para.Add(model.GetAnswer("PermanentPostalCode"));
            }

            else
                NewLines = NewLines + 1;

            if (NewLines != 0)
            {
                for (int i = 1; i <= NewLines; i++)
                {
                    para.Add(Environment.NewLine);
                    para.Add(Environment.NewLine);
                }
            }

            PdfPCell cell = new PdfPCell(para);

            cell.PaddingTop = -11;
            cell.Border = 0;
            cell.PaddingRight = 1;
            cell.PaddingLeft = 0;
            cell.SetLeading(0.0f, 1.3f);
            addressTbl.AddCell(cell);

            writer.DirectContent.SetCharacterSpacing(0.2f);

            addressTbl.WriteSelectedRows(0, -1, xpointsValue, (doc.PageSize.Height - ypointsValue), writer.DirectContent);
        }

        public override void OnOpenDocument(PdfWriter writer, Document doc)
        {
            cb = writer.DirectContent;
            template = cb.CreateTemplate(doc.PageSize.Width, 50);
            //open configuration
            #region configuration settings

            var path = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.XmlDirectory)).FullName;
            var xmlDocument = new XmlDocument();

            xmlDocument.Load(Path.Combine(path, "Ireland/ContractsConfiguration.xml"));

            //get info from config where id ex: "ContractDutch" 
            var contracts = from contract in xmlDocument.ToXDocument().Descendants("contract")
                            where (string)contract.Attribute("id") == id
                            // orderby contract.Element("id").Value ascending
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
                                AddressBoxUse = bool.Parse(contract.Attribute("addressBoxUse").Value)

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

            // Add AddressBox to the First Page
            if (contractsConfig.AddressBoxUse == true)
            {
                PrintDate(writer, doc);
                AddressBox(writer, doc);
                doc.SetMargins(iTextSharp.text.Utilities.MillimetersToPoints(15), 43, iTextSharp.text.Utilities.MillimetersToPoints(27), iTextSharp.text.Utilities.MillimetersToPoints(20));
            }
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

        private void Signature(PdfWriter writer, Document doc, float currentLocation)
        {
            var sigTbl = new PdfPTable(3);
            sigTbl.TotalWidth = doc.PageSize.Width;
            sigTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            float[] columns = new float[] { 10f, 20f, 30f };
            sigTbl.SetWidths(columns);

            //var imgPath = new DirectoryInfo(HostingEnvironment.MapPath(FormLibConstants.Paths.ImageDirectory)).FullName;

            #region LastParagraph
            /** LAST PARAGRAPH **/

            //check against config file
            string lastParagraphText = contractsConfig.LastParagraphText;

            //check for empty values
            string TelNrbr = CustomPdfPage.IsEmpty(department.TelNbr, model.GetDocumentText("TelNbr"));
            //SG 2015-10-23 case 52871: Change to use Head of Department instead of Hr Manager as we have access to that from Help desk to update esiear.
            string HrManager = CustomPdfPage.IsEmpty(department.HeadOfDepartmentName, model.GetDocumentText("Human Resources Manager"));
            string FirstName = CustomPdfPage.IsEmpty(Contract.FirstName,  model.GetDocumentText("First Name"));
            string LastName = CustomPdfPage.IsEmpty(Contract.Surname,  model.GetDocumentText("Last Name"));

            //Telnr
            lastParagraphText = lastParagraphText.Replace("{TelNbr}", TelNrbr);

            if (!string.IsNullOrEmpty(lastParagraphText))
            {
                var paragraphLast = new Paragraph(lastParagraphText, SignatureFont(FontSize()));

                var cellLastParagraph = new PdfPCell(paragraphLast);
                cellLastParagraph.HorizontalAlignment = TextAlign();
                cellLastParagraph.VerticalAlignment = Element.ALIGN_LEFT;
                cellLastParagraph.Border = 0;
                cellLastParagraph.PaddingTop = 0;
                cellLastParagraph.PaddingLeft = 40;
                cellLastParagraph.PaddingRight = 35;
                cellLastParagraph.PaddingBottom = 0;
                cellLastParagraph.Colspan = 3;
                cellLastParagraph.SetLeading(0.0f, 1.7f);

                sigTbl.AddCell(cellLastParagraph);
            }

            //** LAST PARAGRAPH **/
            #endregion

              if (contractsConfig.SignatureUse == true && contractsConfig.Process.ToLower() == "Hiring".ToLower())
            {

            var paragraphLast2 = new Paragraph(Environment.NewLine, SignatureFontBold(FontSize()));
            paragraphLast2.Add(Environment.NewLine);
            paragraphLast2.Add(model.GetDocumentText("WELCOME TO IKEA"));
            paragraphLast2.Add(Environment.NewLine);
            paragraphLast2.Add(Environment.NewLine);

            var cellLastParagraph2 = new PdfPCell(paragraphLast2);
            cellLastParagraph2.HorizontalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph2.VerticalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph2.Border = 0;
            cellLastParagraph2.PaddingTop = 0;
            cellLastParagraph2.PaddingLeft = 40;
            cellLastParagraph2.PaddingRight = 35;
            cellLastParagraph2.PaddingBottom = 0;
            cellLastParagraph2.Colspan = 3;
            //cellLastParagraph2.SetLeading(0.0f, 1.7f);

            sigTbl.AddCell(cellLastParagraph2);

            }

              var paragraphLast3 = new Paragraph(model.GetDocumentText("Yours sincerely"), SignatureFont(FontSize()));
            paragraphLast3.Add(Environment.NewLine);
            paragraphLast3.Add(Environment.NewLine);
            paragraphLast3.Add(Environment.NewLine);
            paragraphLast3.Add(Environment.NewLine);
            var cellLastParagraph3 = new PdfPCell(paragraphLast3);
            cellLastParagraph3.HorizontalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph3.Border = 0;
            cellLastParagraph3.PaddingTop = 0;
            cellLastParagraph3.PaddingLeft = 40;
            cellLastParagraph3.PaddingRight = 35;
            cellLastParagraph3.PaddingBottom = 0;
            cellLastParagraph3.Colspan = 3;
            //cellLastParagraph2.SetLeading(0.0f, 1.7f);

            sigTbl.AddCell(cellLastParagraph3);

            //Name of the HrManager
            paragraphLast3 = new Paragraph(HrManager, SignatureFont(FontSize()));

            cellLastParagraph3 = new PdfPCell(paragraphLast3);
            cellLastParagraph3.HorizontalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph3.Border = 0;
            cellLastParagraph3.PaddingTop = 0;
            cellLastParagraph3.PaddingLeft = 40;
            cellLastParagraph3.PaddingRight = 35;
            cellLastParagraph3.PaddingBottom = 0;
            cellLastParagraph3.Colspan = 3;
            cellLastParagraph3.SetLeading(0.0f, 1.7f);

            sigTbl.AddCell(cellLastParagraph3);

            //Human Resource Manager
            paragraphLast3 = new Paragraph(model.GetDocumentText("Human Resources Manager"), SignatureFontBold(FontSize()));

            cellLastParagraph3 = new PdfPCell(paragraphLast3);
            cellLastParagraph3.HorizontalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
            cellLastParagraph3.Border = 0;
            cellLastParagraph3.PaddingTop = 0;
            cellLastParagraph3.PaddingLeft = 40;
            cellLastParagraph3.PaddingRight = 35;
            cellLastParagraph3.PaddingBottom = 0;
            cellLastParagraph3.Colspan = 3;
            cellLastParagraph3.SetLeading(0.0f, 1.7f);

            sigTbl.AddCell(cellLastParagraph3);

            ////Image
            //iTextSharp.text.Image paragraphLast3Image = iTextSharp.text.Image.GetInstance(Path.Combine(imgPath, "Personnel_Today_Awards_2012_Winner.jpg"));
            //PdfPCell imageCell = new PdfPCell(paragraphLast3Image);
            //imageCell.Colspan = 2; // either 1 if you need to insert one cell
            //imageCell.Border = 0;
            //imageCell.HorizontalAlignment = TextAlign();
            //imageCell.PaddingTop = 20;
            //imageCell.PaddingLeft = 40;
            //imageCell.PaddingRight = 35;
            //imageCell.PaddingBottom = 0;
            //imageCell.Colspan = 3;
            //imageCell.SetLeading(0.0f, 1.9f);

            //sigTbl.AddCell(imageCell);

            if (contractsConfig.SignatureUse == true)
            {

                paragraphLast3.Add(Environment.NewLine);
                paragraphLast3 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                paragraphLast3.Add(Environment.NewLine);

                cellLastParagraph3 = new PdfPCell(paragraphLast3);
                cellLastParagraph3.HorizontalAlignment = TextAlign();
                cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
                cellLastParagraph3.Border = 0;
                cellLastParagraph3.PaddingTop = 0;
                cellLastParagraph3.PaddingLeft = 40;
                cellLastParagraph3.PaddingRight = 35;
                cellLastParagraph3.PaddingBottom = 0;
                cellLastParagraph3.Colspan = 3;
                cellLastParagraph3.SetLeading(0.0f, 1.7f);

                sigTbl.AddCell(cellLastParagraph3);

                string lastParagraphText2 = contractsConfig.LastParagraphText2;

                //FirstName
                lastParagraphText2 = lastParagraphText2.Replace("{FirstName}", FirstName);

                //LastName
                lastParagraphText2 = lastParagraphText2.Replace("{LastName}", LastName);

                paragraphLast3 = new Paragraph(lastParagraphText2, SignatureFont(FontSize()));
                paragraphLast3.Add(Environment.NewLine);
                paragraphLast3.Add(Environment.NewLine);

                cellLastParagraph3 = new PdfPCell(paragraphLast3);
                cellLastParagraph3.HorizontalAlignment = TextAlign();
                cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
                cellLastParagraph3.Border = 0;
                cellLastParagraph3.PaddingTop = 0;
                cellLastParagraph3.PaddingLeft = 40;
                cellLastParagraph3.PaddingRight = 35;
                cellLastParagraph3.PaddingBottom = 0;
                cellLastParagraph3.Colspan = 3;
                cellLastParagraph3.SetLeading(0.0f, 1.7f);

                sigTbl.AddCell(cellLastParagraph3);

                //Your Signature
                paragraphLast3 = new Paragraph(model.GetDocumentText("Your signature"), SignatureFont(FontSize()));

                cellLastParagraph3 = new PdfPCell(paragraphLast3);
                cellLastParagraph3.HorizontalAlignment = TextAlign();
                cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
                cellLastParagraph3.Border = 0;
                cellLastParagraph3.PaddingTop = 0;
                cellLastParagraph3.PaddingLeft = 40;
                cellLastParagraph3.PaddingRight = 35;
                cellLastParagraph3.PaddingBottom = 0;
                cellLastParagraph3.Colspan = 3;
                cellLastParagraph3.SetLeading(0.0f, 1.7f);

                sigTbl.AddCell(cellLastParagraph3);

                //Date
                paragraphLast3 = new Paragraph(model.GetDocumentText("Date signature"), SignatureFont(FontSize()));

                cellLastParagraph3 = new PdfPCell(paragraphLast3);
                cellLastParagraph3.HorizontalAlignment = TextAlign();
                cellLastParagraph3.VerticalAlignment = Element.ALIGN_LEFT;
                cellLastParagraph3.Border = 0;
                cellLastParagraph3.PaddingTop = 0;
                cellLastParagraph3.PaddingLeft = 40;
                cellLastParagraph3.PaddingRight = 35;
                cellLastParagraph3.PaddingBottom = 0;
                cellLastParagraph3.Colspan = 3;
                cellLastParagraph3.SetLeading(0.0f, 1.7f);

                sigTbl.AddCell(cellLastParagraph3);

            }

            string EncTextLeft = contractsConfig.EncTextLeft;
            string EncTextRight = contractsConfig.EncTextRight;

            //Diffrent text for Contact Centre
            if (!string.IsNullOrEmpty(EncTextLeft) && !string.IsNullOrEmpty(EncTextRight))
            {
                //Enc
                //left
                var paraSignature = new Paragraph("", SignatureFont(FontSize()));
                paraSignature.Add(EncTextLeft);

                var cellSignature = new PdfPCell(paraSignature);
                cellSignature.HorizontalAlignment = TextAlign();
                cellSignature.VerticalAlignment = Element.ALIGN_LEFT;
                cellSignature.Border = 0;
                cellSignature.PaddingTop = 20;
                cellSignature.PaddingLeft = 40;
                cellSignature.PaddingBottom = 10;
                cellSignature.SetLeading(0.0f, 1.5f);

                sigTbl.AddCell(cellSignature);

                //Right

                paraSignature = new Paragraph(EncTextRight, SignatureFont(FontSize()));


                // paraSignature = new Paragraph("Terms and Conditions Appendix", SignatureFont);
                //paraSignature.Add(Environment.NewLine);
                //paraSignature.Add("Pre - Placement Questionnaire");
                //paraSignature.Add(Environment.NewLine);
                //paraSignature.Add("New Starter form");
                //paraSignature.Add(Environment.NewLine);
                //paraSignature.Add("Stamped Addressed envelope");

                paraSignature.Alignment = Element.ALIGN_LEFT;

                cellSignature = new PdfPCell(paraSignature);
                cellSignature.HorizontalAlignment = TextAlign();
                cellSignature.Border = 0;
                cellSignature.PaddingTop = 20;
                cellSignature.PaddingLeft = 0;
                cellSignature.PaddingRight = 35;
                cellSignature.PaddingBottom = 10;
                cellSignature.Colspan = 2;
                cellSignature.SetLeading(0.0f, 1.5f);

                sigTbl.AddCell(cellSignature);
            }

            var signatureHeight = sigTbl.CalculateHeights();
            var documentHeight = doc.PageSize.Height;
            var signatureX = currentLocation;

            var remainingspace = writer.GetVerticalPosition(false) - doc.BottomMargin;
            remainingspace += 10;

            if (signatureHeight > remainingspace)
            {
                doc.NewPage();
                //signatureX = documentHeight - (signatureHeight - doc.TopMargin);
                //add to the top
                signatureX = 777;
            }

            sigTbl.WriteSelectedRows(0, -1, 0, signatureX, writer.DirectContent);
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            var currentLocation = writer.GetVerticalPosition(false);

            if (doc.IsMarginMirroring())
            {
                doc.SetMarginMirroring(false);
                Signature(writer, doc, currentLocation - 20);
            }

            //Show Draft or not
            if (ShowDraft(Contract.StateSecondaryId) && contractsConfig.DraftTextUse == true)
            {
                Phrase watermark = new Phrase(model.GetDocumentText("DRAFT"), Verdana(80, BaseColor.LIGHT_GRAY));
                PdfContentByte canvas = writer.DirectContentUnder;
                ColumnText.ShowTextAligned(canvas, Element.ALIGN_CENTER, watermark, 298, 421, 45);
            }

            //use page number or not (At the moment only used in UK Change T&C Print document
            if (contractsConfig.PageNumberUse == true)
            {
                string text = model.GetDocumentText("Page of", writer.PageNumber.ToString()); 
                //string text = "Page " + writer.PageNumber.ToString() + " of ";
                float len = this.PageNumberFont.BaseFont.GetWidthPoint(text, this.PageNumberFont.Size);

                iTextSharp.text.Rectangle pageSize = doc.PageSize;

                cb.SetRGBColorFill(0, 0, 0);
                cb.BeginText();
                cb.SetFontAndSize(this.PageNumberFont.BaseFont, this.PageNumberFont.Size);
                cb.SetTextMatrix(leftMargin + doc.LeftMargin, pageSize.GetBottom(doc.BottomMargin-20));
                cb.ShowText(text);
                cb.EndText();

                cb.AddTemplate(template, doc.LeftMargin + len, pageSize.GetBottom(doc.BottomMargin - 20));
            }
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            if (contractsConfig.PageNumberUse == true)
            {
                template.BeginText();
                template.SetFontAndSize(this.PageNumberFont.BaseFont, this.PageNumberFont.Size);
                template.SetTextMatrix(leftMargin, 0);
                template.ShowText("" + (writer.PageNumber - 1));
                template.EndText();
            }
        }

        /// <summary>Get Text for Service Area, with check for empty data
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetServiceAreaText(string name, FormModel model)
        {
            string serviceArea = model.GetDocumentAnswer(name);
       
            if (serviceArea.ToLower() == "Bus Navigation".ToLower())
            {
                return "Business Navigation & Operations Department";
            }

            if ((serviceArea.ToLower() == "Customer Rel".ToLower()) || (serviceArea.ToLower() == "Cust Relations".ToLower()))
            {
                return "Customer Relations Department";
            }

            if (serviceArea.ToLower() == "HR".ToLower())
            {
                return "Human Resources Department";
            }

            if ((serviceArea.ToLower() == "IKEA Food".ToLower()) || (serviceArea.ToLower() == "IKEA Food Serv".ToLower()))
            {
                return "IKEA Food Department";
            }

            if (serviceArea.ToLower() == "Logistics".ToLower())
            {
                return "Logistics Department";
            }

            if (serviceArea.ToLower() == "Marketing".ToLower())
            {
                return "Marketing Department";
            }

            if (serviceArea.ToLower() == "Operations".ToLower())
            {
                return "Operations Department";
            }

            if (serviceArea.ToLower() == "Sales".ToLower())
            {
                return "Sales Department";
            }

            if ((serviceArea.ToLower() == "Commin".ToLower()) || (serviceArea.ToLower() == "Comm-in".ToLower()))
            {
                return "Comm-In Department";
            }

            return serviceArea;
            
        }

        /// <summary>Get PayInfoText
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetPayInfoText(FormModel model)
        {
            return "payable monthly in arrears on the last Friday of each month by direct transfer into your Bank or Building Society account.";
        }

       

    }
}