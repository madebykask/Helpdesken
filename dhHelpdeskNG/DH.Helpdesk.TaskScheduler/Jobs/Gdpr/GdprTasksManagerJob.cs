using System;
using System.Linq;
using Common.Logging;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.Gdpr
{
    [DisallowConcurrentExecution]
    internal class GdprTasksManagerJob : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(GdprTasksManagerJob));
        private readonly IGDPRJobSettings _settings;
        private readonly IGDPRTasksService _gdprTasksService;

        #region ctor()

        public GdprTasksManagerJob(IGDPRJobSettings settings, IGDPRTasksService gdprTasksService)
        {
            _settings = settings;
            _gdprTasksService = gdprTasksService;
        }

        #endregion

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var pendingTasks = _gdprTasksService.GetPendingTasks();
                if (pendingTasks.Any())
                {
                    _log.Debug($"Found {pendingTasks.Count} to process.");

                    foreach (var pendingTask in pendingTasks)
                    {
                        ProcessTask(context, pendingTask);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Failed to trigger GdprDataPrivacyJob.", ex);
            }
        }

        private void ProcessTask(IJobExecutionContext context, GDPRTask pendingTask)
        {
            var hasStatusChanged = false;
            try
            {
                _log.Debug($"Creating new job for task Id={pendingTask.Id}");

                var job = JobBuilder.Create<GdprDataPrivacyJob>()
                    .WithIdentity(Constants.GDPRDataPrivacyJobName, "GDPR")
                    .StoreDurably()
                    .Build();

                job.JobDataMap.Put(GdprDataPrivacyJob.DataMapKey, pendingTask.Id);

                _gdprTasksService.UpdateTaskStatus(pendingTask.Id, GDPRTaskStatus.Scheduled);
                hasStatusChanged = true;

                //run job immediately
                context.Scheduler.AddJob(job, true);
                context.Scheduler.TriggerJob(job.Key);

                _log.Debug($"New job for task Id={pendingTask.Id} has been scheduled.");
            }
            catch (Exception ex)
            {
                _log.Error($"Unknown error on processing task Id={pendingTask.Id}.", ex);
                if (hasStatusChanged)
                    _gdprTasksService.UpdateTaskStatus(pendingTask.Id, GDPRTaskStatus.None);
            }
        }
    }
}