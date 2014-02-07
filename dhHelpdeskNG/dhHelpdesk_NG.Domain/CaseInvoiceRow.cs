namespace DH.Helpdesk.Domain
{
    using global::System;

    public class CaseInvoiceRow : Entity
    {
        public decimal InvoicePrice { get; set; }
        public int Case_Id { get; set; }
        public int Charge { get; set; }
        public int CreatedByUser_Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Case Case { get; set; }
        public virtual User CreatedByUser { get; set; }
    }
}
