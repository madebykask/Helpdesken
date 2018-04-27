using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Jobs;
using DH.Helpdesk.TaskScheduler.Services;
using Ninject;
using Quartz;
using DH.Helpdesk.TaskScheduler.Dto;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using Quartz.Listener;
using Quartz.Impl.Matchers;

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

            /*Daily Report*/
            if (_applicationSettings.IsDailyReportEnabled)
            {
                var job = JobBuilder.Create<DailyReportJob>()
                    .WithIdentity(Constants.DailyReportJobName)
                    .Build();

                var trigger = _diContainer.Get<IDailyReportService>().GetTrigger();

                _sched.ScheduleJob(job, trigger);
            }

            var _logs = new DataLogModel();

            /*Import Initiator*/
            if (_applicationSettings.IsInitiatorImportEnabled)
            {
                var _initiatorImportService = _diContainer.Get<IImportInitiatorService>();
                var initiatorTriggers = _initiatorImportService.GetTriggers(ref _logs).ToList();


                var i = 0;
                var firstJob = JobBuilder.Create<ImportInitiatorJob>().Build();
                ITrigger firstTrigger = null;
                JobKey lastJobKey = null;
                JobChainingJobListener listener = new JobChainingJobListener("Pipeline Chain");
                IList<IJobDetail> jobs = new List<IJobDetail>();
                foreach (var initTrigger in initiatorTriggers)
                {                   
                    var initiatorJob =
                        JobBuilder.Create<ImportInitiatorJob>()
                                  .WithIdentity($"{Constants.ImportInitiator_JobName}_{i}")
                                  .Build();
                    
                    if (i == 0)
                    {
                        firstJob = initiatorJob;
                        firstTrigger = initTrigger;                        
                    }
                    else
                    {
                        jobs.Add(initiatorJob);
                        listener.AddJobChainLink(lastJobKey, initiatorJob.Key);
                    }

                    lastJobKey = initiatorJob.Key;
                    i++;
                }

                _sched.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.GroupEquals("Pipeline"));

                _sched.ScheduleJob(firstJob, firstTrigger);
                foreach(var _job in jobs)
                {
                    _sched.AddJob(_job, false, true);
                }
            }

            if (_applicationSettings.IsGDPRTasksEnabled)
            {
                //TODO: run new GDPRTasksManagerJob
            }

            /*Job Tracker*/
            var trackerJob = JobBuilder.Create<SettingsTrackerJob>()
               .WithIdentity("SettingsTrackerJob")
               .Build();

            // set up a job to track db starttime change of 'DailyReportJob'
            var trackerSettingsTrigger = TriggerBuilder.Create()
                .WithIdentity("SettingsTrackerJobTrigger")
                .StartNow()
                .ForJob("SettingsTrackerJob")
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();

            _sched.ScheduleJob(trackerJob, trackerSettingsTrigger);

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
