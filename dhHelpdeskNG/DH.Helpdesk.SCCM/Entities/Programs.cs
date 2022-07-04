using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{

    public class Programs
    {

        public long ResourceID { get; set; }

        public string DisplayName { get; set; }

        public string Version { get; set; }

        public string Publisher { get; set; }
    }
}
