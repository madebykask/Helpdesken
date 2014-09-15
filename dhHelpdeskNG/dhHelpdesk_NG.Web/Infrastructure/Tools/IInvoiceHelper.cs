namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Xml;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceHelper
    {
        CaseInvoice[] ToCaseInvoices(string invoices);

        XmlDocument ToOutputXml(CaseInvoice[] invoices);
    }
}