namespace DH.Helpdesk.NewSelfService.Infrastructure.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.NewSelfService.Models.Case;

    public interface ICommonFunctions
    {
        List<CaseSolution> GetCaseTemplates(int customerId);

        List<ActionSetting> GetActionSettings(int customerId);

        CaseLogModel GetCaseLogs(int caseId);
    }
}