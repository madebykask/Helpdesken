using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.CaseSolutionYearly
{
    public class CaseScheduleItem
    {
        public int CaseSolutionId { get; set; }
        public DateTime NextRun { get; set; }
        public string RepeatType { get; set; }
        public int? RepeatInterval { get; set; }
        public int? StartYear { get; set; }
        public Decimal ScheduleTime { get; set; }
        public string ScheduleMonths { get; set; }
        public int? ScheduleMonthlyDay { get; set; }
        public int? ScheduleMonthlyOrder { get; set; }
        public int? ScheduleMonthlyWeekday { get; set; }
        public string DaysOfWeek { get; set; }
        public DateTime? LastExecuted { get; set; }
    }

}
