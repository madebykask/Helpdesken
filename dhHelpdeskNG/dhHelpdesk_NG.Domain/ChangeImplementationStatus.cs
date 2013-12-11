using System;

namespace dhHelpdesk_NG.Domain
{
    public class ChangeImplementationStatus : Entity
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}


