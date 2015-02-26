using System;
namespace DH.Helpdesk.BusinessData.Models.ADFS.Input
{
    public sealed class ADFSSetting
    {
        #region Constructors and Destructors

        public ADFSSetting(
                            string applicationId, 
                            string attrDomain, 
                            string attrUserId,
                            string attrEmployeeNumber,
                            string attrFirstName,
                            string attrSurName,
                            string attrEmail,
                            bool saveSSOLog
                           )
        {
            this.ApplicationId = applicationId;
            this.AttrDomain = attrDomain; 
            this.AttrUserId = attrUserId;
            this.AttrEmployeeNumber = attrEmployeeNumber;
            this.AttrFirstName = attrFirstName;
            this.AttrSurName = attrSurName;
            this.AttrEmail = attrEmail;
            this.SaveSSOLog = saveSSOLog;
        }

        #endregion

        #region Properties
               
        public string ApplicationId { get; private set; }

        public string AttrDomain { get; private set; }

        public string AttrUserId { get; private set; }

        public string AttrEmployeeNumber { get; private set; }

        public string AttrFirstName { get; private set; }

        public string AttrSurName { get; private set; }

        public string AttrEmail { get; private set; }

        public bool SaveSSOLog { get; private set; }

        #endregion
    }
}