using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.CaseSolution
{
    public sealed class CaseSolutionLanguage
    {
        public CaseSolutionLanguage(int caseSolutionId, string name, int languageId, string shortDescription, string information)
        {
            CaseSolutionId = caseSolutionId;
            CaseSolutionName = name;
            ShortDescription = ShortDescription;
            LanguageId = languageId;
            Information = information;

        }

        public int CaseSolutionId { get; set; }

        public string CaseSolutionName { get; set; }

        public int LanguageId { get; set; }
        public string ShortDescription { get; set; }
        public string Information { get; set; }
    }
}
