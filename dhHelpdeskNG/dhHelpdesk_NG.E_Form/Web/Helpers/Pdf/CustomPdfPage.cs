using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace ECT.Web.Pdf
{
    public class CustomPdfPage : iTextSharp.text.pdf.PdfPageEventHelper
    {

        protected Font Header
        {
            get
            {
                return FontFactory.GetFont("Verdana", 12, Font.NORMAL, BaseColor.GRAY);
            }
        }

        protected Font HeaderSmall
        {
            get
            {
                return FontFactory.GetFont("Verdana", 9, Font.NORMAL, BaseColor.GRAY);
            }
        }

        protected Font Footer
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.NORMAL, BaseColor.GRAY);
                
            }
        }

        protected Font FooterSmall
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.BOLD, BaseColor.GRAY);
            }
        }

        protected Font CustomFooter
        {
            get
            {
                return FontFactory.GetFont("verdana", BaseFont.CP1257, 9, Font.NORMAL, BaseColor.BLACK);

            }
        }
        protected Font FooterUnderline
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.UNDERLINE, BaseColor.BLUE);
            }
        }
        protected Font FooterLinkUnderline
        {
            get
            {
                return FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 9, Font.UNDERLINE, BaseColor.GRAY);
            }
        }
       
    }
}