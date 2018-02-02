using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Services;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs
{
    [DisallowConcurrentExecution]
    internal class DailyReportJob: IJob
    {
        private readonly ILog _logger;
        private readonly IDailyReportService _dailyReportService;

        public DailyReportJob(IDailyReportService dailyReportService)
        {
            _logger = LogManager.GetLogger<DailyReportJob>();
            _dailyReportService = dailyReportService;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("Job DailyReportJob started");

            var settings = _dailyReportService.GetJobSettings();
            if(settings == null) throw new ArgumentNullException(nameof(settings));

            var data = _dailyReportService.GetJobData(settings.SqlQuery);

            var enumData = data.AsEnumerable().Select(r =>
            {
                IDictionary<string, object> rowObj = new ExpandoObject();
                foreach (var col in data.Columns.Cast<DataColumn>())
                {
                    rowObj.Add(col.ColumnName, r[col.ColumnName] ?? "");
                }
                return (object)rowObj;

            }).ToList();

            _dailyReportService.CreateCsv(enumData, settings);

            _dailyReportService.UpdateLastRun(settings.Id);

        }
    }
}
