namespace DH.Helpdesk.BusinessData.Models.Invoice
{
    using System.ComponentModel.DataAnnotations;

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

        public int Id { get; private set; } 

        public int CustomerId { get; private set; }

        [Required]
        public string ExportPath { get; private set; }
    }
}