using System;
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

    public enum RelationType
    {
        OneToOne = 1,
        OneToMany = 2,
        ManyToMany = 3,
        Virtual = 4
    }

    public enum RelationActionType
    {
        ValueSetter = 1,
        ListCleaner = 2,
        ListPopulator = 3,
        StaticRuntimeAction = 9
    }

    public enum CaseFieldType
    {
        TextField = 1,
        SingleSelectField = 2,
        MultiSelectField = 3,
        TreeButtonSelect = 4,
        TextArea = 5,
        CheckBox = 6,
        DateField = 7,
        ElementsGroup = 8,
        ButtonField = 9
    }

    public enum CaseFieldStatusType
    {
        Editable = 1,
        Readonly = 2,
        Hidden = 3
    }

    public class CaseRuleModel
    {
        public CaseRuleModel()
        {
            FieldAttributes = new List<FieldAttributeModel>();
            CustomerSettings = new CaseCustomerSettings();
        }

        public CaseRuleType RuleType { get; set; }

        public List<FieldAttributeModel> FieldAttributes { get; set; }

        public CaseCustomerSettings CustomerSettings { get; set; }
    }

    public class CaseCustomerSettings
    {
        public CaseCustomerSettings()
        {

        }

        public bool ConnectUserToWorkingGroup { get; set; }

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

        public CaseFieldType FieldType { get; set; }

        public string FieldCaption { get; set; }

        public bool IsAvailableOnHelpdesk { get; set; }

        public bool IsAvailableOnSelfService { get; set; }

        public bool IsMandatory { get; set; }

        /* Readonly/Hidden/Editable */
        public CaseFieldStatusType StatusType { get; set; }

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
        public FieldItem(string itemValue, string itemText, bool isActive = true, string parentItemValue = "")
        {
            ItemValue = itemValue;
            ItemText = itemText;
            IsActive = isActive;
            ParentItemValue = parentItemValue;
        }

        public string ItemValue { get; private set; }
        

        public string ItemText { get; private set; }

        public bool IsActive { get; private set; }

        public string ForeignKeyValue1 { get; set; }

        public string ForeignKeyValue2 { get; set; }

        public string ForeignKeyValue3 { get; set; }

        public string ResultKeyValue { get; set; }

        public string ParentItemValue { get; set; }

        public static FieldItem CreateEmpty()
        {
            return new FieldItem(string.Empty, string.Empty);
        }

    }
   
    public sealed class FieldRelation
    {
        public FieldRelation()
        {
           
        }

        public int SequenceNo { get; set; }

        public string FieldId { get; set; }

        public int RelationType { get; set; }

        public int ActionType { get; set; }          

        public int? ForeignKeyNumber { get; set; }

        public string DataStore1 { get; set; }

        public string DataStore2 { get; set; }

        public string DataStore3 { get; set; }

        public int? StaticActionId { get; set; }

    }


    public sealed class BasicSingleItemField
    {
        public BasicSingleItemField()
        {
            
        }
        
        public CaseFieldStatusType StatusType { get; set; }        

        public FieldItem Selected { get; set; }        

    }

    public sealed class BasicMultiItemField
    {
        public BasicMultiItemField()
        {
            Items = new List<FieldItem>();
        }

        public CaseFieldStatusType StatusType { get; set; }

        public FieldItem DefaultItem { get; set; }

        public FieldItem Selected { get; set; }

        public List<FieldItem> Items { get; set; }

    }

    public sealed class BasicVirtualDataField
    {
        public BasicVirtualDataField()
        {
            Items = new List<FieldItem>();
        }      

        public List<FieldItem> Items { get; set; }

    }

    public sealed class BasicCaseInformation
    {
        #region Initiator

        public BasicSingleItemField ReportedBy { get; set; }

        public BasicSingleItemField PersonsName { get; set; }

        public BasicSingleItemField PersonsEmail { get; set; }

        public BasicSingleItemField MailToNotifier { get; set; }

        public BasicSingleItemField PersonsPhone { get; set; }

        public BasicSingleItemField PersonsCellPhone { get; set; }

        public BasicSingleItemField CostCentre { get; set; }

        public BasicSingleItemField Place { get; set; }

        public BasicSingleItemField UserCode { get; set; }

        public BasicSingleItemField UpdateUserInfo { get; set; }

        public BasicMultiItemField Regions { get; set; }

        public BasicMultiItemField Departments { get; set; }

        public BasicMultiItemField OUs { get; set; }

        #endregion

        #region IsAbout

        public BasicSingleItemField IsAbout_ReportedBy { get; set; }

        public BasicSingleItemField IsAbout_PersonsName { get; set; }

        public BasicSingleItemField IsAbout_PersonsEmail { get; set; }        

        public BasicSingleItemField IsAbout_PersonsPhone { get; set; }

        public BasicSingleItemField IsAbout_PersonsCellPhone { get; set; }

        public BasicSingleItemField IsAbout_CostCentre { get; set; }

        public BasicSingleItemField IsAbout_Place { get; set; }

        public BasicSingleItemField IsAbout_UserCode { get; set; }
        
        public BasicMultiItemField IsAbout_Regions { get; set; }

        public BasicMultiItemField IsAbout_Departments { get; set; }

        public BasicMultiItemField IsAbout_OUs { get; set; }

        #endregion

        #region ComputerInfo

        public BasicSingleItemField InventoryNumber { get; set; }

        public BasicSingleItemField InventoryType { get; set; }

        public BasicSingleItemField InventoryLocation { get; set; }

        #endregion

        #region Case Info

        public BasicMultiItemField RegistrationSources { get; set; }

        public BasicMultiItemField CaseTypes { get; set; }

        public BasicMultiItemField ProductAreas { get; set; }

        public BasicMultiItemField Systems { get; set; }

        public BasicMultiItemField Urgencies { get; set; }

        public BasicMultiItemField Impacts { get; set; }

        public BasicMultiItemField Categories { get; set; }

        public BasicMultiItemField Supliers { get; set; }

        public BasicSingleItemField InvoiceNumber { get; set; }

        public BasicSingleItemField ReferenceNumber { get; set; }

        public BasicSingleItemField Caption { get; set; }

        public BasicSingleItemField Description { get; set; }

        public BasicSingleItemField Miscellaneous { get; set; }

        public BasicSingleItemField ContactBeforeAction { get; set; }

        public BasicSingleItemField SMS { get; set; }

        public BasicSingleItemField Available { get; set; }

        public BasicSingleItemField Cost { get; set; }

        public BasicSingleItemField OtherCost { get; set; }

        public BasicSingleItemField Currency { get; set; }

        public BasicSingleItemField CaseFile { get; set; }

        #endregion

        #region Other Info

        public BasicMultiItemField WorkingGroups { get; set; }

        public BasicMultiItemField Administrators { get; set; }

        public BasicMultiItemField Priorities { get; set; }

        public BasicMultiItemField Status { get; set; }

        public BasicMultiItemField SubStatus { get; set; }

        public BasicMultiItemField Projects { get; set; }

        public BasicMultiItemField Problems { get; set; }

        public BasicMultiItemField CausingParts { get; set; }

        public BasicMultiItemField Changes { get; set; }

        public BasicSingleItemField PlanDate { get; set; }

        public BasicSingleItemField WatchDate { get; set; }

        public BasicSingleItemField Verified { get; set; }

        public BasicSingleItemField VerifiedDescription { get; set; }

        public BasicMultiItemField SolutionRate { get; set; }

        #endregion

        #region Log Info

        public BasicSingleItemField ExternalLog { get; set; }

        public BasicSingleItemField InternalLog { get; set; }

        public BasicSingleItemField FinishingDescription { get; set; }

        public BasicSingleItemField LogFile { get; set; }
      
        public BasicSingleItemField FinishingDate { get; set; }
     
        public BasicMultiItemField ClosingReason { get; set; }

        #endregion

        #region Virtual Fields

        public BasicVirtualDataField Priority_Impact_Urgent { get; set; }

        #endregion
    }
}