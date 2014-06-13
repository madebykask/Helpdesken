namespace DH.Helpdesk.Dal.Repositories.ActionSetting
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ActionSetting;    
    using DH.Helpdesk.Dal.Dal;

    public interface IActionSettingRepository : INewRepository
    {
        #region Public Methods and Operators
 
        List<ActionSetting> GetActionSettings(int customerId);        

        #endregion
    }
}