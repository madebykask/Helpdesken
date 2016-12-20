namespace DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;

    public sealed class CaseInvoiceFactory : ICaseInvoiceFactory
    {
        public CaseInvoiceSettingsModel GetSettingsModel(
                Customer customer, 
                CaseInvoiceSettings settings)
        {
            var instance = new CaseInvoiceSettingsModel
                               {
                                   Customer = customer,
                                   ArticlesImport = new ArticlesImportModel
                                                    {
                                                        CustomerId = customer.Id
                                                    },
                                   Settings = settings
                               };

            return instance;
        }

        public IInvoiceImporter GetImporter(
                IProductAreaService productAreaService,
                IInvoiceArticleService invoiceArticleService)
        {
            return new XmlImporter(productAreaService, invoiceArticleService);
        }
    }
}