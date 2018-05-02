using System.Linq;
using Common.Logging;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.Services;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.Gdpr
{
    public class GdprTasksManagerJob : IJob
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(GdprTasksManagerJob));
        private readonly IGDPRTasksService _gdprTasksService;

        #region ctor()

        public GdprTasksManagerJob(IGDPRTasksService gdprTasksService)
        {
            _gdprTasksService = gdprTasksService;
        }

        #endregion

        public void Execute(IJobExecutionContext context)
        {
            var pendingTasks = _gdprTasksService.GetPendingTasks();
            if (pendingTasks.Any())
            {
                _log.Debug($"Found {pendingTasks.Count} to process.");

                foreach (var pendingTask in pendingTasks)
                {
                    var job = JobBuilder.Create<GdprDataPrivacyJob>()
                                        .WithIdentity($"{Constants.GDPRDataPrivacyJobName}_{pendingTask.Id}", "GDPR")
                                        .StoreDurably()
                                        .Build();

                    job.JobDataMap.Put(GdprDataPrivacyJob.DataMapKey, pendingTask.Id);

                    //run job immediately
                    context.Scheduler.AddJob(job, true);
                    context.Scheduler.TriggerJob(job.Key);

                    _gdprTasksService.UpdateTaskStatus(pendingTask.Id, GDPRTaskStatus.Scheduled);
                    _log.Debug($"Scheduled task with Id={pendingTask.Id}");
                }
            }
        }
    }
}