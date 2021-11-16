using DH.Helpdesk.Domain.ExtendedCaseEntity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.ExtendedCase
{

    public class ExtendedCaseFormPayloadModel
    {
        [JsonProperty("customerId")]
        public int CustomerId { get; set; }       
        
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }

        [JsonProperty("id", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public bool Status { get; set; }

        [JsonProperty("caseSolutionIds")]
        public int[] CaseSolutionIds { get; set; }

        [JsonProperty("tabs")]
        public List<Tab> Tabs { get; set; }

        //[JsonProperty("sections")]
        //public List<Section> Sections { get; set; }

        [JsonProperty("translations")]
        public List<ExtendedCaseFormTranslation> Translations { get; set; }

    }

    public class Tab
    {
        [JsonProperty("id")]
        public string  Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("columnCount")]
        public int ColumnCount { get; set; }
        
        [JsonProperty("sections")]
        public List<Section> Sections { get; set; }
    }

    public class Section
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("sectionName")]
        public string SectionName { get; set; }

        [JsonProperty("controls")]
        public List<Control> Controls { get; set; }

    }


    public class Control
    {
        [JsonProperty("id")]
        public string Id { get; set; }        
        
        [JsonProperty("type")]
        public string Type { get; set; }     
        
        [JsonProperty("label")]
        public string Label { get; set; } 
        
        [JsonProperty("required")]
        public bool Required { get; set; }   
        
        [JsonProperty("valueBinding")]
        public object ValueBinding { get; set; }

        [JsonProperty("translationId")]
        public int TranslationId { get; set; }

        [JsonProperty("addonText")]
        public string AddOnText { get; set; }

        [JsonProperty("dataSource")]
        public List<DataSource> DataSource { get; set; }

    }

}