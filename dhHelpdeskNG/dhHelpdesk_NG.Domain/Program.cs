namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Program : Entity
    {
        public Program()
        {
            this.Orders = new List<Order>();
        }

        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int ShowOnStartPage { get; set; }
        public string List { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Order> Orders { get; set; } 
    }
}