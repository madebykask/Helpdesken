using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using DH.Helpdesk.TaskScheduler.Services;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.DailReport
{
    internal class DailyReportJobInitializer : IJobInitializer
    {
        private readonly IApplicationSettings _applicationSettings;
        private readonly IDailyReportService _dailyReportService;
        private readonly IScheduler _scheduler;

        public DailyReportJobInitializer(IApplicationSettings applicationSettings,
            IScheduler scheduler,
            IDailyReportService dailyReportService)
        {
            _scheduler = scheduler;
            _applicationSettings = applicationSettings;
            _dailyReportService = dailyReportService;
        }

        public void Run()
        {
            if (!_applicationSettings.IsDailyReportEnabled)
                return;
            
            var job = JobBuilder.Create<DailyReportJob>()
                .WithIdentity(Constants.DailyReportJobName)
                .Build();

            var trigger = _dailyReportService.GetTrigger();
            _scheduler.ScheduleJob(job, trigger);

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

            _scheduler.ScheduleJob(trackerJob, trackerSettingsTrigger);
        }
    }
}