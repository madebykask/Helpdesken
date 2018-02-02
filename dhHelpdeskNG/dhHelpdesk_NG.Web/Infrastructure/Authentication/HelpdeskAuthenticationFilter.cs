using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Web.Infrastructure.Logger;
using DH.Helpdesk.Web.Infrastructure.WorkContext.Concrete;

namespace DH.Helpdesk.Web.Infrastructure.Authentication
{
    public class HelpdeskAuthenticationFilter : IAuthenticationFilter
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISessionContext _sessionContext;
        private readonly ILoggerService _logger = LogManager.Session;

        public HelpdeskAuthenticationFilter(IAuthenticationService authenticationService, 
                                            ISessionContext sessionContext)
        {
            _authenticationService = authenticationService;
            _sessionContext = sessionContext;
        }

        #region IAuthenticationFilter Methods

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            _logger.Debug("AuthenticationFilter called.");

            var ctx = filterContext.HttpContext;
            var identity = ctx.User.Identity;

            //authenticate only if user has been authenticated and principal has been created
            if (identity != null && identity.IsAuthenticated && _sessionContext.UserIdentity == null)
            {
                var isAuthenticated =_authenticationService.SignIn(filterContext.HttpContext);
                if (!isAuthenticated)
                {
                    _logger.Warn($"AuthenticationFilter. Failed to sign in. Signing out. Identity: {identity?.Name}");

                    SignOut(ctx);

                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            //todo: check if we shall issue redirect here ?
        }

        #endregion

        private void SignOut(HttpContextBase ctx)
        {
            _authenticationService.SignOut(ctx);
        }
    }
}