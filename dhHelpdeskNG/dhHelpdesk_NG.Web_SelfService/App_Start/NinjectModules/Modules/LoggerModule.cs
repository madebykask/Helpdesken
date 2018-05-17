using DH.Helpdesk.Common.Logger;
using Ninject.Modules;

namespace DH.Helpdesk.SelfService.NinjectModules.Modules
{
    public class LoggerModule : NinjectModule
    {
        public override void Load()
        {
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