using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{

    public class NetworkAdapterConfiguration
    {

        public long ResourceID { get; set; }

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }


    }
}
