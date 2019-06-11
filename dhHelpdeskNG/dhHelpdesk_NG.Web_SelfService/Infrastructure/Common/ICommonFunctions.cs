using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.ActionSetting;
using DH.Helpdesk.SelfService.Models.Shared;

namespace DH.Helpdesk.SelfService.Infrastructure.Common
{
    public interface ICommonFunctions
    {
        LayoutViewModel GetLayoutViewModel(object tmpDataLanguge);
        List<ActionSetting> GetActionSettings(int customerId);
        bool IsOrderModuleEnabled(int customerId);
        bool IsUserHasOrderTypes(int userId, int customerId);
    }
}