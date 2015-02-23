namespace DH.Helpdesk.Dal.Repositories.ADFS
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.ADFS.Input;    
    using DH.Helpdesk.Dal.Dal;

    public interface IADFSRepository : INewRepository
    {
        #region Public Methods and Operators

        void SaveSSOLog(NewSSOLog SSOLog);

        ADFSSetting GetADFSSetting();

        void SaveADFSSetting(ADFSSetting adfsSetting);

        #endregion
    }
}