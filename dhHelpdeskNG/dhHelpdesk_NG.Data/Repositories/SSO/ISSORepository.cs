namespace DH.Helpdesk.Dal.Repositories.SSO
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.SSO.Input;    
    using DH.Helpdesk.Dal.Dal;

    public interface ISSORepository : INewRepository
    {
        #region Public Methods and Operators
 
        void SaveSSOLog (NewSSOLog SSOLog);

        #endregion
    }
}