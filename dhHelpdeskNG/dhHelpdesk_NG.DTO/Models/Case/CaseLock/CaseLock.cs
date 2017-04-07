namespace DH.Helpdesk.BusinessData.Models.Case.CaseLock
{
    using System;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public sealed class CaseLock : INewBusinessModel
    {
        public CaseLock()
        {            
        }

        public CaseLock(
            int caseId,
            int userId,
            Guid lockGUID,
            string browserSession,
            DateTime createTime,
            DateTime extendedTime)
        {
            this.CaseId = caseId;
            this.UserId = userId;
            this.LockGUID = lockGUID;
            this.BrowserSession = browserSession;
            this.CreatedTime = createTime;
            this.ExtendedTime = extendedTime;
        }

        public CaseLock(
            int caseId, 
            int userId, 
            Guid lockGUID,
            string browserSession,
            DateTime createTime,
            DateTime extendedTime,
            User user)
            : this(caseId, userId, lockGUID, browserSession, createTime, extendedTime)
        {
            this.User = user;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int CaseId { get; private set; }
        
        public int UserId { get; private set; }

        public Guid LockGUID { get; private set; }

        public string BrowserSession { get; private set; }

        public DateTime CreatedTime { get; private set; }

        public DateTime ExtendedTime { get; private set; }
        
        public User User { get; private set; }

    }
}