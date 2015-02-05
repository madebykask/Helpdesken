namespace DH.Helpdesk.Web.Areas.Reports.Infrastructure
{
    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class ReportPrintEvents : PdfPageEventHelper
    {
        public ReportPrintEvents(IElement header, IElement footer)
        {
            this.Footer = footer;
            this.Header = header;
        }

        public IElement Header { get; private set; }

        public IElement Footer { get; private set; }

        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            if (this.Header != null)
            {
                document.Add(this.Header);
            }
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            if (this.Footer != null)
            {
                document.Add(this.Footer);
            }

            base.OnEndPage(writer, document);
        }
    }
}