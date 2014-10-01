namespace DH.Helpdesk.Domain
{
    using global::System;

    using DH.Helpdesk.Domain.Interfaces;

    public class Manufacturer : Entity, ICustomerEntity
    {
        public int Customer_Id { get; set; }

        public string Name { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
