namespace DH.Helpdesk.Domain
{
    using global::System;

    public class RegistrationSourceCustomer : Entity
    {
        public int SystemCode { get; set; }
        public string SourceName { get; set; }
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
