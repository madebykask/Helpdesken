using System;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Document = iTextSharp.text.Document;
using ECT.Model.Entities;

namespace ECT.Web.Pdf.Netherlands
{
    public class NetherlandsPdfPage : CustomPdfPage
    {
        public Contract Contract { get; set; }
        public string Language { get; set; }
        public int State { get; set; }
        public Department department { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document doc)
        {
            PdfPTable headerTbl = new PdfPTable(2);
            headerTbl.TotalWidth = doc.PageSize.Width;

            var c1 = new PdfPCell(new Phrase("")); // Should be to cell adjustment
            c1.Border = 0;
            headerTbl.AddCell(c1);

            string code = Contract.EmployeeNumber + "w101";

            var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

            BaseFont customfont = BaseFont.CreateFont(folderPath + "IDAutomationHC39M.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
            Font bcfont = new Font(customfont, 12);
            bcfont.Color = BaseColor.BLACK;         

            //var para = new Paragraph(code, Barcode);
            var para = new Paragraph(code, bcfont );
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_CENTER;

            var cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            cell.Border = 0;
            cell.PaddingTop = 50;
            cell.PaddingRight = 45;           
            headerTbl.AddCell(cell);

           
            headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 20), writer.DirectContent);

        }
       
        //private void CustomSignature(PdfWriter writer, Document doc, float currentLocation)
        //{

        //    var signTbl = new PdfPTable(2);
        //    signTbl.TotalWidth = doc.PageSize.Width - 100;
        //    signTbl.HorizontalAlignment = Element.ALIGN_CENTER;

        //    var para = new Paragraph("———————————————", CustomFooter);
        //    para.Alignment = Element.ALIGN_CENTER;
        //    para.Add(Environment.NewLine);
        //    para.Alignment = Element.ALIGN_CENTER;

        //    para.Add(I18N.Translate("Date and Employee signature", Language));
        //    para.Add(Environment.NewLine);
        //    var cell = new PdfPCell(para);
        //    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    cell.Border = 0;
        //    cell.PaddingTop = 0;
        //    cell.PaddingLeft = 40;
        //    cell.PaddingBottom = 25;
        //    signTbl.AddCell(cell);


        //    para = new Paragraph("————————————————", CustomFooter);
        //    para.Add(Environment.NewLine);
        //    para.Alignment = Element.ALIGN_CENTER;
        //    para.Add(I18N.Translate("Signature of a person representing the Employer", Language));
        //    para.Add(Environment.NewLine);

        //    cell = new PdfPCell(para);
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cell.Border = 0;
        //    cell.PaddingTop = 0;
        //    cell.PaddingLeft = 50;
        //    cell.PaddingRight = 36;
        //    cell.PaddingBottom = 25;
        //    //

        //    signTbl.AddCell(cell);

        //    signTbl.WriteSelectedRows(0, -1, 0, currentLocation, writer.DirectContent);                

        //}

        //private void Signature(PdfWriter writer, Document doc, float currentLocation)

        //{

        //    var sigTbl = new PdfPTable(3);
        //    sigTbl.TotalWidth = doc.PageSize.Width - 58;
        //    sigTbl.HorizontalAlignment = Element.ALIGN_CENTER;


        //    var para = new Paragraph("———————————————", CustomFooter);
        //    para.Alignment = Element.ALIGN_CENTER;

        //    DateTime currentDate = System.DateTime.Now;
        //    para.Add(currentDate.ToShortDateString());
        //    para.Add(Environment.NewLine);
        //    para.Alignment = Element.ALIGN_CENTER;
        //    para.Add(I18N.Translate("Date and Employee signature", Language));
        //    para.Add(Environment.NewLine);

        //    var cell = new PdfPCell(para);
        //    cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
        //    cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
        //    cell.Border = 0;
        //    cell.PaddingTop = 0;
        //    cell.PaddingLeft = 15;
        //    cell.PaddingBottom = 10;

        //    sigTbl.AddCell(cell);

        //    para = new Paragraph("————————————————", CustomFooter);
        //    para.Add(Environment.NewLine);
        //    para.Alignment = Element.ALIGN_LEFT;
        //    para.Add(I18N.Translate("Signature of a person representing the Employer", Language));
        //    para.Add(Environment.NewLine);

        //    cell = new PdfPCell(para);
        //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
        //    cell.Border = 0;
        //    cell.PaddingTop = 0;
        //    cell.PaddingLeft = 160;
        //    cell.PaddingRight = 35;
        //    cell.PaddingBottom = 10;
        //    cell.Colspan = 2;

        //    sigTbl.AddCell(cell);
            
        //    sigTbl.WriteSelectedRows(0, -1, 30, currentLocation, writer.DirectContent);                


        //}

        public override void OnCloseDocument(PdfWriter writer, Document doc)
        {
            var currentLocation = writer.GetVerticalPosition(false);
            var sigTbl = new PdfPTable(3);
            sigTbl.TotalWidth = doc.PageSize.Width - 58;
            sigTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            DateTime currentDate = System.DateTime.Now;
            var para = new Paragraph(currentDate.ToShortDateString(), CustomFooter);
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(Environment.NewLine);
                        
            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_CENTER;
            para.Add(I18N.Translate("Signature employee", Language));
            para.Add(Environment.NewLine);

            var cell = new PdfPCell(para);
            cell.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell.VerticalAlignment = Element.ALIGN_JUSTIFIED;
            cell.Border = 0;
            cell.PaddingTop = 0;
            cell.PaddingLeft = 15;
            cell.PaddingBottom = 10;

            sigTbl.AddCell(cell);

            para = new Paragraph(I18N.Translate("Signature employer on behalf of IKEA Netherlands B.V", Language),CustomFooter);
            para.Add(Environment.NewLine);
            para.Alignment = Element.ALIGN_LEFT;            
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

            sigTbl.WriteSelectedRows(0, -1, 0, currentLocation , writer.DirectContent);                
        }
                             
    }
}
