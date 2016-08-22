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
            return new WorkingGroupEntity { Id = 0, WorkingGroupName = "-- " + Translation.Get("Ej Tilldelade", Enums.TranslationSource.TextTranslation) + " --", IsActive = 1 };
        }

        public static User notAssignedPerformer()
        {
            return new User { Id = int.MinValue, FirstName = "-- " + Translation.Get("Ej Tilldelade", Enums.TranslationSource.TextTranslation) + " --", SurName="", IsActive = 1 , Performer = 1};
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
            ret.Add(new Field { Id = -1, StringValue = string.Empty });
            ret.Add(new Field { Id = 2, StringValue = Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 1, StringValue = Translation.Get("Avslutade ärenden", Enums.TranslationSource.TextTranslation) });                        
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

        /// <summary>
        /// The is field visible.
        /// </summary>
        /// <param name="fields">
        /// The fields.
        /// </param>
        /// <param name="field">
        /// The field.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsFieldVisible(this IList<CaseFieldSetting> fields, GlobalEnums.TranslationCaseFields field)
        {
            return fields.getCaseSettingsValue(field.ToString()).ShowOnStartPage != 0;
        }

        public static bool IsFieldRequiredOrVisible(this IList<CaseFieldSetting> fields, GlobalEnums.TranslationCaseFields field)
        {
            var fieldSettings = fields.getCaseSettingsValue(field.ToString());
            return fieldSettings.ShowOnStartPage != 0 || fieldSettings.Required != 0;
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

        public static CaseFieldSetting getCaseSettingsValue(this IList<CaseFieldSetting> cfs, string valueToFind)
        {
            var ret = new CaseFieldSetting();  

            if (cfs != null)
            {
                foreach (CaseFieldSetting c in cfs)
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


        public static int CaseFieldSettingRequiredCheck(this IList<CaseFieldSetting> cfs, string valueToFind)
        {
            int ret = 0;
            if (cfs != null)
                foreach (CaseFieldSetting c in cfs)
                {
                    if (string.Compare(c.Name, valueToFind.getCaseFieldName(), true) == 0)
                    {
                        if (c.Required == 1 && c.ShowOnStartPage == 1)
                            ret = 1;   
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
            if (cfs.getCaseSettingsValue(valueToFind).ShowOnStartPage == 0)
                return "display:none";
            return string.Empty;
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

        public static int getFieldSize(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).FieldSize;
        }
        public static string getEMailIdentifier(this IEnumerable<CaseFieldSetting> cfs, string valueToFind)
        {
            return cfs.ToList().getCaseSettingsValue(valueToFind).EMailIdentifier;
        }

        public static string getLabel(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            return cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).Label;
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

    }
}
