namespace DH.Helpdesk.Mobile.Areas.Admin.Models.Invoice
{
    using DH.Helpdesk.BusinessData.Models.Invoice;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class CaseInvoiceSettingsModel
    {
        [NotNull]
        public Customer Customer { get; set; }

        [NotNull]
        public ArticlesImportModel ArticlesImport { get; set; }

        [NotNull]
        public CaseInvoiceSettings Settings { get; set; }
    }
}