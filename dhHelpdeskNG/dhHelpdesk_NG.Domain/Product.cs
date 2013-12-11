using System;

namespace dhHelpdesk_NG.Domain
{
    public class Product : Entity
    {
        public int Customer_Id { get; set; }
        public int Manufacturer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
