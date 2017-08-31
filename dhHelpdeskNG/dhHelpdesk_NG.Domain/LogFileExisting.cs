using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain
{
    public class LogFileExisting : Entity
    {
        public int? Log_Id { get; set; }
        public int Case_Id { get; set; }
        public string FileName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
