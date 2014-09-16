namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Dal.Repositories.Invoice;

    public sealed class CaseInvoiceSettingsService : ICaseInvoiceSettingsService
    {
        private readonly ICaseInvoiceSettingsRepository caseInvoiceSettingsRepository;

        public CaseInvoiceSettingsService(
            ICaseInvoiceSettingsRepository caseInvoiceSettingsRepository)
        {
            this.caseInvoiceSettingsRepository = caseInvoiceSettingsRepository;
        }

        public CaseInvoiceSettings GetSettings(int customerId)
        {
            return this.caseInvoiceSettingsRepository.GetSettings(customerId);
        }

        public int SaveSettings(CaseInvoiceSettings settings)
        {
            return this.caseInvoiceSettingsRepository.SaveSettings(settings);
        }
    }
}