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
    }
}
