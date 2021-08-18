using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CustomerCaseSolutions
    {
        public DH.Helpdesk.Domain.Customer Customer { get; set; }

        public IEnumerable<CaseSolutionSelections> CaseSolutionSelections { get; set; }
    }
}
