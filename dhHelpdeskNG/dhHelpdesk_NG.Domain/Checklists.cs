using System;

namespace dhHelpdesk_NG.Domain
{
    public class Checklists : Entity
    {
        public int Customer_Id { get; set; }
        public string ChecklistName { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
