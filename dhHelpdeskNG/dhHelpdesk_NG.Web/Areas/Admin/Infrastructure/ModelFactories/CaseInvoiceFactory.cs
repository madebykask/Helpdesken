namespace DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories
{
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;

    public sealed class CaseInvoiceFactory : ICaseInvoiceFactory
    {
        private readonly IProductAreaService productAreaService;

        private readonly IInvoiceArticleService invoiceArticleService;

        public CaseInvoiceFactory(
                IProductAreaService productAreaService,
                IInvoiceArticleService invoiceArticleService)
        {
            this.productAreaService = productAreaService;
            this.invoiceArticleService = invoiceArticleService;            
        }

        public CaseInvoiceSettingsModel GetSettingsModel(Customer customer)
        {
            var instance = new CaseInvoiceSettingsModel
                               {
                                   Customer = customer,
                                   ArticlesImport = new ArticlesImportModel
                                                    {
                                                        CustomerId = customer.Id
                                                    }
                               };

            return instance;
        }

        public IInvoiceImporter GetImporter()
        {
            return new IkeaExcelImporter(this.productAreaService, this.invoiceArticleService);
        }
    }
}