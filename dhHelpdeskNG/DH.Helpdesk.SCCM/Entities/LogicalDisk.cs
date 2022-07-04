using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{

    public class LogicalDisk
    {

        public long ResourceID { get; set; }

        public string Name { get; set; }

        public string DeviceId { get; set; }

        public string FileSystem { get; set; }

        public long Size { get; set; }

        public long FreeSpace { get; set; }

        public string DriveType { get; set; }
    }

}
