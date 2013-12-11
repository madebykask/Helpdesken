using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class Supplier : Entity
    {
        public int? Country_Id { get; set; }
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public int SortOrder { get; set; }
        public string ContactName { get; set; }
        public string Name { get; set; }
        public string SupplierNumber { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SyncChangedDate { get; set; }

        public virtual Country Country { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
