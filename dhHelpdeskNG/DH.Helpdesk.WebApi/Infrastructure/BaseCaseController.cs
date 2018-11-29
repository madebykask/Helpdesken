using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.BusinessData.OldComponents;
using DH.Helpdesk.Domain;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    public abstract class BaseCaseController: BaseApiController // TODO: Move to services
    {
        
        protected static bool IsActive(IList<CaseFieldSetting> caseFieldSettings, GlobalEnums.TranslationCaseFields field)
        {
            var caseSettings = GetCaseFieldSetting(caseFieldSettings, field.ToString());
            if (caseSettings == null) return false;

            return (caseSettings.IsActive && !caseSettings.Hide) || caseSettings.Required != 0;
        }

        protected static CaseFieldSetting GetCaseFieldSetting(IList<CaseFieldSetting> caseFieldSettings, string fieldName)
        {
            return caseFieldSettings.FirstOrDefault(s => s.Name.Replace("tblLog_", "tblLog.").Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}