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

        public MyFavoriteFilterJSModel(int id, string name, MyFavoriteFilterFields fields)
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

        public void AddFields(MyFavoriteFilterFields filterFields)
        {
            this.AddField(CaseFilterFields.PerformerNameAttribute, filterFields.AdministratorIds.GetSelectedStr());
            this.AddField(CaseFilterFields.CaseTypeIdNameAttribute, filterFields.CaseTypeIds.GetSelectedStr());
            this.AddField(CaseFilterFields.ClosingReasonNameAttribute, filterFields.ClosingReason.GetSelectedStr());
            this.AddField(CaseFilterFields.DepartmentNameAttribute, filterFields.DepartmentIds.GetSelectedStr());
            this.AddField(CaseFilterFields.PriorityNameAttribute, filterFields.PriorityIds.GetSelectedStr());
            this.AddField(CaseFilterFields.ProductAreaIdNameAttribute, filterFields.ProductAreaIds.GetSelectedStr());
            this.AddField(CaseFilterFields.RegionNameAttribute, filterFields.RegionIds.GetSelectedStr());
            this.AddField(CaseFilterFields.RegisteredByNameAttribute, filterFields.RegisteredByIds.GetSelectedStr());            
            this.AddField(CaseFilterFields.CaseRemainingTimeAttribute, filterFields.RemainingTimeId.GetSelectedStr());
            this.AddField(CaseFilterFields.ResponsibleNameAttribute, filterFields.ResponsibleIds.GetSelectedStr());
            this.AddField(CaseFilterFields.StatusNameAttribute, filterFields.StatusIds.GetSelectedStr());
            this.AddField(CaseFilterFields.StateSecondaryNameAttribute, filterFields.SubStatusIds.GetSelectedStr());            
            this.AddField(CaseFilterFields.WorkingGroupNameAttribute, filterFields.WorkingGroupIds.GetSelectedStr());

            if (filterFields.ClosingDate.FromDate != null)
                this.AddField(CaseFilterFields.CaseClosingDateStartFilterNameAttribute, filterFields.ClosingDate.FromDate.ToString());

            if (filterFields.ClosingDate.FromDate != null)
                this.AddField(CaseFilterFields.CaseClosingDateEndFilterNameAttribute, filterFields.ClosingDate.ToDate.ToString());

            if (filterFields.RegistrationDate.FromDate != null)
                this.AddField(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute, filterFields.RegistrationDate.FromDate.ToString());

            if (filterFields.RegistrationDate.ToDate != null)
                this.AddField(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute, filterFields.RegistrationDate.ToDate.ToString());

            if (filterFields.WatchDate.FromDate != null)
                this.AddField(CaseFilterFields.CaseWatchDateStartFilterNameAttribute, filterFields.WatchDate.FromDate.ToString());

            if (filterFields.WatchDate.ToDate != null)            
                this.AddField(CaseFilterFields.CaseWatchDateEndFilterNameAttribute, filterFields.WatchDate.ToDate.ToString());
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