using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Domain.ExtendedCaseEntity
{
    public class ExtendedCaseFormTranslation
    {
        public int Id { get; set; }

        public string Property { get; set; }
        
        public string Text { get; set; }

        public bool IsSection { get; set; }

        public int LanguageId { get; set; }

        public int TranslationId { get; set; }
    }
}
