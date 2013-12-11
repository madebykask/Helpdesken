using System;
using System.Collections.Generic;

namespace dhHelpdesk_NG.Domain
{
    public class Language : Entity
    {
        public int IsActive { get; set; }
        public string LanguageID { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
    }
}
