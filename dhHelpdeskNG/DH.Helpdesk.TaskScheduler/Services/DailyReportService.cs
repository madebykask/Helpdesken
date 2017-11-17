using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Logging;
using CsvHelper;
using DH.Helpdesk.Dal.DbQueryExecutor;
using DH.Helpdesk.TaskScheduler.Dto;
using Newtonsoft.Json;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Services
{
    internal class DailyReportService : IDailyReportService
    {
        private readonly IDbQueryExecutorFactory _execFactory;
        private readonly ILog _logger;

        public DailyReportService(IDbQueryExecutorFactory execFactory)
        {
            _execFactory = execFactory;
            _logger = LogManager.GetLogger<DailyReportService>();
        }

        public JobSettings GetJobSettings()
        {
            var dbQueryExecutor = _execFactory.Create();
            var settings = dbQueryExecutor.QuerySingleOrDefault<JobSettings>("Select * from tblReportScheduler");
            //_logger.DebugFormat("Settings: {0}", settings != null ? JsonConvert.SerializeObject(settings) : "null");
            return settings;
        }

        public ITrigger GetTrigger()
        {
            var settings = GetJobSettings();
            if (settings == null)
            {
                return TriggerBuilder.Create()
                    .WithIdentity(Constants.DailyReportJobTriggerName)
                    .ForJob(Constants.DailyReportJobName)
                    //.StartNow()
                    .EndAt(DateTimeOffset.UtcNow)
                    .Build();
            }
            //if (settings == null) throw new ArgumentNullException(nameof(settings));

            var trigger = TriggerBuilder.Create()
                        .WithIdentity(Constants.DailyReportJobTriggerName)
                        .StartNow()
                        .ForJob(Constants.DailyReportJobName);

            var cronExpression = "";
            if (!string.IsNullOrWhiteSpace(settings.CronExpression))
            {
                cronExpression = settings.CronExpression;
            } else if (!string.IsNullOrWhiteSpace(settings.StartTime))
            {
                var r = new Regex(@"(\d{1,2}):(\d{1,2})");
                var match = r.Match(settings.StartTime);
                if (match.Success)
                {
                    cronExpression = $"0 {match.Groups[2].Value} {match.Groups[1].Value} ? * *";
                }
            }
            if(string.IsNullOrWhiteSpace(cronExpression)) throw new Exception("No StartTime or CronExpression defined.");

            TimeZoneInfo timeZoneInfo;
            if (!string.IsNullOrWhiteSpace(settings.TimeZone))
            {
                try
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(settings.TimeZone);
                }
                catch (InvalidTimeZoneException)
                {
                    timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");
                }
            } else
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("UTC");

            _logger.DebugFormat("Creating trigger - CronExp:{0}, TimeZone:{1}", cronExpression, timeZoneInfo.Id);
            return trigger.WithSchedule(CronScheduleBuilder.CronSchedule(cronExpression)
                .InTimeZone(timeZoneInfo))
                .Build();
        }

        public DataTable GetJobData(string query)
        {
            if (string.IsNullOrEmpty(query)) throw new ArgumentNullException(nameof(query));

            var dbQueryExecutor = _execFactory.Create();
            return dbQueryExecutor.ExecuteTable(query);
        }

        public void CreateCsv(IList<object> data, JobSettings settings)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            const string defaultFileName = "dailyReport";
            var dateTimeExtension = settings.AppendTime ? $"{DateTime.Now.ToString("_d_M_yyyy_H_m_s")}" : "";

            var filePath = settings.TargetFolder;
            var fileName = $"{settings.OutputFilename ?? defaultFileName}{dateTimeExtension}.csv";
            if(!Directory.Exists(settings.TargetFolder)) throw new DirectoryNotFoundException($"Directory {settings.TargetFolder} not found.");

            using (var textWriter = File.CreateText(filePath + fileName))
            {
                using (var csv = new CsvWriter(textWriter))
                {
                    csv.Configuration.Delimiter = ";";
                    _logger.DebugFormat($"Saving file {fileName} to path {filePath}");
                    csv.WriteRecords(data);
                }
            }
        }

        public void UpdateLastRun(int id)
        {
            var dbQueryExecutor = _execFactory.Create();
            dbQueryExecutor.ExecQuery("Update [tblReportScheduler] set [LastRun]=@Datetime where [Id] = @Id", new { DateTime = DateTime.UtcNow, Id = id });
        }
    }

    internal interface IDailyReportService
    {
        JobSettings GetJobSettings();

        ITrigger GetTrigger();

        DataTable GetJobData(string query);

        void CreateCsv(IList<object> data, JobSettings settings);

        void UpdateLastRun(int id);
    }
}
