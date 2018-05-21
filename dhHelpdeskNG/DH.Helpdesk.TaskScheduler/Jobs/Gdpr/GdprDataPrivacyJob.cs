using System;
using System.Linq;
using Common.Logging;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.BusinessLogic.Gdpr;
using DH.Helpdesk.Services.Services;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.Gdpr
{
    [DisallowConcurrentExecution]
    public class GdprDataPrivacyJob : IJob
    {
        public const string DataMapKey = "data";

        private readonly ILog _log = LogManager.GetLogger(typeof(GdprDataPrivacyJob));
        private readonly IGDPRTasksService _gdprTasksService;
        private readonly IGDPRDataPrivacyFavoriteRepository _dataPrivacyFavoriteRepository;
        private readonly IGDPRDataPrivacyProcessor _dataPrivacyProcessor;

        #region ctor()

        public GdprDataPrivacyJob(IGDPRTasksService gdprTasksService, 
            IGDPRDataPrivacyFavoriteRepository dataPrivacyFavoriteRepository,
            IGDPRDataPrivacyProcessor dataPrivacyProcessor)
        {
            _gdprTasksService = gdprTasksService;
            _dataPrivacyFavoriteRepository = dataPrivacyFavoriteRepository;
            _dataPrivacyProcessor = dataPrivacyProcessor;
        }

        #endregion

        public void Execute(IJobExecutionContext context)
        {
            _log.Debug("Starting data privacy job...");

            var isSuccess = true;
            var errorMsg = string.Empty;

            var jobDataMap = context.MergedJobDataMap;
            if (jobDataMap.ContainsKey(DataMapKey))
            {
                var taskId = Convert.ToInt32(jobDataMap[DataMapKey]);
                var taskInfo = _gdprTasksService.GetById(taskId);

                //run only scheduled tasks 
                if (taskInfo.Status != GDPRTaskStatus.Scheduled)
                {
                    _log.Warn($"Task (Id={taskId}) has already been completed.");
                    return;
                }

                _log.Debug("Executing data privacy action.");

                var parameters = CreateParameters(taskInfo);

                //update status before running 
                var startedAt = DateTime.UtcNow;
                _gdprTasksService.UpdateTaskStatus(taskInfo.Id, GDPRTaskStatus.Running);

                try
                {
                    _dataPrivacyProcessor.Process(taskInfo.CustomerId, taskInfo.UserId, parameters);
                    _log.Debug($"Data privacy task (id={taskInfo.Id}) has completed successfully.");
                    
                }
                catch (Exception e)
                {
                    _log.Error($"Data privacy task (Id={taskInfo.Id}) failed with error.", e);
                    isSuccess = false;
                    errorMsg = "Unknow error. " + e.Message;
                }

                //update task info
                taskInfo = _gdprTasksService.GetById(taskId);
                taskInfo.StartedAt = startedAt;
                taskInfo.EndedAt = DateTime.UtcNow;
                taskInfo.Success = isSuccess;
                taskInfo.Error = errorMsg;
                taskInfo.Status = GDPRTaskStatus.Complete;
                taskInfo.Progress = 100;
                _gdprTasksService.UpdateTask(taskInfo);
                
            }
            else
            {
                throw new Exception("Invalid data"); //todo: handle 
            }

            _log.Debug("Ending data privacy job.");
        }

        private DataPrivacyParameters CreateParameters(GDPRTask taskInfo)
        {
            var favoriteData = _dataPrivacyFavoriteRepository.GetById(taskInfo.FavoriteId);

            var dpp = new DataPrivacyParameters()
            {
                TaskId = taskInfo.Id,
                SelectedCustomerId = taskInfo.CustomerId,
                SelectedFavoriteId = taskInfo.FavoriteId,
                RetentionPeriod = favoriteData.RetentionPeriod,
                RegisterDateTo = favoriteData.RegisterDateTo,
                RegisterDateFrom = favoriteData.RegisterDateFrom,
                ClosedOnly = favoriteData.ClosedOnly,
                ReplaceEmails  = favoriteData.ReplaceEmails,
                FieldsNames = favoriteData.FieldsNames?.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).ToList(),
                ReplaceDataWith = favoriteData.ReplaceDataWith,
                ReplaceDatesWith = favoriteData.ReplaceDatesWith,
                RemoveCaseAttachments = favoriteData.RemoveCaseAttachments,
                RemoveLogAttachments = favoriteData.RemoveLogAttachments
            };

            return dpp;
        }
    }
}