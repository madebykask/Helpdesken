namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class Text : Entity
    {
        public int Type { get; set; }
        public string TextToTranslate { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<TextTranslation> TextTranslations { get; set; }
    }
}
