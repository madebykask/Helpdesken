using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class WatchDateCalendarValue : Entity
    {
        public int WatchDateCalendar_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime WatchDate { get; set; }

        public virtual WatchDateCalendar WatchDateCalendar { get; set; }
    }

    public class WatchDateCalendar : Entity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<WatchDateCalendarValue> WDCValues { get; set; }
    }
}
