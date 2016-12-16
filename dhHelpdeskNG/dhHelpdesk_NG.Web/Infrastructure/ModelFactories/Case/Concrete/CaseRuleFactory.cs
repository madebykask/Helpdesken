using DH.Helpdesk.Domain;
using DH.Helpdesk.Web.Models.CaseRules;
using System.Collections.Generic;
using System.Linq;
using static DH.Helpdesk.BusinessData.OldComponents.GlobalEnums;
using DH.Helpdesk.Web.Infrastructure.Extensions;
using DH.Helpdesk.Common.Extensions.Integer;
using System.Threading;

namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Case.Concrete
{
    public interface ICaseRuleFactory
    {
        CaseRuleModel GetCaseRuleModel(int customerId, CaseRuleMode ruleType, 
                                       IList<CaseFieldSetting> caseFieldSettings,
                                       BasicCaseInformation basicInformation,
                                       Setting customerSettings );
    }


    public class CaseRuleFactory: ICaseRuleFactory
    {        

        public CaseRuleFactory()
        {            
        }        

        public CaseRuleModel GetCaseRuleModel(int customerId, CaseRuleMode mode,
                                              IList<CaseFieldSetting> caseFieldSettings,
                                              BasicCaseInformation basicInformation,
                                              Setting customerSettings)
        {                        
            var ret = new CaseRuleModel();
            ret.RuleType = mode;
            ret.DateFormat = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;

            ret.FieldAttributes = GetOriginalRules(customerId, caseFieldSettings.ToList(),
                                                   basicInformation, customerSettings, mode);           
            return ret;
        }
                

        private List<FieldAttributeModel> GetOriginalRules(int customerId,
                                                           List<CaseFieldSetting> caseFieldSettings,
                                                           BasicCaseInformation basicInformation,
                                                           Setting customerSettings,
                                                           CaseRuleMode ruleType)
        {
            var ret = new List<FieldAttributeModel>();

            // *** Initiator ***
            #region Initiator

            #region ReportedBy

            var curField = TranslationCaseFields.ReportedBy.ToString();
            var attrReportedBy = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ReportedBy.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ReportedBy.StatusType
            };
            ret.Add(attrReportedBy);

            #endregion

            #region PersonsName

            curField = TranslationCaseFields.Persons_Name.ToString();
            var attrPersonName = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsName.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsName.StatusType
            };
            ret.Add(attrPersonName);

            #endregion 

            #region PersonsEmail

            curField = TranslationCaseFields.Persons_EMail.ToString();
            var attrPersonEmail = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsEmail.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsEmail.StatusType
            };
            ret.Add(attrPersonEmail);

            #endregion

            #region MailToNotifier

            curField = TranslationCaseFields.MailToNotifier.ToString();
            var attrMailToNotifier = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.MailToNotifier.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.MailToNotifier.StatusType
            };
            ret.Add(attrMailToNotifier);

            #endregion

            #region PersonsPhone

            curField = TranslationCaseFields.Persons_Phone.ToString();
            var attrPersonPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsPhone.StatusType
            };
            ret.Add(attrPersonPhone);

            #endregion

            #region PersonsCellPhone

            curField = TranslationCaseFields.Persons_CellPhone.ToString();
            var attrPersonCellPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsCellPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PersonsCellPhone.StatusType
            };
            ret.Add(attrPersonCellPhone);

            #endregion

            #region CostCentre

            curField = TranslationCaseFields.CostCentre.ToString();
            var attrCostCentre = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.CostCentre.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CostCentre.StatusType
            };
            ret.Add(attrCostCentre);

            #endregion

            #region Place

            curField = TranslationCaseFields.Place.ToString();
            var attrPlace = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Place.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Place.StatusType
            };
            ret.Add(attrPlace);

            #endregion

            #region UserCode

            curField = TranslationCaseFields.UserCode.ToString();
            var attrUserCode = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.UserCode.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.UserCode.StatusType
            };
            ret.Add(attrUserCode);

            #endregion

            #region UpdateUserInfo

            curField = TranslationCaseFields.UpdateNotifierInformation.ToString();
            var attrUpdateUserInfo = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.UpdateUserInfo.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.UpdateUserInfo.StatusType
            };
            ret.Add(attrUpdateUserInfo);

            #endregion

            #region Region

            curField = TranslationCaseFields.Region_Id.ToString();
            var attrRegion = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Regions.DefaultItem,
                Selected = basicInformation.Regions.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Regions.StatusType,
                Items = basicInformation.Regions.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.Department_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),
                        Applicable = ruleType == CaseRuleMode.TemplateMode,
                        ShowAllIfKeyIsNull = true
                    }
                }
            };
            ret.Add(attrRegion);

            #endregion

            #region Department

            curField = TranslationCaseFields.Department_Id.ToString();
            var attrDepartment = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Departments.DefaultItem,
                Selected = basicInformation.Departments.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Departments.StatusType,
                Items = basicInformation.Departments.Items,                
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.OU_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),
                        Applicable = ruleType == CaseRuleMode.TemplateMode
                    },
                    new FieldRelation() {
                        SequenceNo = 1,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Department_WatchDate.ToString(),
                        DataStore1 = TranslationCaseFields.Department_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.WatchDate.ToString(),
                        Applicable = true,
                        Conditions =  new List<FieldRelationCondition> {                            
                            new FieldRelationCondition(TranslationCaseFields.Department_Id.ToString(), ForeignKeyNum.FKeyNum0, ConditionOperator.HasValue),
                            new FieldRelationCondition(TranslationCaseFields.Priority_Id.ToString(), ForeignKeyNum.FKeyNum2, ConditionOperator.Equal, "0")
                        }
                    }
                }
            };
            ret.Add(attrDepartment);

            #endregion

            #region OU

            curField = TranslationCaseFields.OU_Id.ToString();
            var attrOU = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.OUs.DefaultItem,
                Selected = basicInformation.OUs.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.OUs.StatusType,
                Items = basicInformation.OUs.Items
            };
            ret.Add(attrOU);

            #endregion

            #endregion

            // *** IsAbout ***
            #region IsAbout

            #region IsAbout_ReportedBy

            curField = TranslationCaseFields.IsAbout_ReportedBy.ToString();
            var attrIsAbout_ReportedBy = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_ReportedBy.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_ReportedBy.StatusType
            };
            ret.Add(attrIsAbout_ReportedBy);

            #endregion

            #region IsAbout_PersonsName

            curField = TranslationCaseFields.IsAbout_Persons_Name.ToString();
            var attrIsAbout_PersonName = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_PersonsName.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsName.StatusType
            };
            ret.Add(attrIsAbout_PersonName);

            #endregion 

            #region IsAbout_PersonsEmail

            curField = TranslationCaseFields.IsAbout_Persons_EMail.ToString();
            var attrIsAbout_PersonEmail = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PersonsEmail.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsEmail.StatusType
            };
            ret.Add(attrIsAbout_PersonEmail);

            #endregion           

            #region IsAbout_PersonsPhone

            curField = TranslationCaseFields.IsAbout_Persons_Phone.ToString();
            var attrIsAbout_PersonPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_PersonsPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsPhone.StatusType
            };
            ret.Add(attrIsAbout_PersonPhone);

            #endregion

            #region IsAbout_PersonsCellPhone

            curField = TranslationCaseFields.IsAbout_Persons_CellPhone.ToString();
            var attrIsAbout_PersonCellPhone = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_PersonsCellPhone.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_PersonsCellPhone.StatusType
            };
            ret.Add(attrIsAbout_PersonCellPhone);

            #endregion

            #region IsAbout_CostCentre

            curField = TranslationCaseFields.IsAbout_CostCentre.ToString();
            var attrIsAbout_CostCentre = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_CostCentre.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_CostCentre.StatusType
            };
            ret.Add(attrIsAbout_CostCentre);

            #endregion

            #region IsAbout_Place

            curField = TranslationCaseFields.IsAbout_Place.ToString();
            var attrIsAbout_Place = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_Place.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_Place.StatusType
            };
            ret.Add(attrIsAbout_Place);

            #endregion

            #region IsAbout_UserCode

            curField = TranslationCaseFields.IsAbout_UserCode.ToString();
            var attrIsAbout_UserCode = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.IsAbout_UserCode.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_UserCode.StatusType
            };
            ret.Add(attrIsAbout_UserCode);

            #endregion            

            #region IsAbout_Region

            curField = TranslationCaseFields.IsAbout_Region_Id.ToString();
            var attrIsAbout_Region = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.IsAbout_Regions.DefaultItem,
                Selected = basicInformation.IsAbout_Regions.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_Regions.StatusType,
                Items = basicInformation.IsAbout_Regions.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.IsAbout_Department_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),
                        Applicable = ruleType == CaseRuleMode.TemplateMode,
                        ShowAllIfKeyIsNull = true
                    }
                }
            };
            ret.Add(attrIsAbout_Region);

            #endregion

            #region IsAbout_Department

            curField = TranslationCaseFields.IsAbout_Department_Id.ToString();
            var attrIsAbout_Department = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.IsAbout_Departments.DefaultItem,
                Selected = basicInformation.IsAbout_Departments.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_Departments.StatusType,
                Items = basicInformation.IsAbout_Departments.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToMany.ToInt(),
                        ActionType = RelationActionType.ListPopulator.ToInt(),
                        FieldId = TranslationCaseFields.IsAbout_OU_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),
                        Applicable = ruleType == CaseRuleMode.TemplateMode
                    }
                }
            };
            ret.Add(attrIsAbout_Department);

            #endregion

            #region IsAbout_OU

            curField = TranslationCaseFields.IsAbout_OU_Id.ToString();
            var attrIsAbout_OU = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.IsAbout_OUs.DefaultItem,
                Selected = basicInformation.IsAbout_OUs.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.IsAbout_OUs.StatusType,
                Items = basicInformation.IsAbout_OUs.Items
            };
            ret.Add(attrIsAbout_OU);

            #endregion

            #endregion

            // *** Computer Info ***
            #region Computer Info

            #region InventoryNumber

            curField = TranslationCaseFields.InventoryNumber.ToString();
            var attrInventoryNumber = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InventoryNumber.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InventoryNumber.StatusType
            };
            ret.Add(attrInventoryNumber);

            #endregion

            #region InventoryType

            curField = TranslationCaseFields.ComputerType_Id.ToString();
            var attrInventoryType = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InventoryType.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InventoryType.StatusType
            };
            ret.Add(attrInventoryType);

            #endregion 

            #region InventoryLocation

            curField = TranslationCaseFields.InventoryLocation.ToString();
            var attrInventoryLocation = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InventoryLocation.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InventoryLocation.StatusType
            };
            ret.Add(attrInventoryLocation);

            #endregion

            #endregion

            // *** Case Info ***
            #region Case Info

            #region RegistrationSource

            curField = TranslationCaseFields.RegistrationSourceCustomer.ToString();
            var attrRegistrationSource = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.RegistrationSources.DefaultItem,
                Selected = basicInformation.RegistrationSources.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.RegistrationSources.StatusType,
                Items = basicInformation.RegistrationSources.Items
            };
            ret.Add(attrRegistrationSource);

            #endregion

            #region CaseType

            curField = TranslationCaseFields.CaseType_Id.ToString();
            var attrCaseType = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TreeButtonSelect,
                DefaultItem = basicInformation.CaseTypes.DefaultItem,
                Selected = basicInformation.CaseTypes.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CaseTypes.StatusType,
                Items = basicInformation.CaseTypes.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.Performer_User_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    }
                }

            };
            ret.Add(attrCaseType);

            #endregion 

            #region ProductArea

            curField = TranslationCaseFields.ProductArea_Id.ToString();
            var attrProductArea = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TreeButtonSelect,
                DefaultItem = basicInformation.ProductAreas.DefaultItem,
                Selected = basicInformation.ProductAreas.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ProductAreas.StatusType,
                Items = basicInformation.ProductAreas.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.WorkingGroup_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },
                    new FieldRelation() {
                        SequenceNo = 1,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.Priority_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }
                }
            };
            ret.Add(attrProductArea);

            #endregion 

            #region System

            curField = TranslationCaseFields.System_Id.ToString();
            var attrSystem = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Systems.DefaultItem,
                Selected = basicInformation.Systems.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Systems.StatusType,
                Items = basicInformation.Systems.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.Urgency_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    }                 
                }
            };
            ret.Add(attrSystem);

            #endregion

            #region Urgency

            curField = TranslationCaseFields.Urgency_Id.ToString();
            var attrUrgency = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Urgencies.DefaultItem,
                Selected = basicInformation.Urgencies.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Urgencies.StatusType,
                Items = basicInformation.Urgencies.Items,
                Relations = new List<FieldRelation> {                                      
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Priority_Impact_Urgent.ToString(),
                        DataStore1 = TranslationCaseFields.Impact_Id.ToString(),
                        DataStore2 = TranslationCaseFields.Urgency_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.Priority_Id.ToString()
                    }
                }
            };
            ret.Add(attrUrgency);

            #endregion

            #region Impact

            curField = TranslationCaseFields.Impact_Id.ToString();
            var attrImpact = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Impacts.DefaultItem,
                Selected = basicInformation.Impacts.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Impacts.StatusType,
                Items = basicInformation.Impacts.Items,
                Relations = new List<FieldRelation> {                   
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Priority_Impact_Urgent.ToString(),
                        DataStore1 = TranslationCaseFields.Impact_Id.ToString(),
                        DataStore2 = TranslationCaseFields.Urgency_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.Priority_Id.ToString()
                    }
                }
            };
            ret.Add(attrImpact);

            #endregion

            #region Category

            curField = TranslationCaseFields.Category_Id.ToString();
            var attrCategory = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Categories.DefaultItem,
                Selected = basicInformation.Categories.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Categories.StatusType,
                Items = basicInformation.Categories.Items
            };
            ret.Add(attrCategory);

            #endregion

            #region Supplier

            curField = TranslationCaseFields.Supplier_Id.ToString();
            var attrSupplier = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Supliers.DefaultItem,
                Selected = basicInformation.Supliers.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Supliers.StatusType,
                Items = basicInformation.Supliers.Items
            };
            ret.Add(attrSupplier);

            #endregion

            #region InvoiceNumber

            curField = TranslationCaseFields.InvoiceNumber.ToString();
            var attrInvoiceNumber = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InvoiceNumber.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InvoiceNumber.StatusType
            };
            ret.Add(attrInvoiceNumber);

            #endregion

            #region ReferenceNumber

            curField = TranslationCaseFields.ReferenceNumber.ToString();
            var attrReferenceNumber = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ReferenceNumber.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ReferenceNumber.StatusType
            };
            ret.Add(attrReferenceNumber);

            #endregion

            #region Caption

            curField = TranslationCaseFields.Caption.ToString();
            var attrCaption = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Caption.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Caption.StatusType
            };
            ret.Add(attrCaption);

            #endregion

            #region Description

            curField = TranslationCaseFields.Description.ToString();
            var attrDescription = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Description.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InvoiceNumber.StatusType
            };
            ret.Add(attrDescription);

            #endregion

            #region Miscellaneous

            curField = TranslationCaseFields.Miscellaneous.ToString();
            var attrMiscellaneous = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Miscellaneous.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Miscellaneous.StatusType
            };
            ret.Add(attrMiscellaneous);

            #endregion

            #region ContactBeforeAction

            curField = TranslationCaseFields.ContactBeforeAction.ToString();
            var attrContactBeforeAction = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ContactBeforeAction.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ContactBeforeAction.StatusType
            };
            ret.Add(attrContactBeforeAction);

            #endregion

            #region SMS

            curField = TranslationCaseFields.SMS.ToString();
            var attrSMS = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.SMS.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.SMS.StatusType
            };
            ret.Add(attrSMS);

            #endregion

            #region Available

            curField = TranslationCaseFields.Available.ToString();
            var attrAvailable = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Available.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Available.StatusType
            };
            ret.Add(attrAvailable);

            #endregion

            #region Cost

            curField = TranslationCaseFields.Cost.ToString();
            var attrCost = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Cost.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Cost.StatusType
            };
            ret.Add(attrCost);

            curField = TranslationCaseFields.Cost.ToString();
            var attrOtherCost = new FieldAttributeModel()
            {
                FieldId = "OtherCost",
                FieldName = "OtherCost",
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.OtherCost.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.OtherCost.StatusType
            };
            ret.Add(attrOtherCost);

            curField = TranslationCaseFields.Cost.ToString();
            var attrCurrency = new FieldAttributeModel()
            {
                FieldId = "Currency",
                FieldName = "Currency",
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Currency.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Currency.StatusType
            };
            ret.Add(attrCurrency);
            #endregion

            #region CaseFile

            curField = TranslationCaseFields.Filename.ToString();
            var attrFilename = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.ButtonField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CaseFile.StatusType
            };
            ret.Add(attrFilename);

            #endregion

            #endregion

            // *** Other Info ***
            #region Other Info

            #region WorkingGroup

            curField = TranslationCaseFields.WorkingGroup_Id.ToString();
            var attrWorkingGroup = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.WorkingGroups.DefaultItem,
                Selected = basicInformation.WorkingGroups.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.WorkingGroups.StatusType,
                Items = basicInformation.WorkingGroups.Items,
                Relations = new List<FieldRelation> {                    
                    new FieldRelation() {
                        SequenceNo = 1,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.StateSecondary_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }
                }
            };

            if (customerSettings != null && customerSettings.DontConnectUserToWorkingGroup == 0)
            {
                attrWorkingGroup.Relations.Add(
                                                new FieldRelation()
                                                {
                                                    SequenceNo = 0,
                                                    RelationType = RelationType.ManyToMany.ToInt(),
                                                    ActionType = RelationActionType.ListPopulator.ToInt(),
                                                    FieldId = TranslationCaseFields.Performer_User_Id.ToString(),
                                                    ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt(),
                                                    Applicable = ruleType == CaseRuleMode.TemplateMode,
                                                    ShowRunTimeCurrentValue = ruleType == CaseRuleMode.TemplateMode
                                                });
            }            

            ret.Add(attrWorkingGroup);

            #endregion

            #region Performer_User

            curField = TranslationCaseFields.Performer_User_Id.ToString();
            var attrPerformer_User = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Administrators.DefaultItem,
                Selected = basicInformation.Administrators.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Administrators.StatusType,
                Items = basicInformation.Administrators.Items
            };
            ret.Add(attrPerformer_User);

            #endregion 

            #region Priority

            curField = TranslationCaseFields.Priority_Id.ToString();
            var attrPriority = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Priorities.DefaultItem,
                Selected = basicInformation.Priorities.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Priorities.StatusType,
                Items = basicInformation.Priorities.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                            SequenceNo = 1,
                            RelationType = RelationType.OneToOne.ToInt(),
                            ActionType = RelationActionType.ValueSetter.ToInt(),
                            FieldId = TranslationCaseFields.tblLog_Text_External.ToString(),
                            ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },

                    new FieldRelation() {
                        SequenceNo = 1,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Department_WatchDate.ToString(),
                        DataStore1 = TranslationCaseFields.Department_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.WatchDate.ToString(),
                        Conditions =  new List<FieldRelationCondition> {                            
                            new FieldRelationCondition(TranslationCaseFields.Department_Id.ToString(), ForeignKeyNum.FKeyNum0, ConditionOperator.HasValue),
                            new FieldRelationCondition(TranslationCaseFields.Priority_Id.ToString(), ForeignKeyNum.FKeyNum2, ConditionOperator.Equal, "0")
                        }
                    }
                }
            };
            ret.Add(attrPriority);

            #endregion

            #region Status

            curField = TranslationCaseFields.Status_Id.ToString();
            var attrStatus = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Status.DefaultItem,
                Selected = basicInformation.Status.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Status.StatusType,
                Items = basicInformation.Status.Items,
                Relations = new List<FieldRelation> {
                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.WorkingGroup_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    },
                    new FieldRelation() {
                        SequenceNo = 1,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.StateSecondary_Id.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt()
                    }
                }
            };
            ret.Add(attrStatus);

            #endregion

            #region SubStatus

            curField = TranslationCaseFields.StateSecondary_Id.ToString();
            var attrStateSecondary = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.SubStatus.DefaultItem,
                Selected = basicInformation.SubStatus.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.SubStatus.StatusType,
                Items = basicInformation.SubStatus.Items,
                Relations = new List<FieldRelation> {

                    /* Disabled because it makes cycle */
                    //new FieldRelation() {
                    //    SequenceNo = 0,
                    //    RelationType = RelationType.OneToOne.ToInt(),
                    //    ActionType = RelationActionType.ValueSetter.ToInt(),
                    //    FieldId = TranslationCaseFields.WorkingGroup_Id.ToString(),
                    //    ForeignKeyNumber = ForeignKeyNum.FKeyNum1.ToInt()
                    //},

                    new FieldRelation() {
                        SequenceNo = 0,
                        RelationType = RelationType.Virtual.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = VirtualFields.Department_WatchDate.ToString(),
                        DataStore1 = TranslationCaseFields.Department_Id.ToString(),
                        ResultDataKey = TranslationCaseFields.WatchDate.ToString(),
                        Conditions =  new List<FieldRelationCondition> {
                            new FieldRelationCondition(TranslationCaseFields.StateSecondary_Id.ToString(), ForeignKeyNum.FKeyNum3, ConditionOperator.NotEqual, "0"),
                            new FieldRelationCondition(TranslationCaseFields.Department_Id.ToString(), ForeignKeyNum.FKeyNum0, ConditionOperator.HasValue),
                            new FieldRelationCondition(TranslationCaseFields.Priority_Id.ToString(), ForeignKeyNum.FKeyNum2, ConditionOperator.Equal, "0")
                        }
                    },

                    new FieldRelation() {
                        SequenceNo = 1,
                        RelationType = RelationType.OneToOne.ToInt(),
                        ActionType = RelationActionType.ValueSetter.ToInt(),
                        FieldId = TranslationCaseFields.MailToNotifier.ToString(),
                        ForeignKeyNumber = ForeignKeyNum.FKeyNum2.ToInt(),
                    }                    
                }
            };
            ret.Add(attrStateSecondary);

            #endregion

            #region Project

            curField = TranslationCaseFields.Project.ToString();
            var attrProject = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Projects.DefaultItem,
                Selected = basicInformation.Projects.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Projects.StatusType,
                Items = basicInformation.Projects.Items
            };
            ret.Add(attrProject);

            #endregion

            #region Problem

            curField = TranslationCaseFields.Problem.ToString();
            var attrProblem = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Problems.DefaultItem,
                Selected = basicInformation.Problems.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Problems.StatusType,
                Items = basicInformation.Problems.Items
            };
            ret.Add(attrProblem);

            #endregion

            #region CausingPart

            curField = TranslationCaseFields.CausingPart.ToString();
            var attrCausingPart = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.CausingParts.DefaultItem,
                Selected = basicInformation.CausingParts.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.CausingParts.StatusType,
                Items = basicInformation.CausingParts.Items
            };
            ret.Add(attrCausingPart);

            #endregion

            #region Change

            curField = TranslationCaseFields.Change.ToString();
            var attrChange = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.Changes.DefaultItem,
                Selected = basicInformation.Changes.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Changes.StatusType,
                Items = basicInformation.Changes.Items
            };
            ret.Add(attrChange);

            #endregion

            #region PlanDate

            curField = TranslationCaseFields.PlanDate.ToString();
            var attrPlanDate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.PlanDate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.PlanDate.StatusType
            };
            ret.Add(attrPlanDate);

            #endregion

            #region WatchDate

            curField = TranslationCaseFields.WatchDate.ToString();
            var attrWatchDate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.WatchDate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.WatchDate.StatusType
            };
            ret.Add(attrWatchDate);

            #endregion

            #region Verified

            curField = TranslationCaseFields.Verified.ToString();
            var attrVerified = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.CheckBox,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.Verified.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.Verified.StatusType
            };
            ret.Add(attrVerified);

            #endregion

            #region VerifiedDescription

            curField = TranslationCaseFields.VerifiedDescription.ToString();
            var attrVerifiedDescription = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.VerifiedDescription.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.VerifiedDescription.StatusType
            };
            ret.Add(attrVerifiedDescription);

            #endregion

            #region SolutionRate

            curField = TranslationCaseFields.SolutionRate.ToString();
            var attrSolutionRate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = basicInformation.SolutionRate.DefaultItem,
                Selected = basicInformation.SolutionRate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.SolutionRate.StatusType,
                Items = basicInformation.SolutionRate.Items
            };
            ret.Add(attrSolutionRate);

            #endregion

            #endregion

            // *** Log Info ***
            #region Log Info

            #region ExternalLog

            curField = TranslationCaseFields.tblLog_Text_External.ToString();
            var attrExternalLog = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.ExternalLog.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ExternalLog.StatusType
            };
            ret.Add(attrExternalLog);

            #endregion

            #region Internal Log

            curField = TranslationCaseFields.tblLog_Text_Internal.ToString();
            var attrInternalLog = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.InternalLog.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.InternalLog.StatusType
            };
            ret.Add(attrInternalLog);

            #endregion 

            #region FinishingDescription

            curField = TranslationCaseFields.FinishingDescription.ToString();
            var attrFinishingDescription = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TextArea,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.FinishingDescription.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.FinishingDescription.StatusType
            };
            ret.Add(attrFinishingDescription);

            #endregion

            #region Log File

            curField = TranslationCaseFields.tblLog_Filename.ToString();
            var attrLogFile = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.ButtonField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.LogFile.StatusType
            };
            ret.Add(attrLogFile);

            #endregion

            #region FinishingDate

            curField = TranslationCaseFields.FinishingDate.ToString();
            var attrFinishingDate = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = basicInformation.FinishingDate.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.FinishingDate.StatusType
            };
            ret.Add(attrFinishingDate);

            #endregion

            #region ClosingReason

            curField = TranslationCaseFields.ClosingReason.ToString();
            var attrClosingReason = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(curField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.TreeButtonSelect,
                DefaultItem = basicInformation.ClosingReason.DefaultItem,
                Selected = basicInformation.ClosingReason.Selected,
                IsAvailableOnHelpdesk = caseFieldSettings.getShowOnStartPage(curField).ToBool(),
                IsAvailableOnSelfService = caseFieldSettings.getShowExternal(curField).ToBool(),
                IsMandatory = caseFieldSettings.getRequired(curField).ToBool(),
                StatusType = basicInformation.ClosingReason.StatusType,
                Items = basicInformation.ClosingReason.Items
            };
            ret.Add(attrClosingReason);

            #endregion

            #endregion

            // *** Virtual Data ***
            #region Virtual Data

            #region Priority_Impact_Urgent

            curField = VirtualFields.Priority_Impact_Urgent.ToString();
            var resultField = TranslationCaseFields.Priority_Id.ToString(); 

            var attrVirtual1 = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(resultField, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.SingleSelectField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = true,
                IsAvailableOnSelfService = true,
                IsMandatory = false,
                Items = basicInformation.Priority_Impact_Urgent.Items,
                StatusType = CaseFieldStatusType.Readonly
            };
            ret.Add(attrVirtual1);

            #endregion

            #region Department_WatchDate

            curField = VirtualFields.Department_WatchDate.ToString();
            var resultField_WD = TranslationCaseFields.WatchDate.ToString();

            var attrVirtual2 = new FieldAttributeModel()
            {
                FieldId = curField,
                FieldName = curField,
                FieldCaption = Translation.Get(resultField_WD, Enums.TranslationSource.CaseTranslation, customerId),
                FieldType = CaseFieldType.DateField,
                DefaultItem = new FieldItem(string.Empty, string.Empty, true),
                Selected = new FieldItem(string.Empty, string.Empty, true),
                IsAvailableOnHelpdesk = true,
                IsAvailableOnSelfService = true,
                IsMandatory = false,
                Items = basicInformation.Department_WatchDate.Items,
                StatusType = CaseFieldStatusType.Readonly
            };
            ret.Add(attrVirtual2);

            #endregion

            #endregion

            return ret;

        }        
    }
}