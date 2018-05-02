using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.Gdpr
{
    internal class GdprJobInitializer : IJobInitializer
    {
        private readonly IScheduler _scheduler;
        private readonly IApplicationSettings _appSettings;

        public GdprJobInitializer()
        {
        }

        public GdprJobInitializer(IScheduler scheduler, IApplicationSettings appSettings)
        {
            _scheduler = scheduler;
            _appSettings = appSettings;
        }

        public void Run()
        {
            if (!_appSettings.IsGDPRTasksEnabled)
                return;

            var jobId = nameof(GdprTasksManagerJob);

            var job = JobBuilder.Create<GdprTasksManagerJob>()
                .WithIdentity(jobId, "GDPR_mngr")
                .Build();

            var trigger =
                TriggerBuilder.Create()
                    .WithIdentity($"{jobId}_trigger", "GDPR_mngr")
                    .StartNow()
                    .ForJob(job.Key)
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(50)
                        .RepeatForever())
                    .Build();

            _scheduler.ScheduleJob(job, trigger);
        }
    }
}