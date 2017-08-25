using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Infrastructure.Helpers
{
    internal static class TranslateHelper
    {
        internal static CaseType TranslateCaseType(CaseType caseType)
        {
            if (caseType.ParentCaseType != null)
                caseType.ParentCaseType = TranslateCaseType(caseType.ParentCaseType);

            caseType.Name = Translation.GetCoreTextTranslation(caseType.Name);

            return caseType;
        }
    }
}