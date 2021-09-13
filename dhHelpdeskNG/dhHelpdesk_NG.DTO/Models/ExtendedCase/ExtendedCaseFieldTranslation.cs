using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Language.Output;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    public class ExtendedCaseFieldTranslation
    {
        public LanguageOverview Language { get; set; }

        public bool IsDefaultLanguage { get; set; }

        public string Name { get; set; }

        public string TranslationText { get; set; }
    }
}
