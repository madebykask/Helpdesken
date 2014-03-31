using System;

namespace DH.Helpdesk.BusinessData.Models.Calendar.Output
{
    public sealed class CalendarOverview
    {
        public int Customer_Id { get; set; }
        public DateTime CalendarDate { get; set; }
        public string Caption { get; set; }
        public string Text { get; set; }
    }
}