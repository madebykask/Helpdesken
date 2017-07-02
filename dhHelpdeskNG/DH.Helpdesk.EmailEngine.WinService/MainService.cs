using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DH.Helpdesk.Dal.DbContext;
using DH.Helpdesk.Domain;
using log4net;
using log4net.Config;
using log4net.Core;
using DH.Helpdesk.EmailEngine.Library;
using System.IO;

namespace DH.Helpdesk.EmailEngine.WinService
{
	public partial class MainService : ServiceBase
	{
		private Timer _timer { get; set; }
		private ILog _logger { get; set; }

		public MainService()
		{
			InitializeComponent();


			var delay = int.Parse(ConfigurationManager.AppSettings["Delay"]) * 1000;

			_timer = new Timer
		    {
		        Interval = delay,
		        AutoReset = false
		    };
		    _timer.Elapsed += (sender, args) => { Loop(); };

			XmlConfigurator.Configure();
			_logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		}

		protected override void OnStart(string[] args)
		{
			_logger.Info("Service started");
			_timer.Start();
		}

		protected override void OnStop()
		{
			_logger.Info("Service stopped");
		}

		private void Loop()
		{
			try
			{
				ProcessEmails();
			}
			catch (Exception ex)
			{
				_logger.Error("Exception occured.", ex);
			}
			finally
			{
				_timer.Start();
			}
		}

		private void ProcessEmails()
		{
			var emailProcessor = new EmailProcessor(_logger);
			emailProcessor.ProcessEmails();
		}
	}
}
