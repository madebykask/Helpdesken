using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Enums.Settings;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.WebApi.Logic.CaseFieldSettings
{
    public interface ICaseFieldSettingsHelper
    {
        bool IsActive(IList<CaseFieldSetting> caseFieldSettings, ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, 
            GlobalEnums.TranslationCaseFields field);

        CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName);

        bool IsCaseNew(int? currentCaseId);

        bool IsReadOnly(GlobalEnums.TranslationCaseFields fieldName, int? currentCaseId,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, int permission = 1);

        bool IsReadonlyTemplate(GlobalEnums.TranslationCaseFields field, ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings);
    }

    public class CaseFieldSettingsHelper: ICaseFieldSettingsHelper
    {
        private static readonly List<GlobalEnums.TranslationCaseFields> VisibleFields = new List<GlobalEnums.TranslationCaseFields>()
        {
            GlobalEnums.TranslationCaseFields.CaseType_Id,
            GlobalEnums.TranslationCaseFields.Performer_User_Id,
            GlobalEnums.TranslationCaseFields.Priority_Id,
            GlobalEnums.TranslationCaseFields.tblLog_Text_Internal
        };

        public bool IsActive(IList<CaseFieldSetting> caseFieldSettings, ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, GlobalEnums.TranslationCaseFields field)
        {
            var caseSolutionsSettings = CaseSolutionFieldSetting(caseTemplateSettings, field);
            if (caseSolutionsSettings != null && caseSolutionsSettings.CaseSolutionMode == CaseSolutionModes.Hide)
                return false;

            var caseSettings = GetCaseFieldSetting(caseFieldSettings, field.ToString());
            if (caseSettings == null)
                return false;

            return (caseSettings.IsActive && (!caseSettings.Hide || IsFieldAlwaysVisible(field))) || caseSettings.Required != 0;
        }

        public bool IsReadonlyTemplate(GlobalEnums.TranslationCaseFields field, ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings)
        {
            var caseSolutionsSettings = CaseSolutionFieldSetting(caseTemplateSettings, field);
            return caseSolutionsSettings != null && caseSolutionsSettings.CaseSolutionMode == CaseSolutionModes.ReadOnly;
        }

        public bool IsFieldAlwaysVisible(GlobalEnums.TranslationCaseFields field)
        {
            return VisibleFields.Contains(field);
        }

        public CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName)
        {
            return caseFieldSettings.FirstOrDefault(s => s.Name.Equals(fieldName.Replace("tblLog_", "tblLog."), StringComparison.OrdinalIgnoreCase));
        }

        public bool IsCaseNew(int? currentCaseId)
        {
            return !currentCaseId.HasValue || currentCaseId.Value <= 0;
        }

        public bool IsReadOnly(GlobalEnums.TranslationCaseFields fieldName, int? currentCaseId,
            ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, int permission = 1)
        {
            return (!IsCaseNew(currentCaseId) && !permission.ToBool()) ||
                   (caseTemplateSettings != null && IsCaseNew(currentCaseId) && IsReadonlyTemplate(fieldName, caseTemplateSettings));
        }

        public CaseSolutionSettingOverview CaseSolutionFieldSetting(ReadOnlyCollection<CaseSolutionSettingOverview> caseTemplateSettings, GlobalEnums.TranslationCaseFields field)
        {
            return caseTemplateSettings?.FirstOrDefault(c => c.CaseSolutionField.MapToCaseField() == field);
        }

    }
}