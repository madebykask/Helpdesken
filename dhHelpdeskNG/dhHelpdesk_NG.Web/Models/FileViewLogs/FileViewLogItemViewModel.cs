using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.FileViewLogs
{
    public class FileViewLogItemViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CaseNumber { get; set; }

        public string Operation { get; set; }
        public string Source { get; set; }
        public string DepartmentName { get; set; }
        public string ProductAreaName { get; set; }
    }
}