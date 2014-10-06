namespace DH.Helpdesk.Web.Areas.Admin.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Invoice;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Areas.Admin.Models.Invoice;

    public interface ICaseInvoiceFactory
    {
        CaseInvoiceSettingsModel GetSettingsModel(
                Customer customer,
                CaseInvoiceSettings settings);

        IInvoiceImporter GetImporter(
                IProductAreaService productAreaService,
                IInvoiceArticleService invoiceArticleService);
    }
}