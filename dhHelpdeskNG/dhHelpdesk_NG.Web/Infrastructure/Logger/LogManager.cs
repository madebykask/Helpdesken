namespace DH.Helpdesk.Web.Infrastructure.Logger
{
    using DH.Helpdesk.Common.Logger;

    using Microsoft.Practices.ServiceLocation;

    public static class LogManager
    {
        public static ILoggerService Email
        {
            get { return ServiceLocator.Current.GetInstance<ILoggerService>(Log4NetLoggerService.LogType.EMAIL); }
        }

        public static ILoggerService Error
        {
            get { return ServiceLocator.Current.GetInstance<ILoggerService>(Log4NetLoggerService.LogType.ERROR); }
        }

        public static ILoggerService DataImport
        {
            get { return ServiceLocator.Current.GetInstance<ILoggerService>(Log4NetLoggerService.LogType.DATA_IMPORT); }
        }

        public static ILoggerService Session
        {
            get { return ServiceLocator.Current.GetInstance<ILoggerService>(Log4NetLoggerService.LogType.Session); }
        }
        public static ILoggerService reCaptcha
        {
            get { return ServiceLocator.Current.GetInstance<ILoggerService>(Log4NetLoggerService.LogType.reCaptcha); }
        }
    }
}