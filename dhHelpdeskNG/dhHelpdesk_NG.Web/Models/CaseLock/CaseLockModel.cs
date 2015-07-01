using System;

namespace DH.Helpdesk.Web.Models.CaseLock
{
    using DH.Helpdesk.Domain;

    public sealed class CaseLockModel
    {
        public CaseLockModel()
        {
            this.IsLocked = false;
        }

        public CaseLockModel(
            bool isLocked,
            int caseId,
            int userId,
            string lockGUID,
            string browserSession,
            DateTime createTime,
            DateTime extendedTime,            
            int extendValue,
            User user)
        {
            this.IsLocked = isLocked;
            this.CaseId = caseId;
            this.UserId = userId;
            this.LockGUID = lockGUID;
            this.BrowserSession = browserSession;            
            this.CreatedTime = createTime;
            this.ExtendedTime = extendedTime;
            this.ExtendValue = extendValue;
            this.User = user;                            
        }

        public bool IsLocked { get; private set; }
        
        public int CaseId { get; private set; }

        public int UserId { get; private set; }

        public string LockGUID { get; set; }

        public string BrowserSession { get; private set; }        

        public DateTime CreatedTime { get; private set; }

        public DateTime ExtendedTime { get; private set; }

        public int ExtendValue { get; private set; }

        public User User { get; private set; }
    }

}