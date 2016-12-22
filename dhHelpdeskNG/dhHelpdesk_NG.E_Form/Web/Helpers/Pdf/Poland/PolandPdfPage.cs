using System;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using ECT.Model.Entities;

namespace ECT.Web.Pdf.Poland
{
    public class PolandPdfPage : CustomPdfPage
    {
        public Company CurrentCompany { get; set; }
        public string Language { get; set; }
        //public int state { get; set; }

        public override void OnStartPage(PdfWriter writer, Document doc)
        {           
            if (CurrentCompany.Searchkey == "OI")
            {
                CustomePageDesign(writer, doc);
                return;
            }
            else
            {
                PdfPTable headerTbl = new PdfPTable(3);
                headerTbl.TotalWidth =  doc.PageSize.Width ;
                
                var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/assets/img/IKEA_logo_RGB_2.jpg"));
                logo.ScalePercent(20);
                int w = (int)headerTbl.TotalWidth - (200); 
                headerTbl.SetWidths(new int[]{100,w,100});

                var c1 = new PdfPCell(logo);
                c1.PaddingLeft = 30;
                c1.Border = 0;
                headerTbl.AddCell(c1);

                var c2 = new PdfPCell(new Phrase((CurrentCompany.HeaderName),Header));
                c2.VerticalAlignment = Element.ALIGN_BOTTOM;
                c2.HorizontalAlignment = Element.ALIGN_CENTER;
                c2.Border = 0;
                c2.PaddingLeft = 15;
                headerTbl.AddCell(c2);

                var c3 = new PdfPCell(new Phrase("")); // Should be to cell adjustment
                c3.Border = 0;
                headerTbl.AddCell(c3);

                headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height-15), writer.DirectContent);

