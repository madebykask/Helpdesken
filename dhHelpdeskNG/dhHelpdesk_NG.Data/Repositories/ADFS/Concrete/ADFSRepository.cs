namespace DH.Helpdesk.Dal.Repositories.ADFS.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ADFS.Input;    
    using DH.Helpdesk.Common.Enums;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.ADFS;
    using DH.Helpdesk.Domain.ADFS;

    public sealed class ADFSRepository : Repository, IADFSRepository
    {
        #region Constructors and Destructors

        public ADFSRepository(IDatabaseFactory databaseFactory)
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

        public ADFSSetting GetADFSSetting()
        {
            ADFSSetting ret = null;             
            
            var adfsSetting =
                this.DbContext.ADFSSetting.Select(
                        a =>
                        new
                        {
                            a.ApplicationId,
                            a.AttrDomain,
                            a.AttrEmail,
                            a.AttrEmployeeNumber,
                            a.AttrFirstName,
                            a.AttrSurName,
                            a.AttrUserId,
                            a.SaveSSOLog
                        }).SingleOrDefault();

            if (adfsSetting != null)
                ret = new ADFSSetting(adfsSetting.ApplicationId,
                                      adfsSetting.AttrDomain,
                                      adfsSetting.AttrUserId,
                                      adfsSetting.AttrEmployeeNumber,
                                      adfsSetting.AttrFirstName,
                                      adfsSetting.AttrSurName,
                                      adfsSetting.AttrEmail,
                                      adfsSetting.SaveSSOLog);

            return ret;
       
        }

        public void SaveADFSSetting(ADFSSetting adfsSetting)
        {
            var adfsSettingEntity = new ADFSSettingEntity()
            {
                ApplicationId = adfsSetting.ApplicationId,
                AttrDomain = adfsSetting.AttrDomain,
                AttrEmail = adfsSetting.AttrEmail,
                AttrEmployeeNumber = adfsSetting.AttrEmployeeNumber,
                AttrFirstName = adfsSetting.AttrFirstName,
                AttrSurName = adfsSetting.AttrSurName,
                AttrUserId = adfsSetting.AttrUserId,
                SaveSSOLog= adfsSetting.SaveSSOLog                
            };

            this.DbContext.ADFSSetting.Add(adfsSettingEntity);            
        }

    }
}