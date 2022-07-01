using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Entities
{
    public class Wrapper
    {

        public ComputerSystemWrapper ComputerSystemWrapper { get; set; }

        public OperatingSystemWrapper OperatingSystemWrapper { get; set; }

        public PCBiosWrapper PCBiosWrapper { get; set; }

        public RSystemWrapper RSystemWrapper { get; set; }

        public VideoControllerDataWrapper VideoControllerDataWrapper { get; set; }

        public X86PCMemoryWrapper X86PCMemoryWrapper { get; set; }

    }
}
