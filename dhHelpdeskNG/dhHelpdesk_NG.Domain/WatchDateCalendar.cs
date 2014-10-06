namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class WatchDateCalendarValue : Entity
    {
        public int WatchDateCalendar_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime WatchDate { get; set; }
        public string WatchDateValueName { get; set; }

        public virtual WatchDateCalendar WatchDateCalendar { get; set; }
    }

    public class WatchDateCalendar : Entity
    {
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<WatchDateCalendarValue> WDCValues { get; set; }
    }
}
