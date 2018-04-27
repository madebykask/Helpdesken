using System.Configuration;
using DH.Helpdesk.Common.Configuration;

namespace DH.Helpdesk.TaskScheduler.Infrastructure.Configuration
{
    #region ApplicationSettings

    internal interface IApplicationSettings
    {
        string EnvName { get; }
        string Customers { get; }

        bool IsDailyReportEnabled { get; }
        bool IsInitiatorImportEnabled { get; }
        bool IsGDPRTasksEnabled { get; }
    }

    internal class ApplicationSettingsProvider : IApplicationSettings
    {
        protected readonly System.Configuration.Configuration Config;

        public ApplicationSettingsProvider()
        {
            Config = ConfigurationManager.OpenExeConfiguration(GetType().Assembly.Location);
        }

        public string EnvName => Config.AppSettings.Settings["EnvName"]?.Value;
        public string Customers => Config.AppSettings.Settings["Customers"]?.Value;

        public bool IsDailyReportEnabled
        {
            get
            {
                var dailyReport = AppConfigHelper.GetInt32(ConfigurationManager.AppSettings["DailyReport"]) ?? 0;
                return dailyReport > 0;
            }
        }

        public bool IsInitiatorImportEnabled
        {
            get
            {
                var initiatoImport = AppConfigHelper.GetInt32(ConfigurationManager.AppSettings["InitiatorImport"]) ?? 0;
                return initiatoImport > 0;
            }
        }

        public bool IsGDPRTasksEnabled
        {
            get
            {
                var val = AppConfigHelper.GetInt32(ConfigurationManager.AppSettings["GDPRTasks"]) ?? 0;
                return val > 0;
            }
        }
    }

    #endregion

    internal class ServiceConfigurationManager: IServiceConfigurationManager
    {
        public ServiceConfigurationManager(IApplicationSettings appSettings)
        {
            AppSettings = appSettings;
        }

        public IApplicationSettings AppSettings { get; }
    }

    internal interface IServiceConfigurationManager
    {
        IApplicationSettings AppSettings { get; }
    }
}
