namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Holiday : Entity
    {
        public int HolidayHeader_Id { get; set; }
        public int TimeFrom { get; set; }
        public int TimeUntil { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime HolidayDate { get; set; }

        public virtual HolidayHeader HolidayHeader { get; set; }
    }

    public class HolidayHeader :Entity
    {
        public string Name { get; set; }

        public virtual ICollection<Holiday> Holidays { get; set; }
    }
}
