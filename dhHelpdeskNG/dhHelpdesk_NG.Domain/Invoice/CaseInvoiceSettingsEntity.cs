namespace DH.Helpdesk.Domain.Invoice
{
    public class CaseInvoiceSettingsEntity : Entity
    {
        public int CustomerId { get; set; }

        public string ExportPath { get; set; }

        public string Currency { get; set; }

        public string OrderNoPrefix { get; set; }

        public string Issuer { get; set; }

        public string OurReference { get; set; }

        public string DocTemplate { get; set; }
    }
}