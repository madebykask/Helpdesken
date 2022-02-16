using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.CaseSolution
{
    public sealed class CaseSolutionCategoryLanguage
    {
        public CaseSolutionCategoryLanguage(int categoryId, string categoryName, int languageId)
        {
            CategoryId = categoryId;
            CaseSolutionCategoryName = categoryName;
            LanguageId = languageId;

        }

        public int CategoryId { get; set; }

        public string CaseSolutionCategoryName { get; set; }

        public int LanguageId { get; set; }
    }
}
