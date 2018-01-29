using System.Web.Mvc;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Web.Infrastructure.Authentication;
using DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FilterBindingSyntax;

namespace DH.Helpdesk.Web.NinjectModules.Modules
{
    public class AuthenticationModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IAuthenticationServiceBehaviorFactory>().To<AuthenticationServiceBehaviorFactory>().InRequestScope(); 
            Bind<IAuthenticationService>().To<AuthenticationService>().InRequestScope();
            Bind<IFederatedAuthenticationService>().To<FederatedAuthenticationService>().InRequestScope();

            this.BindFilter<HelpdeskAuthenticationFilter>(FilterScope.Controller, 0).InRequestScope();
        }
    }
}