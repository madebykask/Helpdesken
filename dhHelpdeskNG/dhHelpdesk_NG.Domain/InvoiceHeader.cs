namespace DH.Helpdesk.Domain
{
    using global::System;

    public class InvoiceHeader : Entity
    {
        public double VerificationNumberEnd { get; set; }
        public double VerificationNumberStart { get; set; }
        public int User_Id { get; set; }
        public string InvoiceFilename { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid InvoiceHeaderGUID { get; set; }
    }
}
