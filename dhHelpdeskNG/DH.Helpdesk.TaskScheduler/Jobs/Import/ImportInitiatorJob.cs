using System;
using System.Linq;
using Common.Logging;
using DH.Helpdesk.TaskScheduler.Dto;
using DH.Helpdesk.TaskScheduler.Services;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.Import
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

                    var deletingDays = setting.Days2WaitBeforeDelete;

                    //Delete
                    if (deletingDays > 0)                                            
                        _importInitiatorService.DeleteInitiators(deletingDays, setting.CustomerId, ref _logs);                    

                    if (inputData.InputColumns.Any())
                    _importInitiatorService.ImportInitiator(setting, inputData, fieldSettings, ref _logs);

                    if (setting.Logging == 1)
                        _importInitiatorService.CreatLogFile(_logs);
                }
                

            }
           
        }
    }
}
