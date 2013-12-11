using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class BulletinBoard : Entity
    {
        public int Customer_Id { get; set; }
        public int PublicInformation { get; set; }
        public int ShowOnStartPage { get; set; }
        public string Text { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ShowDate { get; set; }
        public DateTime ShowUntilDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<WorkingGroup> WGs { get; set; }
    }
}
