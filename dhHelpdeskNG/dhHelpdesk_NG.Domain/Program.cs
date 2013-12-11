using System;

namespace dhHelpdesk_NG.Domain
{
    public class Program : Entity
    {
        public int Customer_Id { get; set; }
        public int IsActive { get; set; }
        public int ShowOnStartPage { get; set; }
        public string List { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}