using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class ChecklistAction : Entity
    {
        public int ChecklistService_Id { get; set; }
        public int IsActive { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ChecklistService ChecklistService { get; set; }
    }
}
