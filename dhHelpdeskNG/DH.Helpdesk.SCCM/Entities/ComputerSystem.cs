using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{

    public class ComputerSystem
    {

        public long ResourceID { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public string UserName { get; set; }

        public DateTime? TimeStamp { get; set; }

        
    }
}
