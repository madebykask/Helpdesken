namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Web.Infrastructure;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public static class PrintExtensions
    {
        private static readonly BaseColor HeaderColor = BaseColor.LIGHT_GRAY;

        public static void AddHeader(this PdfPTable table, string text)
        {
            var cell = new PdfPCell(new Phrase(text, GetBoldFont()))
            {
                BackgroundColor = HeaderColor,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER
            };
    
            table.AddCell(cell);
        }       
        
        public static void AddFooter(this PdfPTable table, string text)
        {
            var cell = new PdfPCell(new Phrase(text, GetBoldFont()))
            {
                BackgroundColor = HeaderColor,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER
            };
    
            table.AddCell(cell);
        }       
        
        public static void AddBolded(this PdfPTable table, string text)
        {
            var cell = new PdfPCell(new Phrase(text, GetBoldFont()));
    
            table.AddCell(cell);
        }
        
        public static void AddRowHeader(this PdfPTable table, string text, int? level = null)
        {
            var cell = new PdfPCell(new Phrase(text, GetBoldFont()))
            {
                BackgroundColor = HeaderColor
            };

            if (level.HasValue)
            {
                cell.PaddingLeft = 5f + level.Value * 10f;
            }
            else
            {
                cell.PaddingLeft = 10f;
            }

            table.AddCell(cell);
        }

        public static void AddEmpty(this PdfPTable table)
        {
            var cell = new PdfPCell();
            table.AddCell(cell);
        }

        public static void AddLine(this Document document)
        {
            var p = new Paragraph();
            document.Add(p);
        }

        public static void AddReportDate(this Document document)
        {
            var p = new Paragraph(DateTime.Today.ToShortDateString())
                        {
                            Alignment = Element.ALIGN_LEFT
                        };

            document.Add(p);
        }

        public static void AddReportTitle(this Document document, string title)
        {
            var p = new Paragraph(title)
                        {
                            Alignment = Element.ALIGN_CENTER, 
                            Font = GetBoldFont()
                        };

            document.Add(p);
        }

        public static string ToValuesList(this List<string> list)
        {
            return list != null && list.Any() ? string.Join(", ", list) : Translation.Get("Alla");
        }

        public static void AddReportParams(this Document document, params string[] parameters)
        {
            for (var i = 0; i < parameters.Length; i += 2)
            {
                string text = string.Format("{0}: {1}", parameters[i], parameters[i + 1]);
                document.Add(new Paragraph(text));
            }
        }

        public static void AddImage(this Document document, string src)
        {
            var image = Image.GetInstance(src);
            document.Add(image);
        }

        private static Font GetBoldFont()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA_BOLD);
        }
    }
}