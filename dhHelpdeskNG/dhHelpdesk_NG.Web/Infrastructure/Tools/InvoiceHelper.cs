namespace DH.Helpdesk.Web.Infrastructure.Tools
{
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public static class InvoiceHelper
    {
        public static CaseInvoice[] ToCaseInvoices(string invoices)
        {
            var serializer = new JavaScriptSerializer();
            var invoice = serializer.Deserialize<CaseInvoice>(invoices); 
            return new[] { invoice };
        }
    }
}