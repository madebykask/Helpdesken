using System;
using System.Diagnostics;
using System.ServiceProcess;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Jobs;
using Ninject;
using Quartz;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using DH.Helpdesk.Domain.GDPR;
using System.Threading.Tasks;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Dal.Repositories.GDPR;
using System.Threading;
using System.Linq;

namespace DH.Helpdesk.TaskScheduler
{
    public partial class TaskScheduler : ServiceBase
    {
        private static IKernel _diContainer;
        private static ILog _logger;
        private readonly IScheduler _sched;
        private readonly IApplicationSettings _applicationSettings;
        private readonly IGDPRTaskRepository _gdprTasksRepository;

        #region ctor()

        public TaskScheduler(IKernel diContainer)
        {
            _diContainer = diContainer;
            _logger = LogManager.GetLogger<TaskScheduler>();
            _applicationSettings = _diContainer.Get<IServiceConfigurationManager>().AppSettings;
            _gdprTasksRepository = _diContainer.Get<IGDPRTaskRepository>();

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
                _logger.Debug($"Stopping data privacy job.");
                if (Program.ProcessedTaskId > 0)
                {
                    IGDPRTasksService _gdprTasksService = new GDPRTasksService(_gdprTasksRepository);

                    var taskInfo = _gdprTasksService.GetById(Program.ProcessedTaskId);

                    if (taskInfo.Status != GDPRTaskStatus.Complete)
                    {
                        taskInfo.Error = $"Data privacy job for taskId: {Program.ProcessedTaskId} has been stopped before finishing";
                        taskInfo.Progress = 0;
                        taskInfo.EndedAt = DateTime.UtcNow;
                        taskInfo.Success = false;
                        taskInfo.Status = GDPRTaskStatus.Complete;

                        _logger.Debug($"Data privacy job for taskId: {Program.ProcessedTaskId} has been stopped before finishing");
                        _gdprTasksService.UpdateTask(taskInfo);
                        Environment.Exit(1);
                    }
                }
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
