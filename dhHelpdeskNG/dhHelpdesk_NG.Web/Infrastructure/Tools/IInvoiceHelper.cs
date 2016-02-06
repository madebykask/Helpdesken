namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Xml;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceHelper
    {
        CaseInvoice[] ToCaseInvoices(string invoices, CaseOverview caseOverview, InvoiceArticle[] articles, int curUserId, int? orderIdToXML);

        XmlDocument ToOutputXml(CaseInvoice[] invoices);

        string GetExportFileName();
    }
}