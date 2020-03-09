using System.Collections;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.ReportService
{
    public class SolvedInTimeDataResult
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int Total { get; set; }
        public int SolvedInTimeTotal { get; set; }
    }
}