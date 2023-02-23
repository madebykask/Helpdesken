using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.Inventory.Output;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Services.utils;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.WebApi.Models
{
    public class CaseOverviewWebpartModel
    {
        public List<Column> columns { get; set; }
        public List<Item> items { get; set; }

        public CaseOverviewWebpartModel(IList<DH.Helpdesk.Domain.Case> cases, IList<CaseSettings> columnsFromCaseSettings)
        {
            try
            {
                items = new List<Item>();
                columns = new List<Column>();
                int i = 1;
                foreach (var setting in columnsFromCaseSettings)
                {
                    columns.Add(new Column
                    {
                        key = "column" + i.ToString(),
                        name = setting.Name,
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

                    var item = new Item();
                    item.key = keyCounter.ToString();
                    int columnCounter = 1;
                    foreach (var customerSetting in columnsFromCaseSettings)
                    {
                        if (columnCounter == 1)
                        {
                            var apa = customerCase.getObjectValue(customerSetting.Name);
                            item.column1 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 2)
                        {
                            item.column2 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 3)
                        {
                            item.column3 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 4)
                        {
                            item.column4 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 5)
                        {
                            item.column5 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 6)
                        {
                            item.column6 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 7)
                        {
                            item.column7 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 8)
                        {
                            item.column8 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 9)
                        {
                            item.column9 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        if (columnCounter == 10)
                        {
                            item.column10 = customerCase.getObjectValue(customerSetting.Name);

                        }
                        columnCounter++;

                    }
                    keyCounter++;
                    items.Add(item);

                }
            }
            catch(Exception es)
            {
           
            }

        }


        public class Item
        {
            public string key { get; set; }
            public string column1 { get; set; }
            public string column2 { get; set; }
            public string column3 { get; set; }
            public string column4 { get; set; }
            public string column5 { get; set; }
            public string column6 { get; set; }
            public string column7 { get; set; }
            public string column8 { get; set; }
            public string column9 { get; set; }
            public string column10 { get; set; }
        }
        public class ItemColumn
        {
            public string key { get; set; }
            public string value { get; set; }
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