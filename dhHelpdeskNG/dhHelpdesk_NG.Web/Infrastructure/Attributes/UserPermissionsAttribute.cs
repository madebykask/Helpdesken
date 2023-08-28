namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DH.Helpdesk.BusinessData.Enums.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Users;
    using DH.Helpdesk.Services.Infrastructure;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    internal sealed class UserPermissionsAttribute : AuthorizeAttribute
    {
        private readonly IUserPermissionsChecker userPermissionsChecker;

        public UserPermissionsAttribute()
        {
            this.userPermissionsChecker = ManualDependencyResolver.Get<IUserPermissionsChecker>();
        }

        public UserPermissionsAttribute(UserPermission[] permissions) : this()
        {
            this.Permissions = permissions;
        }

        public UserPermissionsAttribute(UserPermission permission) : this()
        {
            this.Permissions = new[] { permission };
        }

        public UserPermission[] Permissions { get; private set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = SessionFacade.CurrentUser;
            if (user == null)
            {
                return base.AuthorizeCore(httpContext);   
            }

            return this.userPermissionsChecker.UserHasAnyPermissions(UsersMapper.MapToUser(user), this.Permissions.ToList());
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Unathorized", area = "" }));
                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}