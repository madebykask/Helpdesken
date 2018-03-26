using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Dto
{
    internal class ImportInitiator_JobSettings
    {       
        public int CustomerId { get; set; }
        public string Url { get; set; }
        public string InputFilename { get; set; }
        public string Filter { get; set; }
        public int OverwriteFromMasterDirectory { get; set; }
        public int Days2WaitBeforeDelete { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }          
        public int Logging { get; set; }  
        public string StartTime { get; set; }
        public string CronExpression { get; set; }
        public string TimeZone { get; set; }      
        public string ImportFormat { get; set; }              
    }
}
