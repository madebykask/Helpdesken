namespace DH.Helpdesk.Domain.Orders
{
    using global::System;
    using global::System.Collections.Generic;

    public class OrderPropertyEntity : Entity
    {
        public string OrderProperty { get; set; }

        public int OrderTypeId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

        public virtual OrderType OrderType { get; set; }

        public virtual ICollection<Order> Orders { get; set; } 
    }
}