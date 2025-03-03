using DH.Helpdesk.Domain.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.Helpers
{
    public class CaseWithStatistic
    {
        public Case Case { get; set; }
        public CaseStatistic CaseStatistic { get; set; }
    }

}
