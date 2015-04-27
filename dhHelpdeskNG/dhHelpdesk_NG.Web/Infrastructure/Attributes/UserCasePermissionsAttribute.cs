namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.Infrastructure;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class UserCasePermissionsAttribute : AuthorizeAttribute
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public UserCasePermissionsAttribute()
        {
            this.unitOfWorkFactory = ManualDependencyResolver.Get<IUnitOfWorkFactory>();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = SessionFacade.CurrentUser;
            if (user == null)
            {
                return base.AuthorizeCore(httpContext);
            }

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var caseId = httpContext.Request.RequestContext.RouteData.Values["id"] ?? httpContext.Request.Params["id"];

                var caseRep = uow.GetRepository<Case>();

                var cases = caseRep.GetAll();                

                return true;
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Unathorized" }));
                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}