using DH.Helpdesk.BusinessData.Models.DailyReport.Output;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
namespace DH.Helpdesk.Web.Models.DailyReports
{
    public class DailyReportModel
    {
     
        public IList<DailyReportInputModel> InputModels { get; set; }
        
        public DailyReportHistoryModel HistoryModel { get; set; }
        
    }

    


}