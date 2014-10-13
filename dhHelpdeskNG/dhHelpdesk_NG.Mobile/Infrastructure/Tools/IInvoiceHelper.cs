namespace DH.Helpdesk.Mobile.Infrastructure.Tools
{
    using System.Xml;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceHelper
    {
        CaseInvoice[] ToCaseInvoices(string invoices, CaseOverview caseOverview, InvoiceArticle[] articles);

        XmlDocument ToOutputXml(CaseInvoice[] invoices);

        string GetExportFileName();
    }
}