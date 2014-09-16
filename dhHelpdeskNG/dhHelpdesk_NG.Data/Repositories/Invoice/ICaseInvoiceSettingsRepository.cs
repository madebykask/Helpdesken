namespace DH.Helpdesk.Dal.Repositories.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface ICaseInvoiceSettingsRepository
    {
        CaseInvoiceSettings GetSettings(int customerId);

        int SaveSettings(CaseInvoiceSettings settings);
    }
}