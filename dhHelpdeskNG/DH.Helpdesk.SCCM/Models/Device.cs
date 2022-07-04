using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Models
{
    public class Device
    {

        public long ResourceID { get; set; }

        public ComputerSystem _ComputerSystem { get; set; }

        public OperatingSystem _OperatingSystem { get; set; }

        public PCBios _PCBios { get; set; }

        public RSystem _RSystem { get; set; }

        public VideoControllerData _VideoControllerData { get; set; }

        public X86PCMemory _X86PCMemory { get; set; }

        public Enclosure _Enclosure { get; set; }

        public Processor _Processor { get; set; }

        public NetworkAdapter _NetworkAdapter { get; set; }

        public NetworkAdapterConfiguration _NetworkAdapterConfiguration { get; set; }

        public SoundDevice _SoundDevice { get; set; }

        public Programs _Programs { get; set; }

        public LogicalDisk _LogicalDisk { get; set; }

    }

    public class ComputerSystem
    {

        public string Name { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public string UserName { get; set; }

        public DateTime? TimeStamp { get; set; }
    }

    public class OperatingSystem
    {

        public string Caption { get; set; }

        public string CSDVersion { get; set; }

        public string Version { get; set; }

        public int ComputerRole
        {
            get
            {
                if (Caption.Contains("Server"))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    public class PCBios
    {

        public string SerialNumber { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public string SMBIOSBIOSVersion { get; set; }
    }

    public class RSystem
    {

        public string Company { get; set; }
    }

    public class VideoControllerData
    {

        public string Name { get; set; }
    }

    public class X86PCMemory
    {

        public long TotalPhysicalMemory { get; set; }
    }

    public class Enclosure
    {

        public string ChassisTypes { get; set; }

    }

    public class Processor
    {

        public string Name { get; set; }


    }

    public class NetworkAdapter
    {


        public string Name { get; set; }


    }

    public class NetworkAdapterConfiguration
    {

        public string IPAddress { get; set; }

        public string MacAddress { get; set; }


    }

    public class SoundDevice
    {

        public string Name { get; set; }
    }

    public class Programs
    {

        public string DisplayName { get; set; }

        public string Version { get; set; }

        public string Publisher { get; set; }
    }

    public class LogicalDisk
    {

        public string Name { get; set; }

        public string DeviceId { get; set; }

        public string FileSystem { get; set; }

        public string Size { get; set; }
        
        public string FreeSpace { get; set; }

        public string DriveType { get; set; }
    }

}
