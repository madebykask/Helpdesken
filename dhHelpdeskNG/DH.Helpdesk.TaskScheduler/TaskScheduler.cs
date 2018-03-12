using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Jobs;
using DH.Helpdesk.TaskScheduler.Managers;
using DH.Helpdesk.TaskScheduler.Services;
using log4net.Config;
using Ninject;
using Quartz;

namespace DH.Helpdesk.TaskScheduler
{
    public partial class TaskScheduler : ServiceBase
    {
        private static IKernel _diContainer;
        private static ILog _logger;
        private readonly IScheduler _sched;

        public TaskScheduler(IKernel diContainer)
        {
            _diContainer = diContainer;
            InitializeComponent();
            _logger = LogManager.GetLogger<TaskScheduler>();

            var envName = _diContainer.Get<IServiceConfigurationManager>().EnvName;
            ServiceName = string.IsNullOrEmpty(envName) ? "DH.TaskScheduler" : $"DH.TaskScheduler.{envName}";

            var customers = _diContainer.Get<IServiceConfigurationManager>().Customers;            

            _sched = _diContainer.Get<IScheduler>();
        }

        protected override void OnStart(string[] args)
        {
            //_logger.InfoFormat("Starting service {0}", ServiceName);

            //var job = JobBuilder.Create<DailyReportJob>()
            //    .WithIdentity(Constants.DailyReportJobName)
            //    .Build();
            //var trigger = _diContainer.Get<IDailyReportService>().GetTrigger();

            //_sched.ScheduleJob(job, trigger);

            //var trackerJob = JobBuilder.Create<SettingsTrackerJob>()
            //    .WithIdentity("SettingsTrackerJob")
            //    .Build();

            //set up a job to track db starttime change of 'DailyReportJob'
            //var trackerSettingsTrigger = TriggerBuilder.Create()
            //    .WithIdentity("SettingsTrackerJobTrigger")
            //    .StartNow()
            //    .ForJob("SettingsTrackerJob")
            //    .WithSimpleSchedule(x => x
            //        .WithIntervalInSeconds(60)
            //        .RepeatForever())
            //    .Build();
            //_sched.ScheduleJob(trackerJob, trackerSettingsTrigger);


            var importI_job = JobBuilder.Create<ImportInitiatorJob>()
                .WithIdentity(Constants.ImportInitiator_JobName)
                .Build();
            var importI_trigger = _diContainer.Get<IImportInitiatorService>().GetTrigger();

            _sched.ScheduleJob(importI_job, importI_trigger);

            var importI_trackerJob = JobBuilder.Create<ImportInitiatorServiceSettingsTrackerJob>()
                .WithIdentity("ImportInitiatorSettingsTrackerJob")
                .Build();

            var importI_trackerSettingsTrigger = TriggerBuilder.Create()
                .WithIdentity("ImportInitiatorSettingsTrackerJobTrigger")
                .StartNow()
                .ForJob("ImportInitiatorSettingsTrackerJob")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();
            _sched.ScheduleJob(importI_trackerJob, importI_trackerSettingsTrigger);

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
