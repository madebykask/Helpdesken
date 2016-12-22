using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.Model.Entities
{
    public class TextTranslation
    {
        public string LanguageId { get; set; }
        public int TextType { get; set; }
        public string Text { get; set; }
        public string Translation { get; set; }
    }
}
