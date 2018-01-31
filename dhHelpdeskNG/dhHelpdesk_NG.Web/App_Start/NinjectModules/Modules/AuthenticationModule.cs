using System.Web.Mvc;
using DH.Helpdesk.Common.Logger;
using DH.Helpdesk.Services.Services.Authentication;
using DH.Helpdesk.Web.Infrastructure.Authentication;
using DH.Helpdesk.Web.Infrastructure.Authentication.Behaviors;
using Ninject.Modules;

using Ninject;
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
            Bind<IFederatedAuthenticationService>()
                .ToMethod(ctx => new FederatedAuthenticationService(ctx.Kernel.Get<ILoggerService>(Log4NetLoggerService.LogType.Session)))
                .InRequestScope();

            this.BindFilter<HelpdeskAuthenticationFilter>(FilterScope.Controller, 0).InRequestScope();
        }
    }
}