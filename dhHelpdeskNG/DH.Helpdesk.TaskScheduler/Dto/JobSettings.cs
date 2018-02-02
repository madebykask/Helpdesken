using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Dto
{
    internal class JobSettings
    {
        public int Id { get; set; }
        public string TargetFolder { get; set; }
        public string OutputFilename { get; set; }
        public bool AppendTime { get; set; }
        public string StartTime { get; set; }
        public string CronExpression { get; set; }
        public string TimeZone { get; set; }
        //public string ReportNameOrID { get; set; }
        public string SqlQuery { get; set; }
        public string ExportFormat { get; set; }
        public string LastRun { get; set; }
    }
}
