using System;

namespace dhHelpdesk_NG.Domain
{
    public class Floor : Entity
    {
        public int Building_Id { get; set; }
        public int IsActive { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Building Building { get; set; }
    }
}
