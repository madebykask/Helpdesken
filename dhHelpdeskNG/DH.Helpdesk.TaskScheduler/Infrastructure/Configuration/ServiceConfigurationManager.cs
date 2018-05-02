namespace DH.Helpdesk.TaskScheduler.Infrastructure.Configuration
{
    internal interface IServiceConfigurationManager
    {
        IApplicationSettings AppSettings { get; }
    }

    internal class ServiceConfigurationManager: IServiceConfigurationManager
    {
        public ServiceConfigurationManager(IApplicationSettings appSettings)
        {
            AppSettings = appSettings;
        }

        public IApplicationSettings AppSettings { get; }
    }
}
