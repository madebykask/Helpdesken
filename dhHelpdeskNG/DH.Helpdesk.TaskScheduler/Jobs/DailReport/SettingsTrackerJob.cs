using System;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Dto;
using DH.Helpdesk.TaskScheduler.Services;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.DailReport
{
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    internal class SettingsTrackerJob: IJob
    {
        private readonly ILog _logger;
        private readonly IDailyReportService _dailyReportService;

        public SettingsTrackerJob(IDailyReportService dailyReportService)
        {
            _logger = LogManager.GetLogger<SettingsTrackerJob>();
            _dailyReportService = dailyReportService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var previousSettings = "";
            const string settingsName = "DailyReportSettings";

            var settings = _dailyReportService.GetJobSettings();
            var currentSettings = GetValues(settings);

            var dataMap = context.JobDetail.JobDataMap;
            if (dataMap.ContainsKey(settingsName))
                previousSettings = dataMap.GetString(settingsName);
            else
                dataMap.Add(settingsName, currentSettings);

            if (!previousSettings.Equals(GetValues(settings), StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.DebugFormat("Job start time changed - was: {0}, now: {1}", previousSettings, currentSettings);
                UpdateQuartzJobTrigger(context);
                dataMap[settingsName] = currentSettings;
            }
        }

        private string GetValues(JobSettings settings)
        {
            if (settings == null) return "";

            return $"{settings.StartTime}_{settings.CronExpression}_{settings.TimeZone}";
        }

        private void UpdateQuartzJobTrigger(IJobExecutionContext context)
        {
            var trigger = _dailyReportService.GetTrigger();
            var result = context.Scheduler.RescheduleJob(trigger.Key, trigger);
            _logger.InfoFormat("Job {0} is rescheduled", trigger.JobKey.Name);
        }
    }
}
