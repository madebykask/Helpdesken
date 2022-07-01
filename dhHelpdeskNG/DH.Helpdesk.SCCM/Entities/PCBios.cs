using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{

    public class PCBios
    {

        public long ResourceID { get; set; }

        public string SerialNumber { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string SMBIOSBIOSVersion { get; set; }

    }
}
