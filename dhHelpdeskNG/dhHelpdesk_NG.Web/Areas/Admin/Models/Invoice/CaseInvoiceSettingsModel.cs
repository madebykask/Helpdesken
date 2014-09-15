namespace DH.Helpdesk.Web.Areas.Admin.Models.Invoice
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Domain;

    public sealed class CaseInvoiceSettingsModel
    {
        [NotNull]
        public Customer Customer { get; set; }

        [NotNull]
        public ArticlesImportModel ArticlesImport { get; set; }

        [NotNull]
        public InvoiceExportSettingsModel ExportSettings { get; set; }
    }
}