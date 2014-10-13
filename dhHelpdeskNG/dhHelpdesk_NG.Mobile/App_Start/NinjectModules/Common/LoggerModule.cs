namespace DH.Helpdesk.Mobile.NinjectModules.Common
{
    using DH.Helpdesk.Common.Logger;
    using DH.Helpdesk.Mobile.Infrastructure.Logger;

    using Ninject.Modules;

    public class LoggerModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IStartUpTask>().To<Log4NetStartUpTask>().InSingletonScope();
            this.Bind<ILoggerService>()
                .To<Log4NetLoggerService>()
                .InSingletonScope()
                .Named(Log4NetLoggerService.LogType.EMAIL)
                .WithConstructorArgument(Log4NetLoggerService.LogType.EMAIL);
            this.Bind<ILoggerService>()
                .To<Log4NetLoggerService>()
                .InSingletonScope()
                .Named(Log4NetLoggerService.LogType.ERROR)
                .WithConstructorArgument(Log4NetLoggerService.LogType.ERROR);
        }
    }
}