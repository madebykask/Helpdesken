namespace DH.Helpdesk.Services.BusinessLogic.Invoice
{
    using System.IO;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceImporter
    {
        InvoiceArticle[] ImportArticles(Stream stream);
    }
}