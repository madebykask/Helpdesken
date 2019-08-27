namespace DH.Helpdesk.Web.Models.Case
{
    using System;
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Enums;
    using System.Collections.Generic;
    using DH.Helpdesk.Common.Extensions.DateTime;

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
            this.Fields = fields;
        }

        public MyFavoriteFilterJSModel(int id, string name, CaseFilterFavoriteFields fields)
        {
            this.Id = id;
            this.Name = name;
            this.Fields = new MyFavoriteFilterJSFields();
            this.Fields.AddFields(fields);
        }

        public CaseFilterFavoriteFields GetFavoriteFields()
        {
            var ret = new CaseFilterFavoriteFields();

            DateTime? registerStartDate = null;
            DateTime? registerEndDate = null;
            DateTime? watchStartDate = null;
            DateTime? watchEndDate = null;
            DateTime? closingStartDate = null;
            DateTime? closingEndDate = null;

            foreach (var field in this.Fields)
            {
                switch (field.AttributeName)
                {
                    case CaseFilterFields.PerformerNameAttribute:
                        ret.AdministratorFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.CaseTypeIdNameAttribute:
                        ret.CaseTypeFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.ClosingReasonNameAttribute:
                        ret.ClosingReasonFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.DepartmentNameAttribute:
                        ret.DepartmentFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.PriorityNameAttribute:
                        ret.PriorityFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.ProductAreaIdNameAttribute:
                        ret.ProductAreaFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.RegionNameAttribute:
                        ret.RegionFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.CaseRemainingTimeAttribute:
                        ret.RemainingTimeFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.ResponsibleNameAttribute:
                        ret.ResponsibleFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.StatusNameAttribute:
                        ret.StatusFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.StateSecondaryNameAttribute:
                        ret.SubStatusFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.WorkingGroupNameAttribute:
                        ret.WorkingGroupFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.RegisteredByNameAttribute:
                        ret.RegisteredByFilter = new SelectedItems(field.AttributeValue, false);
                        break;

                    case CaseFilterFields.CaseClosingDateStartFilterNameAttribute:
                        closingStartDate = field.AttributeValue == null ? null : (DateTime?)DateTime.Parse(field.AttributeValue);
                        break;

                    case CaseFilterFields.CaseClosingDateEndFilterNameAttribute:
                        closingEndDate = field.AttributeValue == null ? null : (DateTime?)DateTime.Parse(field.AttributeValue);
                        break;

                    case CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute:
                        registerStartDate = field.AttributeValue == null ? null : (DateTime?)DateTime.Parse(field.AttributeValue);
                        break;

                    case CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute:
                        registerEndDate = field.AttributeValue == null ? null : (DateTime?)DateTime.Parse(field.AttributeValue);
                        break;

                    case CaseFilterFields.CaseWatchDateStartFilterNameAttribute:
                        watchStartDate = field.AttributeValue == null ? null : (DateTime?)DateTime.Parse(field.AttributeValue);
                        break;

                    case CaseFilterFields.CaseWatchDateEndFilterNameAttribute:
                        watchEndDate = field.AttributeValue == null ? null : (DateTime?)DateTime.Parse(field.AttributeValue);
                        break;
                }
            }

            ret.RegistrationDateFilter = new DateToDate(registerStartDate, registerEndDate);
            ret.WatchDateFilter = new DateToDate(watchStartDate, watchEndDate);
            ret.ClosingDateFilter = new DateToDate(closingStartDate, closingEndDate);

            return ret;
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
            this.AddField(CaseFilterFields.CaseRemainingTimeAttribute, filterFields.RemainingTimeFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.ResponsibleNameAttribute, filterFields.ResponsibleFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.StatusNameAttribute, filterFields.StatusFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.StateSecondaryNameAttribute, filterFields.SubStatusFilter.GetSelectedStr());            
            this.AddField(CaseFilterFields.WorkingGroupNameAttribute, filterFields.WorkingGroupFilter.GetSelectedStr());
            this.AddField(CaseFilterFields.RegisteredByNameAttribute, filterFields.RegisteredByFilter.GetSelectedStr());
            
            this.AddField(CaseFilterFields.CaseClosingDateStartFilterNameAttribute, filterFields.ClosingDateFilter.FromDate.ToFormattedDate());
            this.AddField(CaseFilterFields.CaseClosingDateEndFilterNameAttribute, filterFields.ClosingDateFilter.ToDate.ToFormattedDate());
            
            this.AddField(CaseFilterFields.CaseRegistrationDateStartFilterNameAttribute, filterFields.RegistrationDateFilter.FromDate.ToFormattedDate());            
            this.AddField(CaseFilterFields.CaseRegistrationDateEndFilterFilterNameAttribute, filterFields.RegistrationDateFilter.ToDate.ToFormattedDate());
            
            this.AddField(CaseFilterFields.CaseWatchDateStartFilterNameAttribute, filterFields.WatchDateFilter.FromDate.ToFormattedDate());           
            this.AddField(CaseFilterFields.CaseWatchDateEndFilterNameAttribute, filterFields.WatchDateFilter.ToDate.ToFormattedDate());
        }
    }

    public class MyFavoriteFilterJSField
    {
        public MyFavoriteFilterJSField()
        {
        }

        public MyFavoriteFilterJSField(string attributeName, string attributeValue)
        {
            this.AttributeName = attributeName;
            this.AttributeValue = attributeValue;
        }

        public string AttributeName { get; set; }                      

        public string AttributeValue { get; set; }
    }

}