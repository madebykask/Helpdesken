using DH.Helpdesk.BusinessData.Enums.Case;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.CaseSections;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.BusinessData.Models.User;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain.Cases;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Problems;
    using DH.Helpdesk.Web.Models.Case;
    using DH.Helpdesk.Common.Enums.Settings;
    using Models.CaseSolution;
    using System;

    public static class ObjectExtensions
    {
        public static string ToJavaScriptObject(this object obj)
        {
            if(obj == null)
                return string.Empty;

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        public static int GetPriority(IList<PriorityImpactUrgency> lst, int urgency, int impact)
        {
            var piu = lst.Where(x => x.Urgency_Id.GetValueOrDefault(0) == urgency && x.Impact_Id.GetValueOrDefault(0) == impact).FirstOrDefault();
            if (piu == null)
                return 0;
            return piu.Priority_Id;
        }


        public static WorkingGroupEntity notAssignedWorkingGroup()
        {
			var notAssigned = notAssignedGeneric();
			return new WorkingGroupEntity { Id = notAssigned.Key, WorkingGroupName = notAssigned.Value, IsActive = 1 };
        }

        public static CustomerUserInfo notAssignedPerformer()
        {
			var notAssigned = notAssignedGeneric();
			return new CustomerUserInfo { Id = notAssigned.Key, FirstName = notAssigned.Value, SurName="", IsActive = 1 , Performer = 1};
        }

		public static Priority notAssignedPriority()
		{
			var notAssigned = notAssignedGeneric();
			return new Priority { Id = notAssigned.Key, Name = notAssigned.Value, IsActive = 1 };
		}

		public static Status notAssignedStatus()
		{
			var notAssigned = notAssignedGeneric();
			return new Status { Id = notAssigned.Key, Name = notAssigned.Value, IsActive = 1 };
		}

		public static StateSecondary notAssignedStateSecondary()
		{
			var notAssigned = notAssignedGeneric();
			return new StateSecondary { Id = notAssigned.Key, Name = notAssigned.Value, IsActive = 1 };
		}

		public static Department notAssignedDepartment()
		{
			var notAssigned = notAssignedGeneric();
			return new Department { Id = notAssigned.Key, DepartmentName = notAssigned.Value, IsActive = 1 };
		}

		public static Region notAssignedRegion()
		{
			var notAssigned = notAssignedGeneric();
			return new Region { Id = notAssigned.Key, Name = notAssigned.Value, IsActive = 1 };
		}


		private static KeyValuePair<int, string> notAssignedGeneric()
		{
			return new KeyValuePair<int, string>(int.MinValue, "-- " + Translation.Get("Ej Tilldelade", Enums.TranslationSource.TextTranslation) + " --");
		}

		public static IList<Field> GetFilterForCases(int followUpPermission, int customerId)
        {
            var ret = new List<Field>();
            ret.Add(new Field { Id = 2, StringValue = Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 1, StringValue = Translation.Get("Avslutade ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 4, StringValue = Translation.Get("Olästa ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 3, StringValue = Translation.Get("Vilande ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 7, StringValue = Translation.Get("Ärenden med", Enums.TranslationSource.TextTranslation) + " " + Translation.Get(GlobalEnums.TranslationCaseFields.WatchDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) });
            if (followUpPermission == 1)
            {
                ret.Add(
                    new Field
                        {
                            Id = 8,
                            StringValue =
                                Translation.Get("För uppföljning", Enums.TranslationSource.TextTranslation)
                        });
            }

            ret.Add(new Field { Id = -1, StringValue = Translation.Get("Alla") });
            return ret;
        }

        public static IList<Field> GetFilterForAdvancedSearch()
        {
            var ret = new List<Field>();
            ret.Add(new Field { Id = (int)CaseProgressFilterEnum.None, StringValue = string.Empty });
            ret.Add(new Field { Id = (int)CaseProgressFilterEnum.CasesInProgress,
                StringValue = Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = (int)CaseProgressFilterEnum.ClosedCases,
                StringValue = Translation.Get("Avslutade ärenden", Enums.TranslationSource.TextTranslation) });                        
            return ret;
        }        

        public static int getPriorityMaxtime(this IList<Priority> pl)
        {
            var ret = 0; 

            if (pl != null)
            {
                foreach (var p in pl)
                {
                    if (p.SolutionTime > ret) 
                        ret = p.SolutionTime; 
                }
            }

            return ret;
        }

        public static int getWorkingDayLength(this Customer c)
        {
            var ret = 0;

            if (c != null)
                ret = c.WorkingDayEnd - c.WorkingDayStart; 

            return ret;
        }

        public static bool IsFieldRequiredOrVisible(this IList<CaseFieldSetting> fields, GlobalEnums.TranslationCaseFields field)
        {
            var isVisible = fields.IsFieldVisible(field);
            var isRequired = fields.IsFieldRequired(field);
            return isVisible || isRequired;
        }

        public static bool IsFieldVisible(this IList<CaseFieldSetting> fields, GlobalEnums.TranslationCaseFields field)
        {
            var fs = fields.getCaseSettingsValue(field.ToString());

            //some fields have Hide setting available but not all! 
            if (GlobalEnums.FieldsWithHide.Contains(field))
            {
                return fs.IsActive && !fs.Hide;
            }
            return fs.IsActive;
        }

        public static bool IsFieldRequired(this IList<CaseFieldSetting> fields, GlobalEnums.TranslationCaseFields field)
        {
            var fieldSettings = fields.getCaseSettingsValue(field.ToString());
            return fieldSettings.Required != 0;
        }

        public static bool IsFieldLocked(this IList<CaseFieldSetting> fields, GlobalEnums.TranslationCaseFields field)
        {
            return fields.getCaseSettingsValue(field.ToString()).Locked == 1;
        }

        public static bool ShowOnPage(this IList<CaseFieldSetting> cfs, GlobalEnums.TranslationCaseFields field)
        {
            var fieldSettings = cfs.getCaseSettingsValue(field.ToString());
            return (fieldSettings?.ShowOnStartPage ?? 0) == 1;
        }

        public static CaseFieldSetting getCaseSettingsValue(this IList<CaseFieldSetting> cfs, string valueToFind)
        {
            var ret = new CaseFieldSetting();  

            if (cfs != null)
            {
                foreach (CaseFieldSetting c in cfs)
                {
                    if (string.Equals(c.Name, valueToFind.getCaseFieldName(), StringComparison.OrdinalIgnoreCase))
                    {
                        ret = c;
                        break;
                    }
                }
            }

            return ret;
        }


        public static int CaseFieldSettingRequiredCheck(this IList<CaseFieldSetting> cfs, string valueToFind, bool isCaseReopend)
        {
            int ret = 0;
            if (cfs != null)
                foreach (CaseFieldSetting c in cfs)
                {
                    if (string.Compare(c.Name, valueToFind.getCaseFieldName(), true) == 0)
                    {
                        if (isCaseReopend && c.RequiredIfReopened == 1 && c.ShowOnStartPage == 1)
                        {
                            ret = 2;
                        }
                        else if ((c.Required == 1 || (isCaseReopend && c.RequiredIfReopened == 1)) && c.ShowOnStartPage == 1)
                        {
                            ret = 1;
                        }
                        break;
                    }
                }

            return ret;
        }

        public static CaseFieldSettingsWithLanguage getCaseFieldSettingsLanguageValue(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            var ret = new CaseFieldSettingsWithLanguage();

            if (cfsl != null)
            {
                foreach (CaseFieldSettingsWithLanguage c in cfsl)
                {
                    if (string.Compare(c.Name, valueToFind.getCaseFieldName(), true) == 0)
                    {
                        ret = c;
                        break;
                    }
                }
            }

            return ret;
        }

        public static string displayHtml(this IList<CaseFieldSetting> cfs, string valueToFind)
        {
            var isVisible = cfs.getCaseSettingsValue(valueToFind).IsActive;
            return !isVisible ? "display:none" : string.Empty;
        }

        public static bool getHide(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).Hide;
        }

        public static int getShowOnStartPage(this IList<CaseFieldSetting> cfs, string valueToFind)
        {
           return cfs.getCaseSettingsValue(valueToFind).ShowOnStartPage;
        }

        public static int getLocked(this IList<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.getCaseSettingsValue(valueToFind).Locked;
        }

        public static int getShowExternal(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).ShowExternal;
        }

        public static int getRequired(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).Required;
        }
        
        public static int getRequiredIfReopened(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).RequiredIfReopened;
        }

        public static int getFieldSize(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).FieldSize;
        }

        public static string getDefaultValue(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).DefaultValue;
        }

        public static string getEMailIdentifier(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).EMailIdentifier;
        }

        public static Guid? getCaseFieldSettingsGUID(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).CaseFieldSettingsGUID;
        }

        public static string getLabel(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            return cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).Label;
        }

        public static string getDefaultLabel(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind, string defaultValue)
        {
            var label = cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).Label;
            if (!string.IsNullOrEmpty(label))
                return label;
            return Translation.GetCoreTextTranslation(defaultValue);
        }

        public static string getFieldHelp(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            return cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).FieldHelp;
        }
        public static string getEMailIdentifier(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            return cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).EMailIdentifier;
        }

        public static string getCaseFieldSettingWithLanguageId(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            return cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).Id.ToString();
        }

        public static string getCaseFieldSettingId(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).Id.ToString();
        }

        public static string getCaseFieldSettingWithLanguage_LanguageId(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            return cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).Language_Id.ToString();
        }

        public static string displayUserInfoHtml(this CaseInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.caseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ReportedBy.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.ReportedBy, CaseSolutionFields.UserNumber)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Persons_EMail, CaseSolutionFields.Email)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_Name.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Persons_Name, CaseSolutionFields.PersonsName)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_Phone.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Persons_Phone, CaseSolutionFields.PersonsPhone)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Persons_CellPhone, CaseSolutionFields.PersonsCellPhone)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Persons_EMail, CaseSolutionFields.PersonsEmail)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Region_Id.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Region_Id, CaseSolutionFields.Region)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Department_Id.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Department_Id, CaseSolutionFields.Department)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Place.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Place, CaseSolutionFields.Place)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.OU_Id.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.OU_Id, CaseSolutionFields.OU)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.UserCode.ToString()).ShowOnStartPage == 1 &&
                     string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.UserCode, CaseSolutionFields.Usercode)))
                return string.Empty;           

            return ret;
        }

        public static string displayUserInfoHtml(this CaseSolutionInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.CaseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ReportedBy.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_Name.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_Phone.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Persons_EMail.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Region_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Department_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Place.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.OU_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.UserCode.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            return ret;
        }
        public static string displayAboutUserInfoHtml(this CaseInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.caseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy, CaseSolutionFields.IsAbout_ReportedBy)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name, CaseSolutionFields.IsAbout_PersonsName)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail, CaseSolutionFields.IsAbout_PersonsEmail)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone, CaseSolutionFields.IsAbout_PersonsPhone)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone, CaseSolutionFields.IsAbout_PersonsCellPhone)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_Region_Id, CaseSolutionFields.IsAbout_Region_Id)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_Department_Id, CaseSolutionFields.IsAbout_Department_Id)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_OU_Id, CaseSolutionFields.IsAbout_OU_Id)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_CostCentre, CaseSolutionFields.IsAbout_CostCentre)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString()).ShowOnStartPage == 1 &&
              string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_Place, CaseSolutionFields.IsAbout_Place)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.IsAbout_UserCode, CaseSolutionFields.IsAbout_UserCode)))
                return string.Empty;

            return ret;

            //var ret = "display:none";

            //if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;

            //return ret;
        }

        public static string displayAboutUserInfoHtml(this CaseSolutionInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.CaseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            return ret;

        }

        public static string displayComputerInfoHtml(this CaseSolutionInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.CaseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.InventoryNumber.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.InventoryLocation.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            return ret;
        }

        public static string displayComputerInfoHtml(this CaseInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.caseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.ComputerType_Id, CaseSolutionFields.InventoryType)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.InventoryNumber.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.InventoryNumber, CaseSolutionFields.InventoryNumber)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.InventoryLocation.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.InventoryLocation, CaseSolutionFields.InventoryLocation)))
                return string.Empty;

            return ret;
        }

        public static string displayCaseInfoHtml(this CaseInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.caseFieldSettings;

            /* Not implemented in Case Template*/
            //if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.CaseNumber.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;

            /* Not implemented in Case Template*/
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.User_Id.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer, CaseSolutionFields.RegistrationSourceCustomer)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.CaseType_Id, CaseSolutionFields.CaseType)))
                return string.Empty;            

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.ProductArea_Id, CaseSolutionFields.ProductArea)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.System_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.System_Id, CaseSolutionFields.System)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Urgency_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Urgency_Id, CaseSolutionFields.Urgency)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Impact_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Impact_Id, CaseSolutionFields.Impact)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1 &&
              string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Category_Id, CaseSolutionFields.Category)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Supplier_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Supplier_Id, CaseSolutionFields.Supplier)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString()).ShowOnStartPage == 1 &&
              string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.InvoiceNumber, CaseSolutionFields.InvoiceNumber)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.ReferenceNumber, CaseSolutionFields.ReferenceNumber)))
                return string.Empty;            

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Caption.ToString()).ShowOnStartPage == 1 &&
                    string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Caption, CaseSolutionFields.Caption)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Description.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Description, CaseSolutionFields.Description)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString()).ShowOnStartPage == 1 &&
              string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.ContactBeforeAction, CaseSolutionFields.ContactBeforeAction)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Miscellaneous.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Miscellaneous, CaseSolutionFields.Miscellaneous)))
                return string.Empty;

            /* Not implemented in Case Template*/
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.AgreedDate.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.SMS.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.SMS, CaseSolutionFields.SMS)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Available.ToString()).ShowOnStartPage == 1 &&
              string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Available, CaseSolutionFields.Available)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Cost.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Cost, CaseSolutionFields.Cost)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Filename.ToString()).ShowOnStartPage == 1 &&
              string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Filename, CaseSolutionFields.FileName)))
                return string.Empty;

            return ret;
        }

        public static string displayCaseInfoHtml(this CaseSolutionInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.CaseFieldSettings;

            /* Not implemented in Case Template*/
            //if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.CaseNumber.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;

            /* Not implemented in Case Template*/
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.User_Id.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.CaseType_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.System_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Urgency_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Impact_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Category_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Supplier_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Caption.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Description.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ContactBeforeAction.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Miscellaneous.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            /* Not implemented in Case Template*/
            //else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.AgreedDate.ToString()).ShowOnStartPage == 1)
            //    return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.SMS.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Available.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Cost.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Filename.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            return ret;
        }

        public static string displayCaseManagementInfoHtml(this CaseInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.caseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.WorkingGroup_Id, CaseSolutionFields.WorkingGroup)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Performer_User_Id, CaseSolutionFields.Administrator)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Priority_Id, CaseSolutionFields.Priority)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Status_Id, CaseSolutionFields.Status)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.StateSecondary_Id, CaseSolutionFields.StateSecondary)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Project.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Project, CaseSolutionFields.Project)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Problem.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Problem, CaseSolutionFields.Problem)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.CausingPart.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.CausingPart, CaseSolutionFields.CausingPart)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Change.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Change, CaseSolutionFields.Change)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.PlanDate.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.PlanDate, CaseSolutionFields.PlanDate)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.WatchDate.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.WatchDate, CaseSolutionFields.WatchDate)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Verified.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.Verified, CaseSolutionFields.Verified)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.VerifiedDescription, CaseSolutionFields.VerifiedDescription)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.SolutionRate.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.SolutionRate, CaseSolutionFields.SolutionRate)))
                return string.Empty;
          
            return ret;
        }

        public static string displayCaseManagementInfoHtml(this CaseSolutionInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.CaseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Priority_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Status_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Project.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Problem.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.CausingPart.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Change.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.PlanDate.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.WatchDate.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.Verified.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.SolutionRate.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            return ret;
        }
        public static string displayLogInfoHtml(this CaseInputViewModel model)
        {
            var ret = "display:none";
            var cfs = model.caseFieldSettings;

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.tblLog_Text_External.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.tblLog_Text_External, CaseSolutionFields.ExternalLogNote)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.tblLog_Text_Internal.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.tblLog_Text_Internal, CaseSolutionFields.InternalLogNote)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.FinishingDescription.ToString()).ShowOnStartPage == 1 &&
                string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.FinishingDescription, CaseSolutionFields.FinishingDescription)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.FinishingDate.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.FinishingDate, CaseSolutionFields.FinishingDate)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ClosingReason.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.ClosingReason, CaseSolutionFields.FinishingCause)))
                return string.Empty;

            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.tblLog_Filename.ToString()).ShowOnStartPage == 1 &&
               string.IsNullOrEmpty(model.GetFieldStyle(GlobalEnums.TranslationCaseFields.tblLog_Filename, CaseSolutionFields.LogFileName)))
                return string.Empty;

            return ret;
        }

        public static bool GetIsAboutEnabled(this IList<CaseFieldSetting> cfs)
        {
            return getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString()).ShowOnStartPage == 1
                    || getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString()).ShowOnStartPage == 1;
        }

        public static CaseSectionModel GetCaseSection(this IEnumerable<CaseSectionModel> lst, CaseSectionType type)
        {
            var section = lst.SingleOrDefault(x => x.SectionType == (int) type);
            return section != null ? section : new CaseSectionModel
            {
                SectionType = (int)type
            };
        }

        public static string getSectionClass(this CaseInputViewModel model, CaseSectionType type)
        {
            var section = model.CaseSectionModels.SingleOrDefault(x => x.SectionType == (int)type);
            var result = string.Empty;
            if (section != null)
            {
                if (model.case_.IsNew() && section.IsNewCollapsed)
                    result = "hideshow";
                if (!model.case_.IsNew() && section.IsEditCollapsed)
                    result = "hideshow";
            }
            return result;
        }

        public static string getSearchEmailClass(this CaseInputViewModel model, CaseSectionType type)
        {
            var section = model.CaseSectionModels.SingleOrDefault(x => x.SectionType == (int)type);
            var result = string.Empty;
            if (section != null)
            {
                if (model.case_.IsNew() && section.IsNewCollapsed)
                    result = "hidefollowers";
                if (!model.case_.IsNew() && section.IsEditCollapsed)
                    result = "hidefollowers";
            }
            return result;
        }

        public static string getSectionHeader(this CaseInputViewModel model, CaseSectionType type, string defaultHeader = "")
        {
            var section = model.CaseSectionModels.SingleOrDefault(x => x.SectionType == (int)type);
            string result;
            if (section != null)
            {
                result = !string.IsNullOrEmpty(section.SectionHeader) ? section.SectionHeader : Translation.GetCoreTextTranslation(defaultHeader);
            }
            else
            {
                result = Translation.GetCoreTextTranslation(defaultHeader);
            }
            return result;
        }

        public static string getSectionHeaderFields(this CaseInputViewModel model, CaseSectionType type)
        {
            var result = new List<string>();
            var section = model.CaseSectionModels.SingleOrDefault(x => x.SectionType == (int)type);
            if (section != null)
            {
                var fields = model.caseFieldSettings.Where(x => section.CaseSectionFields.Contains(x.Id));
                result = GetCaseFieldsValues(model, fields);
            }
            result = result.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            return result.Any() ? " - " + string.Join(" - ", result) : string.Empty;
        }

        public static ProductArea GetParent(this ProductArea pa)
        {
            if (pa == null)
                return null;

            if (pa.ParentProductArea == null)
                return pa;
            else
                return GetParent(pa.ParentProductArea);
        }

        
        #region Private

        private static List<string> GetCaseFieldsValues(CaseInputViewModel model, IEnumerable<CaseFieldSetting> fields)
        {
            var result = new List<string>();
            foreach (var caseFieldSetting in fields)
            {
                #region Initiator
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.ReportedBy.ToString())
                {
                    result.Add(model.case_.ReportedBy);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Persons_Name.ToString())
                {
                    result.Add(model.case_.PersonsName);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Persons_EMail.ToString())
                {
                    result.Add(model.case_.PersonsEmail);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Persons_Phone.ToString())
                {
                    result.Add(model.case_.PersonsPhone);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Persons_CellPhone.ToString())
                {
                    result.Add(model.case_.PersonsCellphone);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Region_Id.ToString())
                {
                    var curRegion = model.regions.SingleOrDefault(r => r.Id == model.case_.Region_Id);
                    if (curRegion != null)
                        result.Add(curRegion.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Department_Id.ToString())
                {
                    var curDep = model.departments.SingleOrDefault(d => d.Id == model.case_.Department_Id);
                    if (curDep != null)
                        result.Add(curDep.DepartmentName);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.OU_Id.ToString())
                {
                    var curOu = model.ous.SingleOrDefault(d => d.Id == model.case_.OU_Id);
                    if (curOu != null)
                        result.Add(curOu.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.CostCentre.ToString())
                {
                    result.Add(model.case_.CostCentre);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Place.ToString())
                {
                    result.Add(model.case_.Place);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.UserCode.ToString())
                {
                    result.Add(model.case_.UserCode);
                }
                #endregion
                #region Regarding

                if (model.case_.IsAbout != null)
                {
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_ReportedBy.ToString())
                    {
                        result.Add(model.case_.IsAbout.ReportedBy);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Name.ToString())
                    {
                        result.Add(model.case_.IsAbout.Person_Name);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_Persons_EMail.ToString())
                    {
                        result.Add(model.case_.IsAbout.Person_Email);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_Persons_Phone.ToString())
                    {
                        result.Add(model.case_.IsAbout.Person_Phone);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_Persons_CellPhone.ToString())
                    {
                        result.Add(model.case_.IsAbout.Person_Cellphone);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_Region_Id.ToString())
                    {
                        var curRegion = model.regions.SingleOrDefault(r => r.Id == model.case_.IsAbout.Region_Id);
                        if (curRegion != null)
                            result.Add(curRegion.Name);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_Department_Id.ToString())
                    {
                        var curDep = model.departments.SingleOrDefault(d => d.Id == model.case_.IsAbout.Department_Id);
                        if (curDep != null)
                            result.Add(curDep.DepartmentName);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_OU_Id.ToString())
                    {
                        var curOu = model.ous.SingleOrDefault(d => d.Id == model.case_.IsAbout.OU_Id);
                        if (curOu != null)
                            result.Add(curOu.Name);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_CostCentre.ToString())
                    {
                        result.Add(model.case_.IsAbout.CostCentre);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_Place.ToString())
                    {
                        result.Add(model.case_.IsAbout.Place);
                    }
                    if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.IsAbout_UserCode.ToString())
                    {
                        result.Add(model.case_.IsAbout.UserCode);
                    }
                }

                #endregion
                #region Computer info
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.InventoryNumber.ToString())
                {
                    result.Add(model.case_.InventoryNumber);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString())
                {
                    result.Add(model.case_.InventoryType);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.InventoryLocation.ToString())
                {
                    result.Add(model.case_.InventoryLocation);
                }
                #endregion
                #region Case info
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.CaseNumber.ToString())
                {
                    result.Add(model.case_.CaseNumber.ToString());
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.RegTime.ToString())
                {
                    result.Add(model.case_.RegTime.ToString());
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.ChangeTime.ToString())
                {
                    result.Add(model.case_.ChangeTime.ToString());
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.User_Id.ToString())
                {
                    var str = string.Empty;
                    if (model.RegByUser != null)
                    {
                        str = model.RegByUser.FirstName + " " + model.RegByUser.SurName;
                        if (model.CaseOwnerDefaultWorkingGroup != null)
                        {
                            str= str + " " + model.CaseOwnerDefaultWorkingGroup.WorkingGroupName;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model.case_.RegUserName))
                        {
                            str = str + " " + model.case_.RegUserName;
                        }

                        if (!string.IsNullOrEmpty(model.case_.RegUserId))
                        {
                            str = str + " " + model.case_.RegUserId;
                        }
                    }
                    str = str + " " + model.case_.IpAddress;
                    result.Add(str);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.RegistrationSourceCustomer.ToString())
                {
                    result.Add(model.SelectedCustomerRegistrationSource);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.CaseType_Id.ToString())
                {
                    var cur = model.caseTypes.SingleOrDefault(d => d.Id == model.case_.CaseType_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                    else
                    {
                        var curCt = GetCaseType(model.caseTypes, model.case_.CaseType_Id);
                        if (curCt != null)
                        {
                            result.Add(curCt.Name);
                        }
                    }
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.ProductArea_Id.ToString())
                {
                    var cur = model.productAreas.SingleOrDefault(d => d.Id == model.case_.ProductArea_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                    else
                    {
                        if (model.case_.ProductArea_Id.HasValue)
                        {
                            var curPa = GetProductAreaOverview(model.productAreas, model.case_.ProductArea_Id.Value);
                            if (curPa != null)
                            {
                                result.Add(curPa.Name);
                            }
                        }
                    }
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.System_Id.ToString())
                {
                    var cur = model.systems.SingleOrDefault(d => d.Id == model.case_.System_Id);
                    if (cur != null)
                        result.Add(cur.SystemName);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Urgency_Id.ToString())
                {
                    var cur = model.urgencies.SingleOrDefault(d => d.Id == model.case_.Urgency_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Impact_Id.ToString())
                {
                    var cur = model.impacts.SingleOrDefault(d => d.Id == model.case_.Impact_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Category_Id.ToString())
                {
                    var cur = model.categories.SingleOrDefault(d => d.Id == model.case_.Category_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                    else
                    {
                        if (model.case_.Category_Id.HasValue)
                        {
                            var curPa = GetCategory(model.categories, model.case_.Category_Id.Value);
                            if (curPa != null)
                            {
                                result.Add(curPa.Name);
                            }
                        }
                    }
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Supplier_Id.ToString())
                {
                    var cur = model.suppliers.SingleOrDefault(d => d.Id == model.case_.Supplier_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.InvoiceNumber.ToString())
                {
                    result.Add(model.case_.InvoiceNumber);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.ReferenceNumber.ToString())
                {
                    result.Add(model.case_.ReferenceNumber);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Caption.ToString())
                {
                    result.Add(model.case_.Caption);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Description.ToString())
                {
                    if (!string.IsNullOrEmpty(model.case_.Description))
                    {
                        if (model.case_.Description.Length > 30)
                            result.Add(model.case_.Description.Substring(0, 30));
                        else
                        {
                            result.Add(model.case_.Description);
                        }
                    }
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Miscellaneous.ToString())
                {
                    if (!string.IsNullOrEmpty(model.case_.Miscellaneous))
                    {
                        if (model.case_.Miscellaneous.Length > 30)
                            result.Add(model.case_.Miscellaneous.Substring(0, 30));
                        else
                        {
                            result.Add(model.case_.Miscellaneous);
                        }
                    }
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.AgreedDate.ToString())
                {
                    if (model.case_.AgreedDate.HasValue)
                    result.Add(model.case_.AgreedDate.Value.ToShortDateString());
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Available.ToString())
                {
                    result.Add(model.case_.Available);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Cost.ToString())
                {
                    var str = model.case_.Cost + " " + model.case_.OtherCost + " " + model.case_.Currency;
                    result.Add(str);
                }
                #endregion
                #region Case management

                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.WorkingGroup_Id.ToString())
                {
                    var cur = model.workingGroups.SingleOrDefault(r => r.Id == model.case_.WorkingGroup_Id);
                    if (cur != null)
                        result.Add(cur.WorkingGroupName);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.CaseResponsibleUser_Id.ToString())
                {
                    var cur = model.ResponsibleUsersAvailable.SingleOrDefault(r => r.Value.Equals(model.ResponsibleUser_Id.ToString()));
                    if (cur != null)
                        result.Add(cur.Text);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Performer_User_Id.ToString())
                {
                    var cur = model.Performers.SingleOrDefault(r => r.Value.Equals(model.Performer_Id.ToString()));
                    if (cur != null)
                        result.Add(cur.Text);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Priority_Id.ToString())
                {
                    var cur = model.priorities.SingleOrDefault(r => r.Id == model.case_.Priority_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Status_Id.ToString())
                {
                    var cur = model.statuses.SingleOrDefault(r => r.Id == model.case_.Status_Id);
                    if (cur != null)
                        result.Add(Translation.GetCoreTextTranslation(cur.Name));
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.StateSecondary_Id.ToString())
                {
                    var cur = model.stateSecondaries.SingleOrDefault(r => r.Id == model.case_.StateSecondary_Id);
                    if (cur != null)
                        result.Add(Translation.GetCoreTextTranslation(cur.Name));
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Project.ToString())
                {
                    var cur = model.projects.SingleOrDefault(d => d.Id == model.case_.Project_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Problem.ToString())
                {
                    var cur = model.problems.SingleOrDefault(d => d.Id == model.case_.Problem_Id);
                    if (cur != null)
                        result.Add(cur.Name);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.CausingPart.ToString())
                {
                    var cur = model.causingParts.SingleOrDefault(d => d.Value.Equals(model.case_.CausingPartId.ToString()));
                    if (cur != null)
                        result.Add(cur.Value);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.Change.ToString())
                {
                    var cur = model.changes.SingleOrDefault(d => d.Id == model.case_.Change_Id);
                    if (cur != null)
                        result.Add(cur.ChangeTitle);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.PlanDate.ToString())
                {
                    if (model.case_.PlanDate.HasValue)
                        result.Add(model.case_.PlanDate.Value.ToShortDateString());
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.WatchDate.ToString())
                {
                    if (model.case_.WatchDate.HasValue)
                        result.Add(model.case_.WatchDate.Value.ToShortDateString());
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.VerifiedDescription.ToString())
                {
                    result.Add(model.case_.VerifiedDescription);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.SolutionRate.ToString())
                {
                    result.Add(model.case_.SolutionRate);
                }

                #endregion
                #region Status
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.ClosingReason.ToString())
                {
                    result.Add(model.FinishingCause);
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.FinishingDate.ToString())
                {
                    if (model.case_.FinishingDate.HasValue)
                        result.Add(model.case_.FinishingDate.Value.ToShortDateString());
                }
                if (caseFieldSetting.Name == GlobalEnums.TranslationCaseFields.FinishingDescription.ToString())
                {
                    if (!string.IsNullOrEmpty(model.case_.FinishingDescription))
                    {
                        if (model.case_.FinishingDescription.Length > 30)
                            result.Add(model.case_.FinishingDescription.Substring(0, 30));
                        else
                        {
                            result.Add(model.case_.FinishingDescription);
                        }
                    }
                }
                #endregion
            }
            return result;
        }

        private static ProductAreaOverview GetProductAreaOverview(IList<ProductAreaOverview> productAreas, int productAreaId)
        {
            ProductAreaOverview pa = null;
            foreach (var productArea in productAreas)
            {
                if (productArea.SubProductAreas != null && productArea.SubProductAreas.Any())
                {
                    var childs = productArea.SubProductAreas;
                    pa = childs.SingleOrDefault(x => x.Id == productAreaId);
                    if (pa != null)
                        return pa;
                    if (childs.Count > 0)
                    {
                        var sortedChildren = childs.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList();
                        pa = GetProductAreaOverview(sortedChildren, productAreaId);
                    }
                }
            }

            return pa;
        }

        [Obsolete("Use GetProductAreaOverview")]
        private static ProductArea GetProductArea(IList<ProductArea> productAreas, int productAreaId)
        {
            ProductArea pa = null;
            foreach (var productArea in productAreas)
            {
                if (productArea.SubProductAreas != null && productArea.SubProductAreas.Any())
                {
                    var childs = productArea.SubProductAreas;
                    pa = childs.SingleOrDefault(x => x.Id == productAreaId);
                    if (pa != null)
                        return pa;

                    if (childs.Count > 0)
                    {
                        pa = GetProductArea(childs.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), productAreaId);
                    }
                }
            }
            return pa;
        }

        private static CaseTypeOverview GetCaseType(IList<CaseTypeOverview> list, int id)
        {
            CaseTypeOverview item = null;
            foreach (var it in list)
            {
                if (it.SubCaseTypes != null && it.SubCaseTypes.Any())
                {
                    var childs = it.SubCaseTypes;
                    item = childs.SingleOrDefault(x => x.Id == id);
                    if (item != null)
                        return item;

                    if (childs.Count > 0)
                    {
                        item = GetCaseType(childs.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), id);
                    }
                }
            }
            return item;
        }

        private static CategoryOverview GetCategory(IList<CategoryOverview> list, int id)
        {
            CategoryOverview item = null;
            foreach (var it in list)
            {
                if (it.SubCategories != null && it.SubCategories.Any())
                {
                    var childs = it.SubCategories;
                    item = childs.SingleOrDefault(x => x.Id == id);
                    if (item != null)
                        return item;
                    if (childs.Count > 0)
                    {
                        item = GetCategory(childs.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(), id);
                    }
                }
            }
            return item;
        }

        #endregion

    }
}
