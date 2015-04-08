
namespace DH.Helpdesk.BusinessData.Models
{
    using global::System;
    using global::System.Collections.Generic;

    public class TextList
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string TextToTranslate { get; set; }
        public string CreatedByFirstName { get; set; }
        public string CreatedByLastName { get; set; }
        public DateTime? ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ChangedByFirstName { get; set; }
        public string ChangedByLastName { get; set; }
        
        public List<TextTranlationsTextLanguageList> Translations { get; set; }
    }
}