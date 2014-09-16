namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    public sealed class CaseInvoiceSettings
    {
        public CaseInvoiceSettings(
                int id, 
                int customerId, 
                string exportPath)
        {
            this.Id = id;
            this.CustomerId = customerId;
            this.ExportPath = exportPath;
        }

        public CaseInvoiceSettings(int customerId)
        {
            this.CustomerId = customerId;
        }

        public CaseInvoiceSettings()
        {            
        }

        public int Id { get; set; } 

        public int CustomerId { get; set; }

        public string ExportPath { get; set; }
    }
}