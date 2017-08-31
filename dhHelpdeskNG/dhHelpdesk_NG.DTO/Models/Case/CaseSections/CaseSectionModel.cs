using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseSections
{
    public class CaseSectionModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string SectionHeader { get; set; }

        public int SectionType { get; set; }

        public bool IsNewCollapsed { get; set; }

        public bool IsEditCollapsed { get; set; }

        public List<int> CaseSectionFields { get; set; }
    }
}
