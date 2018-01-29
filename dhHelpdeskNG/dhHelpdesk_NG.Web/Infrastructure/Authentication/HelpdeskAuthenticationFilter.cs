using System.Web.Mvc;
using System.Web.Mvc.Filters;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public class HelpdeskAuthenticationFilter : IAuthenticationFilter
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        
        public HelpdeskAuthenticationFilter(IAuthenticationService authenticationService, 
                                            ISessionContext sessionContext)
        {
            _authenticationService = authenticationService;
            _sessionContext = sessionContext;
        }

        #region IAuthenticationFilter Methods

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //todo:remove logging
            var log = LogManager.Session;
            log.Debug("AuthenticationFilter called.");

            var ctx = filterContext.HttpContext;

            //authenticate only if user has been authenticated and principal has been created
            if (ctx.User.Identity.IsAuthenticated && _sessionContext.UserIdentity == null)
            {
                var isAuthenticated =_authenticationService.Authenticate(filterContext.HttpContext);
                if (!isAuthenticated)
                {
                    log.Warn("AuthenticationFilter. Failed to authenticate.");
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //todo: check if we shall issue redirect here ?
        }

        #endregion
    }
}