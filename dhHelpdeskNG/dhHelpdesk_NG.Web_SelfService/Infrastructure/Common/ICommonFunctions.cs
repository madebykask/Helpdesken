namespace DH.Helpdesk.SelfService.Infrastructure.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.SelfService.Models.Case;

    public interface ICommonFunctions
    {
        List<CaseSolution> GetCaseTemplates(int customerId);

        List<ActionSetting> GetActionSettings(int customerId);

        CaseLogModel GetCaseLogs(int caseId);
        bool IsOrderModuleEnabled(int customerId);
        bool IsUserHasOrderTypes(int userId, int customerId);
    }
}