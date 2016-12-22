using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECT.Model.Entities
{
    public class FormSettings
    {
        public int Id { get; set; }
        public int TextTypeId { get; set; }
        public bool LogTranslations { get; set; }
        public string DateFormat { get; set; }
    }
}