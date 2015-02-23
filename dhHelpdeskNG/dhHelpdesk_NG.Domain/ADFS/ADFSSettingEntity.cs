using System.ComponentModel.DataAnnotations;

namespace DH.Helpdesk.Domain.ADFS
{
    using global::System;

    public class ADFSSettingEntity: Entity 
    {
        #region Public Properties

        public string ApplicationId { get; set; }        

	    public string AttrDomain { get; set; }        

	    public string AttrUserId { get; set; }        

	    public string AttrEmployeeNumber { get; set; }        

	    public string AttrFirstName { get; set; }        

	    public string AttrSurName { get; set; }        

	    public string AttrEmail { get; set; }

        public bool SaveSSOLog { get; set; }        

        #endregion
    }
}