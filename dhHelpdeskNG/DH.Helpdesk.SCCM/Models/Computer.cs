using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Models
{
    public class Computer
    {

        public long ResourceID { get; set; }

        public Computer _Computer { get; set; }

        public OperatingSystem _OperatingSystem { get; set; }

        public PCBios _PCBios { get; set; }

        public RSystem _RSystem { get; set; }

        public VideoControllerData _VideoControllerData { get; set; }

        public X86PCMemory _X86PCMemory { get; set; }



    }

    public class ComputerSystem
    {
        public long ResourceID { get; set; }

        public string Name { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public string UserName { get; set; }

        public DateTime? TimeStamp { get; set; }
    }

    public class OperatingSystem
    {
        public long ResourceID { get; set; }

        public string Caption { get; set; }

        public object CSDVersion { get; set; }

        public string Version { get; set; }
    }

    public class PCBios
    {
        public long ResourceID { get; set; }

        public string SerialNumber { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string SMBIOSBIOSVersion { get; set; }
    }

    public class RSystem
    {
        public long ResourceID { get; set; }

        public string Company { get; set; }
    }

    public class VideoControllerData
    {
        public long ResourceID { get; set; }

        public string Name { get; set; }
    }

    public class X86PCMemory
    {
        public long ResourceID { get; set; }

        public long TotalPhysicalMemory { get; set; }
    }
}
