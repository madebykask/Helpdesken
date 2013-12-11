using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dhHelpdesk_NG.Domain
{
    public class Calendar : Entity
    {
        public int ChangedByUser_Id { get; set; }
        public int Customer_Id { get; set; }
        public int PublicInformation { get; set; }
        public int ShowOnStartPage { get; set; }
        public string Caption { get; set; }
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime CalendarDate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ShowUntilDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual User ChangedByUser { get; set; }
        public virtual ICollection<WorkingGroup> WGs { get; set; }
    }
}
