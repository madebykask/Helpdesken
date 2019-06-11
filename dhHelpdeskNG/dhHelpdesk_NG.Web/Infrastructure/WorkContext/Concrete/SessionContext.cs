using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Types;

namespace DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete
{
    public interface ISessionContext
    {
        string SessionId { get; }

        IUserIdentity UserIdentity { get; }
        void SetUserIdentity(UserIdentity userIdentity);

        void SetCurrentLanguageId(int languageId);
        void SetCurrentLanguageCode(string languageCode);

        LoginMode LoginMode { get; }
        void SetLoginMode(LoginMode mode);

        void ClearSession(bool abandon = false);
    }
    
    public class SessionContext : ISessionContext
    {
        public string SessionId
        {
            get { return SessionFacade.SessionId; }
        }

        public IUserIdentity UserIdentity
        {
            get { return SessionFacade.CurrentUserIdentity; }
        }

        public void SetUserIdentity(UserIdentity userIdentity)
        {
            SessionFacade.CurrentUserIdentity = userIdentity;
        }

        public LoginMode LoginMode
        {
            get { return SessionFacade.CurrentLoginMode; }
        }

        public void SetCurrentLanguageId(int languageId)
        {
            SessionFacade.CurrentLanguageId = languageId;
        }

        public void SetCurrentLanguageCode(string languageCode)
        {
            SessionFacade.CurrentLanguageCode = languageCode;
        }

        public void SetLoginMode(LoginMode mode)
        {
            SessionFacade.CurrentLoginMode = mode;
        }

        public void ClearSession(bool abandon = false)
        {
            SessionFacade.ClearSession(abandon);
        }
    }
}