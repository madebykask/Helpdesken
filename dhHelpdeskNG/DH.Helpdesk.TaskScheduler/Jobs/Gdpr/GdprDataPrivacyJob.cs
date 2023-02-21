using System;
using System.Linq;
using Common.Logging;
using DH.Helpdesk.BusinessData.Models.Gdpr;
using DH.Helpdesk.Dal.Repositories.GDPR;
using DH.Helpdesk.Domain.GDPR;
using DH.Helpdesk.Services.BusinessLogic.Gdpr;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.TaskScheduler.Infrastructure.Configuration;
using Quartz;

namespace DH.Helpdesk.TaskScheduler.Jobs.Gdpr
{
    [DisallowConcurrentExecution]
    internal class GdprDataPrivacyJob : IJob
    {
        public const string DataMapKey = "data";

        private readonly ILog _log = LogManager.GetLogger(typeof(GdprDataPrivacyJob));
        private readonly IGDPRTasksService _gdprTasksService;
        private readonly IGDPRDataPrivacyFavoriteRepository _dataPrivacyFavoriteRepository;
        private readonly IGDPRDataPrivacyProcessor _dataPrivacyProcessor;
        private readonly IGDPRJobSettings _settings;

        #region ctor()

        public GdprDataPrivacyJob(IGDPRTasksService gdprTasksService, 
                                  IGDPRDataPrivacyFavoriteRepository dataPrivacyFavoriteRepository,
                                  IGDPRDataPrivacyProcessor dataPrivacyProcessor,
                                  IGDPRJobSettings settings)
        {
            _gdprTasksService = gdprTasksService;
            _dataPrivacyFavoriteRepository = dataPrivacyFavoriteRepository;
            _dataPrivacyProcessor = dataPrivacyProcessor;
            _settings = settings;
        }

        #endregion

        public void Execute(IJobExecutionContext context)
        {
            var isSuccess = true;
            var errorMsg = string.Empty;

            var jobDataMap = context.MergedJobDataMap;
            if (jobDataMap.ContainsKey(DataMapKey))
            {
                var taskId = Convert.ToInt32(jobDataMap[DataMapKey]);
                var taskInfo = _gdprTasksService.GetById(taskId);

                _log.Debug($"Starting data privacy job. TaskId: {taskId}");

                //run only scheduled tasks 
                if (taskInfo.Status != GDPRTaskStatus.Scheduled)
                {
                    _log.Warn($"Task (Id={taskId}) has already been completed.");
                    return;
                }

                var parameters = CreateParameters(taskInfo);

                //update status before running 
                var startedAt = DateTime.UtcNow;

                _log.Debug($"Set task to running. TaskId: {taskId}");
                _gdprTasksService.UpdateTaskStatus(taskInfo.Id, GDPRTaskStatus.Running);

                try
                {
                    _log.Debug($"Executing data privacy operation. TaskId: {taskId}");
                    _dataPrivacyProcessor.Process(taskInfo.CustomerId, taskInfo.UserId, parameters, _settings.GDPRBatchSize, _settings.GDPRDeletionTimeoutSeconds);
                    _log.Debug($"Data privacy operation has completed successfully. TaskId: {taskId})");
                }
                catch (Exception e)
                {
                    isSuccess = false;
                    var errorGuid = Guid.NewGuid();
                    _log.Error($"Data privacy operation failed with error. ErrorId: {errorGuid}. TaskId: {taskId}.", e);
                    errorMsg = $"ErrorId: {errorGuid}. " + e.Message;
                }

                //update task info
                taskInfo = _gdprTasksService.GetById(taskId);
                taskInfo.StartedAt = startedAt;
                taskInfo.EndedAt = DateTime.UtcNow;
                taskInfo.Success = isSuccess;
                taskInfo.Error = errorMsg;
                taskInfo.Status = GDPRTaskStatus.Complete;

                _gdprTasksService.UpdateTask(taskInfo);

                _log.Debug($"Ending data privacy job. TaskId: {taskId}");
            }
            else
            {
                throw new Exception("Invalid data"); //todo: handle 
            }
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
                FinishedDateTo = favoriteData.FinishedDateTo,
                FinishedDateFrom = favoriteData.FinishedDateFrom,
                ClosedOnly = favoriteData.ClosedOnly,
                ReplaceEmails  = favoriteData.ReplaceEmails,
                FieldsNames = favoriteData.FieldsNames?.Split(new [] {','}, StringSplitOptions.RemoveEmptyEntries).ToList(),
                ReplaceDataWith = favoriteData.ReplaceDataWith,
                ReplaceDatesWith = favoriteData.ReplaceDatesWith,
                RemoveCaseAttachments = favoriteData.RemoveCaseAttachments,
                RemoveLogAttachments = favoriteData.RemoveLogAttachments,
                RemoveFileViewLogs = favoriteData.RemoveFileViewLogs,
                CaseTypes = favoriteData.CaseTypes?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                ProductAreas = favoriteData.ProductAreas?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(),
                GDPRType = favoriteData.GDPRType
            };

            return dpp;
        }
    }
}