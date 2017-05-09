using System;

namespace DH.Helpdesk.Services.BusinessLogic.Invoice
{
    using System.IO;

    public interface IInvoiceImporter
    {
        ArticlesImportData ImportArticles(Stream stream, DateTime lastSyncDate, string filter);

        void SaveImportedArticles(ArticlesImportData data, int customerId, DateTime lastSyncDate);
    }
}