using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.FileViewLogs
{
    public class FileViewLogsViewModel
    {
        public IList<SelectListItem> Customers { get; set; }
        public int SelectedCustomerId { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public List<int> SelectedDepartmetsIds { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public int? Amount { get; set; }

    }
}