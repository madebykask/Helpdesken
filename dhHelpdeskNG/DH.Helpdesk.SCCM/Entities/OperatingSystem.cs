using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{

    public class OperatingSystem
    {

        public long ResourceID { get; set; }

        public string Caption { get; set; }

        public string CSDVersion { get; set; }

        public string Version { get; set; }
    }
}
