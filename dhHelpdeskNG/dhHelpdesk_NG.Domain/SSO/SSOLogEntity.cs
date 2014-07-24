using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.Domain.SSO
{
    using global::System;

    public class SSOLogEntity : Entity
    {
        #region Public Properties
        	    
        public string ApplicationId  {get;set;}

        public string NetworkId  {get;set;}

        public string ClaimData { get; set; }

        public DateTime CreatedDate  {get;set;}
        
        #endregion
    }
}