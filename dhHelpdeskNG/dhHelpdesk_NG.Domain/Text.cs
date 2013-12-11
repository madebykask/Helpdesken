using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class Text : Entity
    {
        public int Type { get; set; }
        public string TextToTranslate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
    }
}
