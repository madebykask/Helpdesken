namespace DH.Helpdesk.Domain
{
    using global::System;

    public class Vendor : Entity
    {
        public int Customer_Id { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string EMail { get; set; }
        public string HomePage { get; set; }
        public string Phone { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public string VendorName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
