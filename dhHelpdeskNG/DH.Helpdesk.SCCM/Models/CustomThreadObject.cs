using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.SCCM.Models
{
    public class CustomThreadObject
    {
        public CustomThreadObject(List<Entities.RSystem> rSystem, int threadNumber, string token)
        {
            RSystem = rSystem;
            ThreadNumber = threadNumber;
            Token = token;
        }

        public List<Entities.RSystem> RSystem { get; set; }
        
        public int ThreadNumber { get; set; }

        public string Token { get; set; }
        

    }
}
