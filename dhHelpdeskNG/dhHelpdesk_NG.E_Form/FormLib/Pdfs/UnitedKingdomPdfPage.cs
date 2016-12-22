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
using System.Threading;
using ECT.FormLib.Models;
using ECT.Model.Abstract;
using ECT.Model.Contrete;


namespace ECT.FormLib.Pdfs
{
    public class UnitedKingdomPdfPage : CustomPdfPage
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

        // Print LOGO for Doncaster
        private void PrintLogo(PdfWriter writer, Document doc)
        {
            PdfPTable logoTbl = new PdfPTable(3);
            logoTbl.TotalWidth = doc.PageSize.Width;
            var xpointsValue = iTextSharp.text.Utilities.MillimetersToPoints(17);
            var ypointsValue = iTextSharp.text.Utilities.MillimetersToPoints(22);

            var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/FormLibContent/assets/img/IKEA_logo_RGB_2.jpg"));
            logo.ScalePercent(17);
            int w = (int)logoTbl.TotalWidth - (200);
            logoTbl.SetWidths(new int[] { 100, w, 100 });

            var c1 = new PdfPCell(logo);         
            c1.Border = 0;
            logoTbl.AddCell(c1);

            var c2 = new PdfPCell(new Phrase(""));// Should be to cell adjustment
            c2.Border = 0;
            logoTbl.AddCell(c2);

            var c3 = new PdfPCell(new Phrase("")); // Should be to cell adjustment
            c3.Border = 0;
            logoTbl.AddCell(c3);


            logoTbl.WriteSelectedRows(0, -1, xpointsValue, (doc.PageSize.Height - ypointsValue), writer.DirectContent);
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
            //para.SetLeading(0, 2);

            //para.Add(Environment.NewLine);
            para.Add(c1);           
            // First Name & Last Name 
            para.Add(Environment.NewLine);
            para.Add((model.GetDocumentAnswer("FirstName", true) + " " + model.GetDocumentAnswer("LastName", true )));
                        

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

            xmlDocument.Load(Path.Combine(path, "unitedKingdom/ContractsConfiguration.xml"));

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
                                //TextAlign = contract.Attribute("textAlign").Value,
                                //FontSize = int.Parse(contract.Attribute("fontSize").Value)
                                AddressBoxUse = bool.Parse(contract.Attribute("addressBoxUse").Value),

                            };

            //Set logo picture if it is Doncaster contract
                      var LogoUse = from contract in xmlDocument.ToXDocument().Descendants("contract")
                            where (string)contract.Attribute("id") == "Doncaster"
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
                                //TextAlign = contract.Attribute("textAlign").Value,
                                //FontSize = int.Parse(contract.Attribute("fontSize").Value)
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
          //  contractsConfig.TextAlign = contractsList[0].TextAlign;
            contractsConfig.AddressBoxUse = contractsList[0].AddressBoxUse;
            contractsConfig.TextAlign = xmlDocument.ToXDocument().Root.Attribute("textAlign") != null ? xmlDocument.ToXDocument().Root.Attribute("textAlign").Value : "left";
          //  contractsConfig.FontSize = contractsList[0].FontSize;
            contractsConfig.FontSize = xmlDocument.ToXDocument().Root.Attribute("fontSize") != null ? int.Parse(xmlDocument.ToXDocument().Root.Attribute("fontSize").Value) : 11;
            contractsConfig.Style = xmlDocument.ToXDocument().Root.Attribute("style") != null ? xmlDocument.ToXDocument().Root.Attribute("style").Value : "";


            #endregion

            //open configuration

