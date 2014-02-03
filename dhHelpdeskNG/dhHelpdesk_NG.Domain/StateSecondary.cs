using System;

namespace dhHelpdesk_NG.Domain
{
    public class StateSecondary : Entity
    {
        public int Customer_Id { get; set; }
        public int IncludeInCaseStatistics { get; set; }
        public int IsActive { get; set; }
        public int NoMailToNotifier { get; set; }
        public int ResetOnExternalUpdate { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? WorkingGroup_Id { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual WorkingGroupEntity WorkingGroup { get; set; }
    }
}
