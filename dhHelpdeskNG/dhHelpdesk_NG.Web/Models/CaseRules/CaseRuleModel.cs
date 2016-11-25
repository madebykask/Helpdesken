using System.Collections.Generic;

namespace DH.Helpdesk.Web.Models.CaseRules
{
    public enum CaseRuleType
    {
        OriginalRule = 0,
        NewMode = 1,
        InheritMode = 2,
        SelfService = 3
    }

    public enum RelationActionType
    {
        ValueSetter = 1,
        ListCleaner = 2,
        ListPopulator = 3
    }

    public class CaseRuleModel
    {
        public CaseRuleModel()
        {

        }

        public CaseRuleType RuleType { get; set; }

        public List<FieldAttributeModel> FieldAttributes { get; set; }
    }    

    public class FieldAttributeModel
    {

        public FieldAttributeModel()
        {
            Items = new List<FieldItem>();
            Relations = new List<FieldRelation>();
        }

        public string FieldId { get; set; }

        public string FieldName { get; set; }

        public string FieldType { get; set; }

        public string FieldCaption { get; set; }

        public bool IsAvailableOnHelpdesk { get; set; }

        public bool IsAvailableOnSelfService { get; set; }

        public string IsMandatory { get; set; }

        public int FieldAttribute { get; set; }        

        public FieldItem DefaultItem { get; set; }

        public FieldItem Selected { get; set; }

        public List<FieldItem> Items { get; set; }

        public List<FieldRelation> Relations { get; set; }


        public void AddItem(FieldItem item)
        {
            Items.Add(item);
        }

        public void AddRelation(FieldRelation relation)
        {
            Relations.Add(relation);
        }

    }

    public sealed class FieldItem
    {
        public FieldItem(string itemValue, string itemText, bool isActive)
        {
            ItemValue = itemValue;
            ItemText = itemText;
            IsActive = isActive;
        }

        public string ItemValue { get; private set; }

        public string ItemText { get; private set; }

        public bool IsActive { get; private set; }

    }

    public sealed class FieldRelation
    {
        public FieldRelation()
        {

        }

        public int SequenceNo { get; set; }

        public string FieldId { get; set; }

        public string FieldName { get; set; }

        public string FieldCaption { get; set; }

        public int ActionType { get; set; }

        public FieldItem ValueToSet { get; set; }

    }
}