            // Add AddressBox to the First Page
            if (contractsConfig.AddressBoxUse == true)
            {
                PrintDate(writer, doc);

                if (id == "Doncaster")
                PrintLogo(writer, doc);

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

            //if (department == null)
            //{
            //    department = new Department();
            //}

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
            string FirstName = CustomPdfPage.IsEmpty(Contract.FirstName, model.GetDocumentText("First Name"));
            string LastName = CustomPdfPage.IsEmpty(Contract.Surname, model.GetDocumentText("Last Name"));

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
                cellLastParagraph.SetLeading(0.0f, 1.9f);

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
                //cellLastParagraph2.SetLeading(0.0f, 1.9f);

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
            //cellLastParagraph2.SetLeading(0.0f, 1.9f);

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
            cellLastParagraph3.SetLeading(0.0f, 1.9f);

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
            cellLastParagraph3.SetLeading(0.0f, 1.9f);

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
                cellLastParagraph3.SetLeading(0.0f, 1.9f);

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
                cellLastParagraph3.SetLeading(0.0f, 1.9f);

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
                cellLastParagraph3.SetLeading(0.0f, 1.9f);

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
                cellLastParagraph3.SetLeading(0.0f, 1.9f);

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
            //remainingspace += 10;  *** SG Kommenterade detta

            if (signatureHeight > remainingspace)
            {
                doc.NewPage();
                sigTbl.WriteSelectedRows(0, -1, 0, doc.PageSize.Height - doc.TopMargin, writer.DirectContent);
                //signatureX = documentHeight - (signatureHeight - doc.TopMargin);
                //add to the top
                // signatureX = 777; *** SG Kommenterade detta
            }
            //sigTbl.WriteSelectedRows(0, -1, 0, signatureX, writer.DirectContent); *** SG Kommenterade detta

                // Added By SG to Place the signature at the end of the Text if there is Space
            else
            {
                sigTbl.WriteSelectedRows(0, -1, 0, currentLocation - 15 , writer.DirectContent);
            }
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            var currentLocation = writer.GetVerticalPosition(false);

            if (doc.IsMarginMirroring())
            {
                doc.SetMarginMirroring(false);
                //Signature(writer, doc, currentLocation - 20); *** SG Kommenterade detta
                Signature(writer, doc, currentLocation);
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

                string  text = model.GetDocumentText("Page of", writer.PageNumber.ToString()); 
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

        /// <summary>Get Text for Function (Service Area is the old name)
        /// </summary> 
        /// 
        //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetServiceAreaText(string name, FormModel model)
        {
            string serviceArea = model.GetDocumentAnswer(name);

            if (serviceArea.ToLower() == "﻿Bus Navigation".ToLower())
            {
                return "Business Navigation & Operations Department";
            }

            if (serviceArea.ToLower() == "CDC Operations".ToLower())
            {
                return "CDC Operations Department";
            }

            if (serviceArea.ToLower() == "Comm-In".ToLower())
            {
                return "Communication & Interior Design Department";
            }

            if (serviceArea.ToLower() == "Cust Distrib".ToLower())
            {
                return "Customer Distribution Department".ToLower();
            }

            if (serviceArea.ToLower() == "Cust Relations".ToLower())
            {
                return "Customer Relations Department";
            }

            if (serviceArea.ToLower() == "Cust Services".ToLower())
            {
                return "Customer Services Department";
            }

            if (serviceArea.ToLower() == "Facilities".ToLower())
            {
                return "Facilities Department";
            }

            if (serviceArea.ToLower() == "Finance &amp; Admin".ToLower() | serviceArea == "Finance & Admin".ToLower())
            {
                return "Finance & Administration Department";
            }

            if (serviceArea.ToLower() == "HR".ToLower())
            {
                return "Human Resources Department";
            }

            if (serviceArea.ToLower() == "IKEA Food Serv".ToLower())
            {
                return "IKEA Food Department";
            }

            if (serviceArea.ToLower() == "Internationals".ToLower())
            {
                return "Internationals Department";
            }

            if (serviceArea.ToLower() == "Logistics".ToLower())
            {
                return "Logistics Department";
            }

            if (serviceArea.ToLower() == "Marketing".ToLower())
            {
                return "Marketing Department";
            }

            if (serviceArea.ToLower() == "Operational Sup".ToLower())
            {
                return "Operational Support Department".ToLower();
            }

            if (serviceArea.ToLower() == "Operations".ToLower())
            {
                return "Operations Department";
            }

            if (serviceArea.ToLower() == "Projects".ToLower())
            {
                return "Projects Department";
            }

            if (serviceArea.ToLower() == "Property".ToLower())
            {
                return "Property Department";
            }

            if (serviceArea.ToLower() == "Resource Plann".ToLower())
            {
                return "Resource Planning Department";
            }

            if (serviceArea.ToLower() == "Sales".ToLower())
            {
                return "Sales Department";
            }

            if (serviceArea.ToLower() == "Warehouse Oper".ToLower())
            {
                return "Warehouse";
            }

            return serviceArea;
        }

        /// <summary>Get Allowances text depending on Allowances type and Allowance Amount/Units
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetAllowancesText(string AllowancesType, string AllowancesAmountUnits, FormModel Model)
        {

           // if (Model.GetDocumentAnswer(AllowancesType).ToLower() == "150C - Hourly FLT Skills (Fork Lift Truck Allowance)".ToLower() | Model.GetDocumentAnswer(AllowancesType).ToLower() == "150C - Hourly FLT Skills (Fork Lift Truck Allowance Milton Keynes)".ToLower())
            if (Model.GetDocumentAnswer(AllowancesType).ToLower() == "Fork Lift Truck Allowance".ToLower())
            {
                return "In addition you will receive a fork lift truck skills allowance of &pound;" + Model.GetDocumentAnswer(AllowancesAmountUnits,true) + " per hour (subject to having a valid fork lift truck licence and using this skill).";
            }

           // if (Model.GetDocumentAnswer(AllowancesType).ToLower() == "150P - NIght Premium".ToLower() | Model.GetDocumentAnswer(AllowancesType).ToLower() == "150P - NIght Premium London 1".ToLower() | Model.GetDocumentAnswer(AllowancesType).ToLower() == "150P - NIght Premium London 2".ToLower() | Model.GetDocumentAnswer(AllowancesType).ToLower() == "150P - NIght Premium London 3".ToLower() | Model.GetDocumentAnswer(AllowancesType).ToLower() == "150U - Night Premium Dist".ToLower() | Model.GetDocumentAnswer(AllowancesType).ToLower() == "300G - Night Shift".ToLower())
            if (Model.GetDocumentAnswer(AllowancesType).ToLower() == "Night Shift".ToLower())
            {
                return "In addition you will receive a night allowance of &pound;" + Model.GetDocumentAnswer(AllowancesAmountUnits,true) + " per hour for hours worked between 22.00 and 06.00 (subject to working a minimum of one hour during this period)";
            }

           // if (Model.GetDocumentAnswer(AllowancesType).ToLower() == "300D - First Aid Allowance".ToLower())
            if (Model.GetDocumentAnswer(AllowancesType).ToLower() == "First Aid Allowance".ToLower())
            {
                return "In addition you will receive a First Aid skills allowance of &pound;" + Model.GetDocumentAnswer(AllowancesAmountUnits,true) + " per annum (subject to valid qualification and using this skill).";
            }

            return "";
        }

        /// <summary>Get Benefit 1 from Benefit level
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetBenefit1Text(FormModel Model)
        {

            if (Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 1".ToLower() | Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 2".ToLower() | Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 3".ToLower())
            {
                return "You will be entitled to Private Medical Health (Cigna) for yourself and where applicable your partner and children paid for by IKEA. You will also receive Dental Insurance cover paid for by IKEA for yourself and your partner.";
            }

            return "";
        }

        /// <summary>Get Benefit 2 from Benefit level
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetBenefit2Text(FormModel Model)
        {
            if (Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 1".ToLower())
            {
                return "You have the benefit of up to 13 weeks paid sick leave in any rolling year from your first day at IKEA.";
            }

            if (Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 2".ToLower() | Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 3".ToLower())
            {
                return "You have the benefit of up to 26 weeks paid sick leave in any calendar year from your first day at IKEA.";
            }

            return "";
        }

        /// <summary>Get Benefit 3 from Benefit level
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetBenefit3Text(FormModel Model)
        {
            if (Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 2".ToLower() | Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 3".ToLower())
            {
                return "Permanent Health Insurance (PHI) will provide you with 75% of your salary, in the event that you are unable to work due to chronic or disabling illness. You are entitled to Health Check every 2 years with Nuffield.";
            }

            return "";
        }

        /// <summary>Get Benefit 4 from Benefit level
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetBenefit4Text(FormModel Model)
        {
            if (Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 3".ToLower())
            {
                return "You are eligible for a company car or car allowance of &pound;7,200. You will receive the benefit of a 10% pension contribution, made by IKEA when you join the IKEA pension scheme.";
            }
            return "";
        }

        //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetProbationtext(FormModel Model)
        {
            if (Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Co-worker".ToLower() | Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 1".ToLower())
            {
                return "If it is agreed your probationary period is extended beyond your first six months’ then your notice period will remain as one week from either side. Likewise, where you intend to resign from your employment, during your probationary period you will be required to give the company one week’s notice in writing. During the probationary period the provisions of the company disciplinary procedure will not apply. Beyond the probationary period you will give/be given 4 weeks’ notice in writing.";
            }

            if (Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 2".ToLower() | Model.GetDocumentAnswer("BenefitLevel").ToLower() == "Level 3".ToLower())
            {
                return "If it is agreed your probationary period is extended beyond your first six months’ then your notice period will remain as one week from either side. Likewise, where you intend to resign from your employment, during your probationary period you will be required to give the company one week’s notice in writing. During the probationary period the provisions of the company disciplinary procedure will not apply. Beyond the probationary period you will give/be given 12 weeks’ notice in writing.";
            }

            return "";
        }

        /// <summary>Get PayInfoText
        /// </summary> 
        /// //To do: Do this in a better way, get data from xml/db? /TA
        public static string GetPayInfoText(FormModel Model)
        {
            //Hourly
            if (Model.GetDocumentAnswer("PayrollCategory").ToLower() == "Hourly".ToLower())
            {   // SG :  WO Week 15-2016  Case : 54769
                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Bristol".ToLower())
                {
                    return " per hour payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Croydon".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Service Office".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Tottenham".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Wembley".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Property Investments Ltd".ToLower())
                {
                    return " per hour payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Lakeside".ToLower())
                {
                    return " per hour payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Milton Keynes".ToLower())
                {
                    return " per hour payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Contact Centre".ToLower())
                {
                    return " per hour payable monthly in arrears on the last Friday of each month by direct transfer into your Bank or Building Society account. ";
                }

                return " per hour payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
            }
            //"Salaried", "Salaried Management" or "Salaried Senior Management"
            else
            {
                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Bristol".ToLower())
                {
                    return " per annum payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Croydon".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Service Office".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Tottenham".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Wembley".ToLower() | Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Property Investments Ltd".ToLower())
                {
                    return " per annum payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Lakeside".ToLower())
                {
                    return " per annum payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Milton Keynes".ToLower())
                {
                    return " per annum payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
                }

                if (Model.GetDocumentAnswer("BusinessUnit").ToLower() == "IKEA Contact Centre".ToLower())
                {
                    return " per annum payable monthly in arrears on the last Friday of each month by direct transfer into your Bank or Building Society account.  Salary is inclusive of all hours/days worked. ";
                }

                return " per annum payable monthly in arrears in accordance with the terms set out from time to time in our Company Handbook. Salary is inclusive of all hours/days worked. ";
            }

        }






    }

}