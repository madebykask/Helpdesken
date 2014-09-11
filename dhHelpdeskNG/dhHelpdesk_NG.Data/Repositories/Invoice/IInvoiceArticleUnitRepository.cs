namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface IInvoiceArticleUnitRepository
    {
        InvoiceArticleUnit[] GetUnits(int customerId);

        void SaveUnit(InvoiceArticleUnit unit);
    }
}