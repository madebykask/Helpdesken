namespace DH.Helpdesk.Domain
{
    using global::System;
    using global::System.Collections.Generic;

    public class TextTranslation
    {
        public int Language_Id { get; set; }
        public int Text_Id { get; set; }
        public int TextTranslation_Id { get; set; }
        public string TextTranslated { get; set; }
        public int? ChangedByUser_Id { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Language Language { get; set; }
        public virtual Text Text { get; set; }
        //public User User { get; set; }
    }
}
