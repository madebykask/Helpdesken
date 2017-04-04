using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure.Attributes;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users;
    using DH.Helpdesk.Services.BusinessLogic.Admin.Users.Concrete;

    using Ninject.Modules;

    internal sealed class AdminModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IUserPermissionsChecker>().To<UserPermissionsChecker>();

            this.BindFilter<UserCasePermissionsAttribute>(FilterScope.Action, 0)
                .WhenActionMethodHas<UserCasePermissionsAttribute>()
                .InRequestScope();
        }
    }
}