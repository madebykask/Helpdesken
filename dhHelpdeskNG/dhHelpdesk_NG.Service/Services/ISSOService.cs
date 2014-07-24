namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.SSO.Input;

    public interface ISSOService
    {
        
        void SaveSSOLog (NewSSOLog SSOLog);

    }
}