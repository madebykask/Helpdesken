namespace DH.Helpdesk.Services.Services
{
    using DH.Helpdesk.BusinessData.Models.Invoice;

    public interface ICaseInvoiceSettingsService
    {
        CaseInvoiceSettings GetSettings(int customerId);

        int SaveSettings(CaseInvoiceSettings settings); 
    }
}