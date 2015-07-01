namespace DH.Helpdesk.BusinessData.Models.Case.CaseLock
{
    using System;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.OldComponents;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.BusinessData.Models.Shared.Input;
    using System.Collections.Generic;

    public sealed class LockedCaseOverview 
    {
        public LockedCaseOverview()
        {            
        }

        public LockedCaseOverview(User user, IList<LockInfo> lockInfo)
        {
            this.User = user;
            this.LockInfo = lockInfo;                        
        }
        
        public User User { get; private set; }

        public IList<LockInfo> LockInfo { get; private set; }        
    }

    public sealed class LockInfo
    {
        public LockInfo()
        {
        }

        public LockInfo(int caseId, 
                        decimal caseNumber, 
                        int customerId,
                        string customerName,
                        DateTime createdTime)
        {
            this.CaseId = caseId;
            this.CaseNumber = caseNumber;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.CreatedTime = createdTime;                        
        }
        
        public int CaseId { get; private set; }

        public decimal CaseNumber { get; private set; }

        public int CustomerId { get; private set; }

        public string CustomerName { get; private set; }        

        public DateTime CreatedTime { get; private set; }                
    }

}