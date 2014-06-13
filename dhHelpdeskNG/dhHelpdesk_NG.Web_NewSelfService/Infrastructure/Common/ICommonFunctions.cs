namespace DH.Helpdesk.NewSelfService.Infrastructure.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;
    using DH.Helpdesk.Domain;

    public interface ICommonFunctions
    {
        List<CaseSolution> GetCaseTemplates(int customerId);

        List<ActionSetting> GetActionSettings(int customerId);
    }
}