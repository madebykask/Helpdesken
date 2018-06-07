using System;
using System.Diagnostics;
using System.ServiceProcess;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Jobs;
using Ninject;
using Quartz;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;

namespace DH.Helpdesk.TaskScheduler
{
    public partial class TaskScheduler : ServiceBase
    {
        private static IKernel _diContainer;
        private static ILog _logger;
        private readonly IScheduler _sched;
        private readonly IApplicationSettings _applicationSettings;

        #region ctor()

        public TaskScheduler(IKernel diContainer)
        {
            _diContainer = diContainer;
            _logger = LogManager.GetLogger<TaskScheduler>();
            _applicationSettings = _diContainer.Get<IServiceConfigurationManager>().AppSettings;

            var envName = _applicationSettings.EnvName;
            ServiceName = string.IsNullOrEmpty(envName) ? "DH.TaskScheduler" : $"DH.TaskScheduler.{envName}";
            _sched = _diContainer.Get<IScheduler>();

            InitializeComponent();
        }

        #endregion

        protected override void OnStart(string[] args)
        {
            _logger.InfoFormat("Starting service {0}", ServiceName);

            //Debugger.Launch();

            var initializers = _diContainer.GetAll<IJobInitializer>();
            foreach (var initializer in initializers)
            {
                initializer.Run();
            }

            if (_sched.GetJobGroupNames().Count > 0)
                _sched.Start();
                
        }

        protected override void OnStop()
        {
            try
            {
                _sched?.Shutdown();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.InnerException);//TODO: Add more information
                throw;
            }
            //_logger.InfoFormat("Service {0} is stopped", ServiceName);
        }
    }
}
