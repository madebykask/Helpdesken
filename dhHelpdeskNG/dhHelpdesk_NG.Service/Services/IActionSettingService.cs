namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;

    public interface IActionSettingService
    {
        
        List<ActionSetting> GetActionSettings(int customerId);

    }
}