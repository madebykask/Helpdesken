namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.SSO.Input;    
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.SSO;
    using System.Globalization;

    public class SSOService : ISSOService
    {
        #region Fields

        private readonly ISSORepository _SSORepository;
      
        #endregion

        #region Constructors and Destructors

        public SSOService(ISSORepository ssoRepository)
        {
            this._SSORepository = ssoRepository;            
        }

        #endregion

        #region Public Methods and Operators

        public void SaveSSOLog(NewSSOLog SSOLog)
        {
            this._SSORepository.SaveSSOLog(SSOLog);
            this._SSORepository.Commit();
        }
        
        #endregion
    }
}