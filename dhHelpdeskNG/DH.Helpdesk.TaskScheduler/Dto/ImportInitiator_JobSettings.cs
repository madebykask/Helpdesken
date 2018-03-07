using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Dto
{
    internal class ImportInitiator_JobSettings
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string UserName { get; }
        public string Password { get; }
        public string InputFilename { get; set; }
        public bool AppendTime { get; set; }
        public string StartTime { get; set; }
        public string CronExpression { get; set; }
        public string TimeZone { get; set; }
        //public string ReportNameOrID { get; set; }
        public string SqlQuery { get; set; }
        public string ImportFormat { get; set; }
        public string SaveFolder { get; }
        public string LastRun { get; set; }
    }
}
