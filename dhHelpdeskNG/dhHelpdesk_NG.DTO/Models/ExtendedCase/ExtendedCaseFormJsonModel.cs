using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{
    // ExtendedCaseFormJsonModel myDeserializedClass = JsonConvert.DeserializeObject<ExtendedCaseFormJsonModel>(myJsonResponse); 

    public class ExtendedCaseFormJsonModel
    {
        public int id { get; set; }
        public string name { get; set; }

        [JsonIgnore]
        public string description { get; set; }

        [JsonIgnore]
        public int customerId { get; set; }

        [JsonIgnore]
        public string customerGuid { get; set; }

        [JsonIgnore]
        public int languageId { get; set; }

        [JsonIgnore]
        public bool status { get; set; }

        [JsonIgnore]
        public int[] caseSolutionIds { get; set; }

        public LocalizationElement localization { get; set; }

        //public List<DataSourcesElement> dataSources { get; set; }
        public List<DataSource> dataSources { get; set; }

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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string messageName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string enabled { get; set; }
    }

    public class ValidatorsElement
    {
        public List<OnSaveElement> onSave { get; set; }
    }

    public class OnSave
    {
    }

    public class ControlElement
    {
        public string id { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public object valueBinding { get; set; }
        public string addonText { get; set; }
        public ValidatorsElement validators { get; set; }
        public List<DataSource> dataSource { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string caseBinding { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string caseBindingBehaviour { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hiddenBinding;

    }

    public class Validators
    {
    }

    public class SectionElement
    {
        public string id { get; set; }
        
        public string name { get; set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hiddenBinding { get; set; }

        public List<ControlElement> controls { get; set; }
    }

    public class TabElement
    {
        public string id { get; set; }
        public string name { get; set; }
        public string columnCount { get; set; }
        public List<SectionElement> sections { get; set; }
    }

    public class DataSource
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string text { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string type { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string valueField;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string textField;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<DataSourcesParams> parameters { get; set; }

    }

    //public class DataSourcesElement
    //{
    //    public string type { get; set; }

    //    public string id { get; set; }

    //    public List<DataSourcesParams> parameters { get; set; }
    //}

    public class DataSourcesParams
    {
        public string name { get; set; }
        public string field { get; set; }
    }
}
