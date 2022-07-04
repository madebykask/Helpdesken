using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{
    public class Wrapper
    {

        public GenericValueWrapper<ComputerSystem> ComputerSystem { get; set; }

        public GenericValueWrapper<OperatingSystem> OperatingSystem { get; set; }

        public GenericValueWrapper<PCBios> PCBios { get; set; }

        public GenericValueWrapper<RSystem> RSystem { get; set; }

        public GenericValueWrapper<VideoControllerData> VideoControllerData { get; set; }

        public GenericValueWrapper<X86PCMemory> X86PCMemory { get; set; }

        public GenericValueWrapper<Enclosure> Enclosure { get; set; }

        public GenericValueWrapper<Processor> Processor { get; set; }

        public GenericValueWrapper<NetworkAdapter> NetworkAdapter { get; set; }

        public GenericValueWrapper<NetworkAdapterConfiguration> NetworkAdapterConfiguration { get; set; }

        public GenericValueWrapper<SoundDevice> SoundDevice { get; set; }

        public GenericValueWrapper<Programs> Programs { get; set; }

        public GenericValueWrapper<LogicalDisk> LogicalDisk { get; set; }

    }
}
