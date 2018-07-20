using DH.Helpdesk.Common.Logger;
using Ninject.Modules;

namespace DH.Helpdesk.WebApi.Infrastructure.Config.DependencyInjection
{
    public class LoggerModule : NinjectModule
    {
        public override void Load()
        {

            //Bind<ILoggerService>()
            //    .To<Log4NetLoggerService>()
            //    .InSingletonScope()
            //    .Named(Log4NetLoggerService.LogType.EMAIL)
            //    .WithConstructorArgument(Log4NetLoggerService.LogType.EMAIL);

            Bind<ILoggerService>()
                .To<Log4NetLoggerService>()
                .InSingletonScope()
                .Named(Log4NetLoggerService.LogType.ERROR)
                .WithConstructorArgument(Log4NetLoggerService.LogType.ERROR);

            Bind<ILoggerService>()
                .To<Log4NetLoggerService>()
                .InSingletonScope()
                .Named(Log4NetLoggerService.LogType.Session)
                .WithConstructorArgument(Log4NetLoggerService.LogType.Session);
        }
    }
}