namespace DH.Helpdesk.SelfService.Infrastructure.Common
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;
    using DH.Helpdesk.Domain;
    using Models.Shared;

    public interface ICommonFunctions
    {
        LayoutViewModel GetLayoutViewModel(object tmpDataLanguge);

        List<CaseSolution> GetCaseTemplates(int customerId);
        List<ActionSetting> GetActionSettings(int customerId);
        bool IsOrderModuleEnabled(int customerId);
        bool IsUserHasOrderTypes(int userId, int customerId);
    }
}