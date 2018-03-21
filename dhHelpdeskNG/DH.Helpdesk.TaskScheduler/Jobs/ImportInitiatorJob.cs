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

            var settings = _importInitiatorService.GetJobSettings();
            if(settings == null) throw new ArgumentNullException(nameof(settings));
            foreach (var setting in settings)
            {
                var inputData = _importInitiatorService.ReadCsv(setting);         
                var fieldSettings = _importInitiatorService.GetInitiatorSettings(setting.CustomerId);
                _importInitiatorService.ImportInitiator(inputData, fieldSettings);
            }

        }
    }
}
