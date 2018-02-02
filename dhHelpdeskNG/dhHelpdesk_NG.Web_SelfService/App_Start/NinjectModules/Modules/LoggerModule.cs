using DH.Helpdesk.Common.Logger;
using log4net.Core;
using Ninject.Modules;
using Ninject.Web.Common;

namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
    public class LoggerModule : NinjectModule
    {
        public override void Load()
        {
            //default logger
            this.Bind<ILoggerService>()
                .To<Log4NetLoggerService>()
                .InRequestScope()
                .WithConstructorArgument("logType", x => x.Request.Target.Type.DeclaringType.ToString());
                
            this.Bind<ILoggerService>()
                .To<Log4NetLoggerService>()
                .InSingletonScope()
                .Named(Log4NetLoggerService.LogType.ERROR)
                .WithConstructorArgument(Log4NetLoggerService.LogType.ERROR);

            this.Bind<ILoggerService>()
                .To<Log4NetLoggerService>()
                .InSingletonScope()
                .Named(Log4NetLoggerService.LogType.Session)
                .WithConstructorArgument(Log4NetLoggerService.LogType.Session);
        }
    }
}