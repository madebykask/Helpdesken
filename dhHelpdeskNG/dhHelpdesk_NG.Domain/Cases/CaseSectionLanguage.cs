using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.Cases
{
    public class CaseSectionLanguage
    {
        public int CaseSection_Id { get; set; }
        public int Language_Id { get; set; }
        public string Label { get; set; }

        public virtual CaseSection CaseSection { get; set; }
        public virtual Language Language { get; set; }
    }
}
