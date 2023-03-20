using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Web;
using static DH.Helpdesk.WebApi.Models.CaseOverviewWebpartModel;

namespace DH.Helpdesk.WebApi.Models.Output
{
    public class CasesResult
    {
        [JsonPropertyName("items")]
        public List<JsonObject> Items { get; set; }
        [JsonPropertyName("columns")]
        public List<CaseField> Columns { get; set; }


        public CasesResult()
        {
        }

        public CasesResult(string text, string fieldId)
        {
            Items = new List<JsonObject>();
            Columns = new List<CaseField>();
        }

    }

    public class CaseField
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("stringValue")]
        public string StringValue { get; set; }
        
        [JsonPropertyName("identifier")]
        public string Identifier { get; set; }
        
        [JsonPropertyName("fieldName")]
        public string FieldName { get; set; }
        
        [JsonPropertyName("minWidth")]
        public int MinWidth { get; set; } = 100;
        
        [JsonPropertyName("maxWidth")]
        public int MaxWidth { get; set; } = 200;
        
        [JsonPropertyName("isResizable")]
        public bool IsResizable { get; set; } = true;

    }

}