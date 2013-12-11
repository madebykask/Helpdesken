using System;

namespace dhHelpdesk_NG.DTO.DTOs
{
    public class CustomerReportList
    {
        public int ActiveOnPage { get; set; }
        public int CustomerId { get; set; }
        public int ReportId { get; set; }
        public string ReportName { get; set; }
    }
}
