namespace DH.Helpdesk.Dal.Repositories.SSO.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.SSO.Input;    
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.SSO;
    using DH.Helpdesk.Domain.SSO;

    public sealed class SSORepository : Repository, ISSORepository
    {
        #region Constructors and Destructors

        public SSORepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        public void SaveSSOLog(NewSSOLog SSOLog)
        {
            var ssoLogEntity = new SSOLogEntity()
            {
                ApplicationId = SSOLog.ApplicationId,
                NetworkId = SSOLog.NetworkId,
                ClaimData = SSOLog.ClaimData,
                CreatedDate = SSOLog.CreatedDate
            };

            this.DbContext.SSOLogs.Add(ssoLogEntity);
            //this.InitializeAfterCommit(newCircular, circularEntity);            
       
        }

       

    }
}