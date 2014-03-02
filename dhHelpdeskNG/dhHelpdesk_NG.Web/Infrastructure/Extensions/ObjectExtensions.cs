namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Domain;

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
            return new User { Id = 0, FirstName = "-- " + Translation.Get("Ej Tilldelade", Enums.TranslationSource.TextTranslation) + " --", SurName="", IsActive = 1 , Performer = 1};
        }

        public static IList<Field> GetFilterForCases(int followUpPermission, IList<Priority> pl, int customerId)
        {
            var ret = new List<Field>();

            ret.Add(new Field { Id = 2, StringValue = Translation.Get("Pågående ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 1, StringValue = Translation.Get("Avslutade ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 4, StringValue = Translation.Get("Olästa ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 3, StringValue = Translation.Get("Vilande ärenden", Enums.TranslationSource.TextTranslation) });
            ret.Add(new Field { Id = 7, StringValue = Translation.Get("Ärenden med", Enums.TranslationSource.TextTranslation) + " " + Translation.Get(GlobalEnums.TranslationCaseFields.WatchDate.ToString(), Enums.TranslationSource.CaseTranslation, customerId) });

            if (followUpPermission == 1)
                ret.Add(new Field { Id = 8, StringValue = Translation.Get("För uppföljning", Enums.TranslationSource.TextTranslation) });

            if (pl.getPriorityMaxtime() > 0)
                ret.Add(new Field { Id = 10, StringValue = Translation.Get("Akuta ärenden", Enums.TranslationSource.TextTranslation) });

            int start = 10;
            int i = 1;

            if (pl != null)
                foreach (Priority p in pl.OrderBy(x => x.SolutionTime))
                {
                    if (p.SolutionTime > 0 && p.IsActive == 1)
                    {
                        ret.Add(new Field { Id = (start + i), StringValue = Translation.Get("Återstående åtgärdstid", Enums.TranslationSource.TextTranslation) + " " + p.SolutionTime.ToString() + " " + Translation.Get("timmar", Enums.TranslationSource.TextTranslation) });
                        i++;
                    }
                }

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

        public static string getLabel(this IEnumerable<CaseFieldSettingsWithLanguage> cfsl, string valueToFind)
        {
            return cfsl.ToList().getCaseFieldSettingsLanguageValue(valueToFind).Label;
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

        public static string displayUserInfoHtml(this IList<CaseFieldSetting> cfs)
        {
            var ret = "display:none";

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

        public static string displayComputerInfoHtml(this IList<CaseFieldSetting> cfs)
        {
            var ret = "display:none";

            if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.ComputerType_Id.ToString()).ShowOnStartPage == 1)
                return string.Empty;
            else if (getCaseSettingsValue(cfs, GlobalEnums.TranslationCaseFields.InventoryNumber.ToString()).ShowOnStartPage == 1)
                return string.Empty;

            return ret;
        }

    }
}
