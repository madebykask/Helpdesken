using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.FileViewLog
{
    public class FileViewLogListItemModel
    {
        public FileViewLogModel Log { get; set; }
        public int? ProductAreaId { get; set; }
        public string ProductAreaName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string UserName { get; set; }
        public decimal CaseNumber { get; set; }
    }
}
