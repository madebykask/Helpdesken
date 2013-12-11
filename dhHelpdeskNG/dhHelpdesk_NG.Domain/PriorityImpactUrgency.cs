using System;

namespace dhHelpdesk_NG.Domain
{
    public class PriorityImpactUrgency : Entity
    {
        public int? Impact_Id { get; set; }
        public int Priority_Id { get; set; }
        public int? Urgency_Id { get; set; }

        public virtual Impact Impact { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Urgency Urgency { get; set; }
    }
}
