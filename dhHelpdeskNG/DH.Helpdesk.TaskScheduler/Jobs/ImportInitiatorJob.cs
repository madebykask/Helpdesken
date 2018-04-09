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
using System.Configuration;
using DH.Helpdesk.TaskScheduler.Dto;
using System.IO;

namespace DH.Helpdesk.TaskScheduler.Jobs
{
    [DisallowConcurrentExecution]
    internal class ImportInitiatorJob: IJob
    {
        private readonly ILog _logger;
        private readonly IImportInitiatorService _importInitiatorService;


        public ImportInitiatorJob(IImportInitiatorService importInitiatorService)
        {
            _logger = LogManager.GetLogger<ImportInitiatorJob>();
            _importInitiatorService = importInitiatorService;
        }

        public void Execute(IJobExecutionContext context)
        {
            _logger.InfoFormat("Job InitiatorImportJob started");
            var _logs = new DataLogModel();

            var settings = _importInitiatorService.GetJobSettings(ref _logs);
            if(settings == null) throw new ArgumentNullException(nameof(settings));
            foreach (var setting in settings)
            {
                if (setting.CustomerId != 0)
                {
                    var inputData = _importInitiatorService.ReadCsv(setting, ref _logs);
                    var fieldSettings = _importInitiatorService.GetInitiatorSettings(setting.CustomerId);

                    if(inputData.InputColumns.Any())
                    _importInitiatorService.ImportInitiator(setting, inputData, fieldSettings, ref _logs);

                    if (setting.Logging == 1)
                        _importInitiatorService.CreatLogFile(_logs);
                }
                

            }
           
        }
    }
}
