using System;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseLock
{
    #region Interfaces

    public interface ICaseLockOverview
    {
        int Id { get; }
        int CaseId { get; }
        int UserId { get; }
        Guid LockGUID { get; }
        string BrowserSession { get; }
        DateTime CreatedTime { get; }
        DateTime ExtendedTime { get; }

        ICaseLockUserInfo User { get; }
    }

    public interface ICaseLockUserInfo
    {
        int Id { get; }
        string UserId { get; }
        string FirstName { get;  }
        string LastName { get;  }
    }

    #endregion

    public class CaseLockOverview : ICaseLockOverview
    {
        private string _browserSession;

        public int Id { get; set; }

        public int CaseId { get; set; }

        public int UserId { get; set; }

        public Guid LockGUID { get; set; }

        public string BrowserSession
        {
            get { return _browserSession ?? string.Empty; }
            set { _browserSession = value; }
        }

        public DateTime CreatedTime { get; set; }

        public DateTime ExtendedTime { get; set; }

        public ICaseLockUserInfo User { get; set; }
    }

    public class CaseLockUserInfo : ICaseLockUserInfo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}