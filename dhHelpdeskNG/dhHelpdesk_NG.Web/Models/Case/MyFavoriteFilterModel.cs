namespace DH.Helpdesk.Web.Models.Case
{
    using System;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Enums;
    using System.Collections.Generic;

    public sealed class MyFavoriteFilterJSModel
    {
        public MyFavoriteFilterJSModel()
        {
            this.Fields = new MyFavoriteFilterJSFields();
        }

        public MyFavoriteFilterJSModel(int id, string name, MyFavoriteFilterJSFields fields)
        {
            this.Id = id;
            this.Name = name;
            this.Fields = new MyFavoriteFilterJSFields();
            this.Fields = fields;
        }

        public MyFavoriteFilterJSModel(int id, string name, CaseFilterFavoriteFields fields)
        {
            this.Id = id;
            this.Name = name;
            this.Fields = new MyFavoriteFilterJSFields();
            this.Fields.AddFields(fields);
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public MyFavoriteFilterJSFields Fields { get; set; }

    }

    public class MyFavoriteFilterJSFields: List<MyFavoriteFilterJSField>
    {
        public MyFavoriteFilterJSFields()
        {            
        }

        public void AddField(string attributeName, string attributeValue)
        {
            if (!string.IsNullOrEmpty(attributeName))
            {
                var item = this.Find(f=> f.AttributeName.ToLower() == attributeName.ToLower());
                if (item != null && !string.IsNullOrEmpty(item.AttributeName))
                    item.AttributeValue = attributeValue;
                else
                    this.Add(new MyFavoriteFilterJSField(attributeName, attributeValue));                
            }
        }

        public void AddFields(List<MyFavoriteFilterJSField> jsFields)
        {
            foreach (var field in jsFields)                
                this.AddField(field.AttributeName, field.AttributeValue);
        }

        public void AddFields(CaseFilterFavoriteFields filterFields)
        {
            this.AddField(CaseFilterFields.PerformerNameAttribute, filterFields.AdministratorFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.CaseTypeIdNameAttribute, filterFields.CaseTypeFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.ClosingReasonNameAttribute, filterFields.ClosingReasonFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.DepartmentNameAttribute, filterFields.DepartmentFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.PriorityNameAttribute, filterFields.PriorityFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.ProductAreaIdNameAttribute, filterFields.ProductAreaFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.RegionNameAttribute, filterFields.RegionFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.RegisteredByNameAttribute, filterFields.RegisteredByFilter.GetSelectedStr());            
            this.AddField(CaseFilterFields.CaseRemainingTimeAttribute, filterFields.RemainingTimeFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.ResponsibleNameAttribute, filterFields.ResponsibleFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.StatusNameAttribute, filterFields.StatusFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.StateSecondaryNameAttribute, filterFields.SubStatusFilter.GetSelectedStr());            
            this.AddField(CaseFilterFields.WorkingGroupNameAttribute, filterFields.WorkingGroupFilter.GetSelectedStr());

            if (filterFields.ClosingDateFilter.FromDate != null)
                this.AddField(CaseFilterFields.CaseClosingDateStartFilterNameAttribute, filterFields.ClosingDateFilter.FromDate.ToString());

            if (filterFields.ClosingDateFilter.FromDate != null)
                this.AddField(CaseFilterFields.CaseClosingDateEndFilterNameAttribute, filterFields.ClosingDateFilter.ToDate.ToString());

            if (filterFields.RegistrationDateFilter.FromDate != null)
                this.AddField(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute, filterFields.RegistrationDateFilter.FromDate.ToString());

            if (filterFields.RegistrationDateFilter.ToDate != null)
                this.AddField(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute, filterFields.RegistrationDateFilter.ToDate.ToString());

            if (filterFields.WatchDateFilter.FromDate != null)
                this.AddField(CaseFilterFields.CaseWatchDateStartFilterNameAttribute, filterFields.WatchDateFilter.FromDate.ToString());

            if (filterFields.WatchDateFilter.ToDate != null)            
                this.AddField(CaseFilterFields.CaseWatchDateEndFilterNameAttribute, filterFields.WatchDateFilter.ToDate.ToString());
        }
    }

    public class MyFavoriteFilterJSField
    {
        public MyFavoriteFilterJSField(string attributeName, string attributeValue)
        {
            this.AttributeName = attributeName;
            this.AttributeValue = attributeValue;
        }

        public string AttributeName { get; private set; }

        public string AttributeValue { get; set; }
    }

}