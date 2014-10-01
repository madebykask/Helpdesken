namespace DH.Helpdesk.Domain
{
    using global::System;

    using global::System.Collections.Generic;

    using DH.Helpdesk.Domain.Interfaces;

    public class Product : Entity, ICustomerEntity
    {
        public int Customer_Id { get; set; }

        public int Manufacturer_Id { get; set; }

        public string Name { get; set; }

        public DateTime ChangedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ICollection<License> Licenses { get; set; } 

        public virtual ICollection<Application> Applications { get; set; } 
    }
}
