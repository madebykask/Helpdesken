using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace DH.Helpdesk.TaskScheduler
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(this.GetType().Assembly.Location);
            var appSettings = configuration.AppSettings;
            var envName = appSettings.Settings["EnvName"]?.Value;

            InitializeComponent();
            var name = string.IsNullOrEmpty(envName) ? "DH.TaskScheduler" : $"DH.TaskScheduler.{envName}";
            //serviceInstaller1.Description = name;
            serviceInstaller1.DisplayName = name;
            serviceInstaller1.ServiceName = name;
        }
    }
}
