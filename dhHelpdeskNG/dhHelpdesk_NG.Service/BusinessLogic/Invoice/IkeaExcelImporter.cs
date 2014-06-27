namespace DH.Helpdesk.Services.BusinessLogic.Invoice
{
    using System.IO;

    using DH.Helpdesk.BusinessData.Models.Invoice;

    public sealed class IkeaExcelImporter : IInvoiceImporter
    {
        public InvoiceArticle[] ImportArticles(Stream stream)
        {
            return new InvoiceArticle[0];
        }
    }
}