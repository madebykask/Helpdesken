using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class Urgency : Entity
    {
        public int Customer_Id { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<PriorityImpactUrgency> PriorityImpactUrgencies { get; set; }
    }
}
