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

        [JsonProperty("items")]
        public List<Root> Items { get; set; }

        [JsonProperty("caseSolutionIds")]
        public int[] CaseSolutionIds { get; set; }
    }
    public class Root
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("label", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Label { get; set; }

        //[JsonProperty("validators")]
        //public Validators Validators { get; set; }
    }


    public class OnSave
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Validators
    {
        [JsonProperty("onSave")]
        public List<OnSave> OnSave { get; set; }
    }
}