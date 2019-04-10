using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DH.Helpdesk.EmailEngine.WinService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : System.Configuration.Install.Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}

		public override void Install(IDictionary stateSaver)
		{
			SetServiceName();
			base.Install(stateSaver);
		}

		public override void Uninstall(IDictionary savedState)
		{
			SetServiceName();
			base.Uninstall(savedState);
		}

		private void SetServiceName()
		{
			var defaultName = "DH Helpdesk Email Engine";
			var config = ConfigurationManager.OpenExeConfiguration(Assembly.GetAssembly(typeof(ProjectInstaller)).Location);
			string serviceName;
			if (config.AppSettings.Settings.AllKeys.Contains("ServiceName"))
			{
				serviceName = config.AppSettings.Settings["ServiceName"].Value.Trim();
				serviceName = serviceName == string.Empty ? defaultName : serviceName;
			}
			else
			{
				serviceName = defaultName;
			}

			this.serviceInstaller1.ServiceName = serviceName;
			this.serviceInstaller1.DisplayName = serviceName;
		}
	}
}
