using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Customer
{
    public class CustomerEmailSettings
    {
        public int CustomerId { get; set; }
        public int CommunicateWithNotifier { get; set; }
        public int CommunicateWithPerformer { get; set; }
        

    }
}
