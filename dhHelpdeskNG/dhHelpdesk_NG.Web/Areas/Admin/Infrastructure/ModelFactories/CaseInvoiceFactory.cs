namespace DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories
{
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Invoice;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;

    public sealed class CaseInvoiceFactory : ICaseInvoiceFactory
    {
        public CaseInvoiceSettingsModel GetSettingsModel(Customer customer)
        {
            var instance = new CaseInvoiceSettingsModel
                               {
                                   Customer = customer
                               };

            return instance;
        }

        public IInvoiceImporter GetImporter()
        {
            return new IkeaExcelImporter();
        }
    }
}