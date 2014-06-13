namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;
    using DH.Helpdesk.Dal.Repositories.ActionSetting;

    
    public sealed class ActionSettingService : IActionSettingService
    {
        
        private readonly IActionSettingRepository _actionSettingRepository;

        public ActionSettingService(IActionSettingRepository actionSettingRepository)
        {
            this._actionSettingRepository = actionSettingRepository;
        }

        
        public List<ActionSetting> GetActionSettings(int customerId)
        {
            return this._actionSettingRepository.GetActionSettings(customerId);
        }
              
    }
}