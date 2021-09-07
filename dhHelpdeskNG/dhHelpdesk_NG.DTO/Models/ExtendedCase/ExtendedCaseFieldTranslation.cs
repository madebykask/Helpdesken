using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    public class ExtendedCaseFieldTranslation
    {
        public Domain.Language Language { get; set; }

        public string Name { get; set; }

        public string TranslationText { get; set; }
    }
}
