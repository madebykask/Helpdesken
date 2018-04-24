using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.Common.Enums.Cases;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CaseSectionInfo
    {
        public string DefaultName { get; set; }
        public CaseSectionType Type { get; set; }
    }
}
