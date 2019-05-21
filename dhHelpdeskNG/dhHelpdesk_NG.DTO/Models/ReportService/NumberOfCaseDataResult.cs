using System;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class NumberOfCaseDataResult
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public DateTime? DateTime { get; set; }
        public int CasesAmount { get; set; }
    }
}