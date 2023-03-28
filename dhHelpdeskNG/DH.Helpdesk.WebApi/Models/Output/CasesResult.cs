using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Common.Extensions.String;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.utils;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Web;

namespace DH.Helpdesk.WebApi.Models.Output
{
    public class CasesResult
    {
        [JsonPropertyName("items")]
        public List<ExpandoObject> Items { get; set; }
        [JsonPropertyName("columns")]
        public List<CaseField> Columns { get; set; }

        public CasesResult(CustomerUserCase[] cases, string caseFieldsToReturn)
        {
            try
            {
                Items = new List<ExpandoObject>();
                Columns = new List<CaseField>();
                int i = 1;
                //Get properties of CustomerUserCase
                Type myClassType = typeof(CustomerUserCase);
                PropertyInfo[] propertyInfos = myClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                string[] propertyNames = Array.ConvertAll(propertyInfos, p => p.Name);
                foreach (var prop in propertyNames)
                {
                    if (caseFieldsToReturn.IsNotNullOrEmpty() && caseFieldsToReturn.ToLower().Contains(prop.ToLower()))
                    {
                        //if (prop.ToLower() != "id")
                        //{

                            Columns.Add(new CaseField
                            {
                                Key = "column" + i.ToString(),
                                Name = prop,
                                FieldName = "column" + i.ToString(),
                                Identifier = prop,
                                IsResizable = true
                            });

                          
                        //}

                        i++;

                    }
                    
                }

                int keyCounter = 1;
                foreach (var customerCase in cases)
                {
                    dynamic item = new ExpandoObject();
                    ((IDictionary<string, object>)item)["key"] = keyCounter;
                    ((IDictionary<string, object>)item)["id"] = customerCase.Id;
                    int columnCounter = 1;

                    foreach (var prop in propertyNames)
                    {
                        if (caseFieldsToReturn.IsNotNullOrEmpty() && caseFieldsToReturn.ToLower().Contains(prop.ToLower()))
                        {
                            string propertyName = "column" + columnCounter.ToString();
                            ((IDictionary<string, object>)item)[propertyName] = customerCase.getObjectValue(prop);
                            columnCounter++;
                        }   
                    }
                    Items.Add(item);
                    keyCounter++;
                }
            }
            catch (Exception es)
            {
            }
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

        //[JsonPropertyName("minWidth")]
        //public int MinWidth { get; set; } = 100;

        //[JsonPropertyName("maxWidth")]
        //public int MaxWidth { get; set; } = 200;

        [JsonPropertyName("isResizable")]
        public bool IsResizable { get; set; } = true;


        [JsonPropertyName("isVisible")]
        public bool IsVisible { get; set; } = true;

    }

}