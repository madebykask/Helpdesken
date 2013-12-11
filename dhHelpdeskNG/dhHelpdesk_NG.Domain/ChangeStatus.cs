using System;

namespace dhHelpdesk_NG.Domain
{
    public class ChangeStatus : Entity
    {
        public int CompletionStatus { get; set; }
        public int Customer_Id { get; set; }
        public int InformOrderer { get; set; }
        public int IsDefault { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
