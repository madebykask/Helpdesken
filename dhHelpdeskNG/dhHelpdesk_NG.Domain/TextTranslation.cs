using System;

namespace dhHelpdesk_NG.Domain
{
    public class TextTranslation
    {
        public int Language_Id { get; set; }
        public int Text_Id { get; set; }
        public int TextTranslation_Id { get; set; }
        public string TextTranslated { get; set; }

        public virtual Language Language { get; set; }
        public virtual Text Text { get; set; }
    }
}