                //if (state != 0)
                //{
                //    state = 0;
                //    OnEndPage(writer, doc);
                //}

            }

            
        }

        private void CustomePageDesign(PdfWriter writer, Document doc)
        {
            
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 110;
            table.LockedWidth = true;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;

            var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/assets/img/InterGroup_logo1.jpg"));
            logo.ScalePercent(73);
            

            PdfPCell cell = new PdfPCell(logo);
            cell.HorizontalAlignment = Element.ALIGN_RIGHT; //0=Left, 1=Centre, 2=Right
            cell.UseVariableBorders = true;
            BaseColor green = new BaseColor(103, 185, 0);
            cell.BorderColorLeft = green;
            cell.BorderWidthLeft = 3;
            cell.BorderWidthBottom = 0;
            cell.BorderColorRight = BaseColor.WHITE;
            table.AddCell(cell);

            Chunk c1 = new Chunk(CurrentCompany.footerName, FooterSmall);

            var para = new Paragraph();
            para.Font = Footer;
            para.Alignment = Element.ALIGN_RIGHT;
            para.Add(Environment.NewLine);
            para.Add(c1);
            para.Add(Environment.NewLine);
            para.Add("Janki, Pl. Szwedzki 3");
            para.Add(Environment.NewLine);
            para.Add("05-090 Raszyn");
            para.Add(Environment.NewLine);
            para.Add("Polska");
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine); // New paragraph
            para.Add("P " + CurrentCompany.Tel);
            para.Add(Environment.NewLine);
            para.Add("F " + CurrentCompany.Fax);
            para.Add(Environment.NewLine);
            para.Add("www.iicg.pl");
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine); // New paragraph
            para.Add("KRS " + CurrentCompany.KRSNo + " – Sąd ");
            para.Add(Environment.NewLine);
            para.Add("Rejonowy ");
            para.Add(Environment.NewLine);
            para.Add("dla m.st. Warszawy");
            para.Add(Environment.NewLine);
            para.Add("XIV Wydział. Gospodarczy");
            para.Add(Environment.NewLine);
            para.Add("KRS");
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine); // New paragraph
            para.Add("Kapitał zakładowy:");
            para.Add(Environment.NewLine);
            para.Add(CurrentCompany.KapitalNo + " zł");
            para.Add(Environment.NewLine);
            para.Add("(w całości wpłacony)");
            para.Add(Environment.NewLine);
            para.Add(Environment.NewLine);
            para.Add("NIP: " + CurrentCompany.NIP);

            PdfPCell cell2 = new PdfPCell(para);
            cell2.HorizontalAlignment = Element.ALIGN_LEFT; //0=Left, 1=Centre, 2=Right
            cell2.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell2.UseVariableBorders = true;
            cell2.BorderColorLeft = green;
            cell2.BorderColorTop = BaseColor.WHITE;
            cell2.BorderColorBottom = BaseColor.WHITE;
            cell2.BorderColorRight = BaseColor.WHITE;
            cell2.BorderWidthLeft = 3;
            cell2.PaddingBottom = 30;
            cell2.PaddingLeft = 4;
            
            cell2.MinimumHeight = doc.PageSize.Height-(doc.BottomMargin+100);
            table.AddCell(cell2);
        
            table.WriteSelectedRows(0, -1, doc.PageSize.Width-table.TotalWidth, 
                                    (doc.PageSize.Height - 30), writer.DirectContent);

            //if (state != 0)
            //{
            //    state = 0;
            //    CustomePagefooterDesign(writer, doc);
            //}
        }


        public override void OnParagraph(PdfWriter writer, Document doc, float currentLocation)
        {
            var remainingspace = writer.GetVerticalPosition(false) - doc.BottomMargin;
            if (remainingspace <= 150)
            {
                doc.NewPage();
                writer.Flush();
            }
        }

        private void CustomSignature(PdfWriter writer, Document doc, float currentLocation)
        {

            var signTbl = new PdfPTable(2);
            signTbl.TotalWidth = doc.PageSize.Width - 100;
            signTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            var para = new Paragraph("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯", CustomFooter);
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_CENTER;

            para.Add(I18N.Translate("Date and Employee signature", Language));
            para.Add(Environment.NewLine);
            var cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 40;
            cell.PaddingBottom = 25;
            signTbl.AddCell(cell);


            para = new Paragraph("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯", CustomFooter);
            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(I18N.Translate("Signature of a person representing the Employer", Language));
            para.Add(Environment.NewLine);

            cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 50;
            cell.PaddingRight = 36;
            cell.PaddingBottom = 25;
            //

            signTbl.AddCell(cell);

            signTbl.WriteSelectedRows(0, -1, 0, currentLocation, writer.DirectContent);                

        }

        private void Signature(PdfWriter writer, Document doc, float currentLocation)

        {

            var sigTbl = new PdfPTable(3);
            sigTbl.TotalWidth = doc.PageSize.Width - 58;
            sigTbl.HorizontalAlignment = Element.ALIGN_CENTER;


            var para = new Paragraph("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯", CustomFooter);
            para.Alignment = Element.ALIGN_CENTER;

            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(I18N.Translate("Date and Employee signature", Language));
            para.Add(Environment.NewLine);

            var cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 15;
            cell.PaddingBottom = 10;

            sigTbl.AddCell(cell);

            para = new Paragraph("¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯", CustomFooter);
            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_LEFT;
            para.Add(I18N.Translate("Signature of a person representing the Employer", Language));
            para.Add(Environment.NewLine);

            cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 160;
            cell.PaddingRight = 35;
            cell.PaddingBottom = 10;
            cell.Colspan = 2;

            sigTbl.AddCell(cell);
            
            sigTbl.WriteSelectedRows(0, -1, 30, currentLocation, writer.DirectContent);                


        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {   
            if (CurrentCompany.Searchkey == "OI")
            {
                CustomePagefooterDesign(writer, doc);
                return;
            }
            else
            {
                    
                    var footerTbl = new PdfPTable(3);
                    footerTbl.TotalWidth = doc.PageSize.Width - 58;
                    footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

                    // Address.........  



                    Chunk c1 = new Chunk(CurrentCompany.footerName, FooterSmall);
                    Chunk c2 = new Chunk("www.IKEA.pl", FooterUnderline);


                    var para = new Paragraph();
                    para.Font = Footer;
                    para.Alignment = Element.ALIGN_CENTER;
                    para.Add(Environment.NewLine);
                    para.Add(c1);
                    para.Add(Environment.NewLine);
                    para.Add("Pl. Szwedzki 3, Janki");
                    para.Add(Environment.NewLine);
                    para.Add("05-090 Raszyn, Polska");
                    para.Add(Environment.NewLine);
                    para.Add(c2);

                    var cell = new PdfPCell(para);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.PaddingTop = 0;
                    cell.PaddingLeft = 5;
                    cell.BorderColorTop = new BaseColor(System.Drawing.Color.Black);
                    cell.BorderWidthTop = 1f;
                    cell.MinimumHeight = doc.PageSize.Height - doc.BottomMargin;

                    footerTbl.AddCell(cell);

                    para = new Paragraph();
                    para.Font = Footer;
                    para.Alignment = Element.ALIGN_CENTER;
                    para.Add(Environment.NewLine);
                    para.Add("Tel: " + CurrentCompany.Tel);
                    para.Add(Environment.NewLine);
                    para.Add("Fax: " + CurrentCompany.Fax);
                    para.Add(Environment.NewLine);
                    para.Add("NIP: " + CurrentCompany.NIP);
                    para.Add(Environment.NewLine);
                    para.Add("Regon: " + CurrentCompany.Regon);

                    cell = new PdfPCell(para);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = 0;
                    cell.PaddingTop = 0;
                    cell.PaddingLeft = 5;
                    cell.PaddingRight = 0;
                    cell.BorderColorTop = new BaseColor(System.Drawing.Color.Black);
                    cell.BorderWidthTop = 1f;
                    cell.MinimumHeight = doc.PageSize.Height - doc.BottomMargin;


                    footerTbl.AddCell(cell);

                    para = new Paragraph();
                    para.Font = Footer;
                    para.Alignment = Element.ALIGN_CENTER;
                    para.Add(Environment.NewLine);
                    para.Add("KRS " + CurrentCompany.KRSNo + " – Sąd Rejonowy dla m.st. Warszawy");
                    para.Add(Environment.NewLine);
                    para.Add("XIV Wydział Gospodarczy KRS");
                    para.Add(Environment.NewLine);
                    para.Add("Kapitał zakładowy:" + CurrentCompany.KapitalNo + "zł");
                    para.Add(Environment.NewLine);
                    para.Add("Deutsche Bank: " + CurrentCompany.INGBankNo);

                    cell = new PdfPCell(para);
                    cell.HorizontalAlignment = Element.ALIGN_MIDDLE;
                    cell.Border = 0;
                    cell.PaddingTop = 0;
                    cell.PaddingLeft = -60;
                    cell.PaddingRight = 0;
                    cell.BorderColorTop = new BaseColor(System.Drawing.Color.Black);
                    cell.BorderWidthTop = 1f;
                    cell.MinimumHeight = doc.PageSize.Height - doc.BottomMargin;

                    footerTbl.AddCell(cell);

                    //if (state != 0)
                    //{                                                
                    //    footerTbl.WriteSelectedRows(0, -1, 30, doc.BottomMargin, writer.DirectContent);                        
                    //    return;
                    //}

                    var remainingPageSpace = writer.GetVerticalPosition(false) - doc.BottomMargin;
                    var currentLocation = writer.GetVerticalPosition(false);
                    float signHeight = 160;
                    if (remainingPageSpace <= signHeight)
                    {                                                
                        footerTbl.WriteSelectedRows(0, -1, 30, doc.BottomMargin, writer.DirectContent);                        
                        if (doc.IsMarginMirroring())
                        {                            
                            doc.SetMarginMirroring(false);
                            Signature(writer, doc, doc.BottomMargin + 40);
                            //state = doc.PageNumber + 1;
                            //doc.NewPage();
                        }
                    }
                    else
                    {
                        if (currentLocation >= (doc.PageSize.Height / 2) + 100 )
                       {
                           Signature(writer, doc, (currentLocation - 80));
                           footerTbl.WriteSelectedRows(0, -1, 30, doc.BottomMargin, writer.DirectContent);
                           return;
                       }
                       else
                       {
                           Signature(writer, doc, doc.BottomMargin + 40);
                           footerTbl.WriteSelectedRows(0, -1, 30, doc.BottomMargin, writer.DirectContent);
                       }

                       doc.SetMarginMirroring(false);                    
                    }


            }

        }

        private void CustomePagefooterDesign(PdfWriter writer, Document doc)
        {
            var footerTbl = new PdfPTable(2);
            footerTbl.TotalWidth = doc.PageSize.Width - 100;
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            // Address.........  


            var logo = Image.GetInstance(HttpContext.Current.Server.MapPath("~/assets/img/InterGroup_logo.jpg"));
            logo.ScalePercent(13);

            var cell = new PdfPCell(logo);
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.PaddingLeft = 32;
            cell.PaddingRight = 53;
            cell.PaddingTop = 20;
            cell.PaddingBottom = 10;
            cell.Border = 0;
            cell.Colspan = 2;

            footerTbl.AddCell(cell);

            //if (state != 0)
            //{
            //    footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 0), writer.DirectContent);
            //    return;
            //}

            var remainingPageSpace = writer.GetVerticalPosition(false) - doc.BottomMargin;
            var currentLocation = writer.GetVerticalPosition(false);
            float signHeight = 170;
            if (remainingPageSpace <= signHeight)
            {
                footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 0), writer.DirectContent);
                if (doc.IsMarginMirroring())
                {
                    doc.SetMarginMirroring(false);
                    CustomSignature(writer, doc, doc.BottomMargin + 20);
                    //state = doc.PageNumber + 1;
                    //doc.NewPage();
                }
            }
            else
            {
                if (currentLocation >= (doc.PageSize.Height/2) + 100 )
                {
                    CustomSignature(writer, doc, (currentLocation - 70));
                    footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 0), writer.DirectContent);
                    return;
                }
                else
                {
                    CustomSignature(writer, doc, doc.BottomMargin + 40);
                    footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 0), writer.DirectContent);
                }

                doc.SetMarginMirroring(false);
            }            
        }
    }
}
