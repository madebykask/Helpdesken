
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using System.Reflection;
using System.Dynamic;
using DH.Helpdesk.BusinessData.Models.Customer;

namespace DH.Helpdesk.WebApi.Models
{
    public class CaseOverviewWebpartModel
    {
        public List<Column> columns { get; set; }
        public List<ExpandoObject> items { get; set; }

        public CaseOverviewWebpartModel(CustomerUserCase[] cases)
        {
            try
            {
                items = new List<ExpandoObject>();
                columns = new List<Column>();
                int i = 1;
                //Get properties of CustomerUserCase
                Type myClassType = typeof(CustomerUserCase);
                PropertyInfo[] propertyInfos = myClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                string[] propertyNames = Array.ConvertAll(propertyInfos, p => p.Name);
                foreach (var prop in propertyNames)
                {
                    columns.Add(new Column
                    {
                        key = "column" + i.ToString(),
                        name = prop,
                        fieldName = "column" + i.ToString(),
                        minWidth = 100,
                        maxWidth = 200,
                        isResizable = true
                    });
                    i++;
                }

                int keyCounter = 1;
                foreach (var customerCase in cases)
                {
                    dynamic item = new ExpandoObject();
                    ((IDictionary<string, object>)item)["key"] = keyCounter;
                    int columnCounter = 1;
                    foreach (var prop in propertyNames)
                    {
                        
                        string propertyName = "column" + columnCounter.ToString();
                        
                        ((IDictionary<string, object>)item)[propertyName] = customerCase.getObjectValue(prop);
                        columnCounter++;
                    }
                    items.Add(item);
                    keyCounter++;
                }
               
                
                
            }
            catch(Exception es)
            {
           
            }

        }

        public class Column
        {
            public string key { get; set; }
            public string name { get; set; }
            public string fieldName { get; set; }
            public int minWidth { get; set; }
            public int maxWidth { get; set; }
            public bool isResizable { get; set; }
        }
    }
}