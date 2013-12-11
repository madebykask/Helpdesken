using System;

namespace dhHelpdesk_NG.Domain
{
    public class Status : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int IsDefault { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
