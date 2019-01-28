using System;
using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Common.Extensions.Integer;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.WebApi.Logic.CaseFieldSettings
{
    public interface ICaseFieldSettingsHelper
    {
        bool IsActive(IList<CaseFieldSetting> caseFieldSettings, GlobalEnums.TranslationCaseFields field);
        CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName);
        bool IsCaseNew(int currentCaseId);
        bool IsReadOnly(GlobalEnums.TranslationCaseFields fieldName, int currentCaseId, int permission = 1);
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

        public bool IsCaseNew(int currentCaseId)
        {
            return currentCaseId < 0;
        }

        public bool IsReadOnly(GlobalEnums.TranslationCaseFields fieldName, int currentCaseId, int permission = 1)
        {
            return !IsCaseNew(currentCaseId) && !permission.ToBool();
        }

    }
}