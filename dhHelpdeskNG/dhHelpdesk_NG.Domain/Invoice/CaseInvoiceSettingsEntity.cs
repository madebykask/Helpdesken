namespace DH.Helpdesk.Domain.Invoice
{
    public class CaseInvoiceSettingsEntity : Entity
    {
        public int CustomerId { get; set; }

        public string ExportPath { get; set; }
    }
}