using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler.Managers
{
    internal class ServiceConfigurationManager: IServiceConfigurationManager
    {
        protected readonly Configuration Config;

        public ServiceConfigurationManager()
        {
            Config = ConfigurationManager.OpenExeConfiguration(GetType().Assembly.Location);
        }

        public string EnvName => Config.AppSettings.Settings["EnvName"]?.Value;
        public string Customers => Config.AppSettings.Settings["Customers"]?.Value;
    }

    public interface IServiceConfigurationManager
    {
        string EnvName { get; }
        string Customers { get; }
    }
}
