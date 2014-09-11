namespace DH.Helpdesk.Services.BusinessLogic.Invoice
{
    using System.IO;

    public interface IInvoiceImporter
    {
        ArticlesImportData ImportArticles(Stream stream);

        void SaveImportedArticles(ArticlesImportData data, int customerId);
    }
}