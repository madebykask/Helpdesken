using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

    public class RootElement
    {
        public int id { get; set; }
        public string name { get; set; }
        public LocalizationElement localization { get; set; }
        public ValidatorsMessagesElement validatorsMessages { get; set; }
        public string styles { get; set; }
        public List<TabElement> tabs { get; set; }
    }
    public class LocalizationElement
    {
        public string dateFormat { get; set; }
        public string decimalSeparator { get; set; }
    }

    public class ValidatorsMessagesElement
    {
        public string required { get; set; }
        public string dateYearFormat { get; set; }
        public string email { get; set; }
    }

    public class OnSaveElement
    {
        public string type { get; set; }
    }

    public class ValidatorsElement
    {
        public List<OnSave> onSave { get; set; }
    }

    public class OnSave
    {
    }

    public class ControlElement
    {
        public string id { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public string valueBinding { get; set; }
        public Validators validators { get; set; }
    }

    public class Validators
    {
    }

    public class SectionElement
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<ControlElement> controls { get; set; }
    }

    public class TabElement
    {
        public string id { get; set; }
        public string name { get; set; }
        public string columnCount { get; set; }
        public List<SectionElement> sections { get; set; }
    }
}
