using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.WebApi.Logic.CaseFieldSettings
{
    public interface ICaseFieldSettingsHelper
    {
        bool IsActive(IList<CaseFieldSetting> caseFieldSettings, GlobalEnums.TranslationCaseFields field);
        CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName);
    }

    public class CaseFieldSettingsHelper: ICaseFieldSettingsHelper
    {
        public bool IsActive(IList<CaseFieldSetting> caseFieldSettings, GlobalEnums.TranslationCaseFields field)
        {
            var caseSettings = GetCaseFieldSetting(caseFieldSettings, field.ToString());
            if (caseSettings == null) return false;

            return (caseSettings.IsActive && !caseSettings.Hide) || caseSettings.Required != 0;
        }

        public CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName)
        {
            return caseFieldSettings.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}