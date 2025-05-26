namespace DH.Helpdesk.Domain
{
    using global::System;

    public class CaseSolutionSchedule
    {
        public decimal ScheduleTime { get; set; }
        public int CaseSolution_Id { get; set; }
        public int ScheduleType { get; set; }
        public int ScheduleWatchDate { get; set; }
        public string ScheduleDay { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string RepeatType { get; set; }
        public int? RepeatInterval { get; set; }
        public int? StartYear { get; set; }
        public string DaysOfWeek { get; set; } // t.ex. "1,2,3"
        public DateTime? NextRun { get; set; }
        public DateTime? LastExecuted { get; set; }
        public int? ScheduleMonthlyDay { get; set; }
        public int? ScheduleMonthlyOrder { get; set; }
        public int? ScheduleMonthlyWeekday { get; set; }
        public string ScheduleMonths { get; set; } // t.ex. "1,2,3"



    }
}
