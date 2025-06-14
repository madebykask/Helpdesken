﻿
using System;
namespace DH.Helpdesk.BusinessData.Models
{
    public class TextTranslationLanguageList
    {
        public TextTranslationLanguageList()
        { 
        }

        public int Language_Id { get; set; }
        public int Text_Id { get;  set; }
        public int TextTranslation_Id { get; set; }
        public string LanguageName { get; set; }
        public string TranslationName { get; set; }
        public int ChangedByUser_Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class TextTranslationList
    {
        public int TranslationText_Id { get; set; }
        public int Language_Id { get; set; }
        public int Text_Id { get; set; }
        public string LanguageName { get; set; }
        public string TranslationName { get; set; }
    }

    public class TextTranlationsTextLanguageList
    {
        public int Language_Id { get; set; }
        public int Text_Id { get; set; }
        public int TranslationText_Id { get; set; }
        public string TranslationName { get; set; }
    }
}
