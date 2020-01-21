namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Ninject;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Cases;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Case;
    using DH.Helpdesk.Services.Infrastructure;
    using DH.Helpdesk.Services.Services;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class UserCasePermissionsAttribute : AuthorizeAttribute
    {
        private readonly IUserService _userService;
        private readonly ICaseService _caseService;

        private bool _isCaseExists { get; set; }

        #region ctor()

        public UserCasePermissionsAttribute()
        {
        }

        [Inject]
        public UserCasePermissionsAttribute(IUserService userService, ICaseService caseService)
        {
            _userService = userService;
            _caseService = caseService;
        }

        #endregion

        #region AuthorizeCore

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = SessionFacade.CurrentUser;
            if (user == null)
            {
                return base.AuthorizeCore(httpContext);
            }

            var caseIdParam = httpContext.Request.RequestContext.RouteData.Values["id"] ?? httpContext.Request.Params["id"];
            _isCaseExists = true;

            var caseId = 0;
            if (int.TryParse(caseIdParam?.ToString(), out caseId))
            {
                if (!_caseService.IsCaseExist(caseId))
                    return true;

                var isAuthorised = _userService.VerifyUserCasePermissions(user, caseId);
                return isAuthorised;
            }

            return false;
        }

        #endregion

        #region HandleUnauthorizedRequest

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Unathorized" }));
                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }

        #endregion
    }
}