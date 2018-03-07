using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Dto;
using DH.Helpdesk.TaskScheduler.Services;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs
{
    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    internal class ImportInitiatorServiceSettingsTrackerJob : IJob
    {
        private readonly ILog _logger;
        private readonly IImportInitiatorService _importInitiatorService;

        public ImportInitiatorServiceSettingsTrackerJob(IImportInitiatorService importInitiatorService)
        {
            _logger = LogManager.GetLogger<SettingsTrackerJob>();
            _importInitiatorService = importInitiatorService;
        }

        public void Execute(IJobExecutionContext context)
        {
            var previousSettings = "";
            const string settingsName = "ImportInitiatorSettings";

            var settings = _importInitiatorService.GetJobSettings();
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

        private string GetValues(ImportInitiator_JobSettings settings)
        {
            if (settings == null) return "";

            return $"{settings.StartTime}_{settings.CronExpression}_{settings.TimeZone}";
        }

        private void UpdateQuartzJobTrigger(IJobExecutionContext context)
        {
            var trigger = _importInitiatorService.GetTrigger();
            var result = context.Scheduler.RescheduleJob(trigger.Key, trigger);
            _logger.InfoFormat("Job {0} is rescheduled", trigger.JobKey.Name);
        }
    }
}
