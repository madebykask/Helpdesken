using System;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class ReportedTimeDataResult
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public DateTime? DateTime { get; set; }
        public int TotalTime { get; set; }
    }
}