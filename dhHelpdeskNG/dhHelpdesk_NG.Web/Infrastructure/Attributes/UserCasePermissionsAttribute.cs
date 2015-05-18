namespace DH.Helpdesk.Web.Infrastructure.Attributes
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

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
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IUserService userService;

        public UserCasePermissionsAttribute()
        {
            this.unitOfWorkFactory = ManualDependencyResolver.Get<IUnitOfWorkFactory>();
            this.userService = ManualDependencyResolver.Get<IUserService>();
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

                var customerIds = this.userService.GetUserProfileCustomersSettings(user.Id).Select(c => c.CustomerId).ToList();

                var caseRep = uow.GetRepository<Case>();
                var userDepartmentRep = uow.GetRepository<DepartmentUser>();
                var userRep = uow.GetRepository<User>();
                var customerRep = uow.GetRepository<Customer>();
                var customerUserRep = uow.GetRepository<CustomerUser>();

                var cases = caseRep.GetAll();
                var userDepartments = userDepartmentRep.GetAll();
                var users = userRep.GetAll().GetById(user.Id);
                var customers = customerRep.GetAll().GetByIds(customerIds);
                var customerUsers = customerUserRep.GetAll();

                if (user.RestrictedCasePermission.ToBool())
                {
                    switch (user.UserGroupId)
                    {
                        case (int)BusinessData.Enums.Admin.Users.UserGroup.Administrator:
                            cases = cases.GetByAdministratorOrResponsibleUser(user.Id, user.Id);
                            break;
                        case (int)BusinessData.Enums.Admin.Users.UserGroup.User:
                            cases = cases.GetByReportedByOrUserId(user.UserId, user.Id);
                            break;
                    }
                }

                return CasesMapper.MapToUserCaseIds(
                            cases, 
                            userDepartments, 
                            users, 
                            customers,
                            customerUsers)
                            .Contains(int.Parse((string)caseId));
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