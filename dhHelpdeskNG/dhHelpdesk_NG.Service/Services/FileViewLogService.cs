using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Enums.FileViewLogs;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.BusinessData.Models.Shared.Input;
using DH.Helpdesk.Common.Constants;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Enums.FileViewLog;
using DH.Helpdesk.Common.Tools;
using LinqLib.Operators;

namespace DH.Helpdesk.Services.Services
{
	public interface IFileViewLogService
    {
        FileViewLogModel Log(int caseId, string userName, string fileName, string filePath,
            FileViewLogFileSource fileSource, FileViewLogOperation operation);
		FileViewLogModel Log(int caseId, int userId, string fileName, string filePath, FileViewLogFileSource fileSource, FileViewLogOperation operation);
        IList<FileViewLogListItemModel> Find(FileViewLogListFilter filter, string timeZoneId);
    }
	public class FileViewLogService : IFileViewLogService
	{
		private readonly IEntityToBusinessModelMapper<FileViewLogEntity, FileViewLogModel> _entityToModelMapper;
		private readonly IFileViewLogRepository _fileViewLogRepository;
		private readonly IBusinessModelToEntityMapper<FileViewLogModel, FileViewLogEntity> _modelToEntityMapper;

		public FileViewLogService(IFileViewLogRepository fileViewLogRepository,
			IEntityToBusinessModelMapper<FileViewLogEntity, FileViewLogModel> entityToModelMapper,
			IBusinessModelToEntityMapper<FileViewLogModel, FileViewLogEntity> modelToEntityMapper)
		{
			_fileViewLogRepository = fileViewLogRepository;
			_entityToModelMapper = entityToModelMapper;
			_modelToEntityMapper = modelToEntityMapper;

		}

        public FileViewLogModel Log(int caseId, string userName, string fileName, string filePath,
            FileViewLogFileSource fileSource, FileViewLogOperation operation)
        {
            var model = new FileViewLogModel
            {
                Case_Id = caseId,
                User_Id = null,
                UserName = userName,
                FileName = fileName,
                FilePath = filePath,
                FileSource = fileSource,
                Operation = operation,
                CreatedDate = DateTime.UtcNow
            };

            return Log(model);
        }

		public FileViewLogModel Log(int caseId, int userId, string fileName, string filePath, FileViewLogFileSource fileSource, FileViewLogOperation operation)
        {
            var model = new FileViewLogModel
			{
				Case_Id = caseId,
				User_Id = userId,
                UserName = null,
				FileName = fileName,
				FilePath = filePath,
				FileSource = fileSource,
				Operation = operation,
				CreatedDate = DateTime.UtcNow
			};

            return Log(model);
        }

        public IList<FileViewLogListItemModel> Find(FileViewLogListFilter filter, string timeZoneId)
        {
            const int maxAmount = 10000;
            const int defaultAmount = 500;
            if (filter.Sort == null)
                filter.Sort = new SortField(FileViewLogSortFields.Department, SortBy.Ascending);
            if (filter.AmountPerPage > maxAmount)
                filter.AmountPerPage = defaultAmount;
            if (filter.PeriodFrom.HasValue)
                filter.PeriodFrom = filter.PeriodFrom.GetStartOfDay();
            if (filter.PeriodTo.HasValue)
                filter.PeriodTo = filter.PeriodTo.GetEndOfDay();

            var data = _fileViewLogRepository.Find(filter);

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

            if (!string.IsNullOrWhiteSpace(timeZoneId))
            {
                foreach (var item in data)
                    item.Log.CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(item.Log.CreatedDate, userTimeZone);
            }

            return data;
        }

        private FileViewLogModel Log(FileViewLogModel model)
        {
            var entity = new FileViewLogEntity();
            if(!string.IsNullOrEmpty(Path.GetExtension(model.FilePath)))// all uploaded files must have extension
                model.FilePath = Path.GetDirectoryName(model.FilePath);

            _modelToEntityMapper.Map(model, entity);

            _fileViewLogRepository.Add(entity);
            _fileViewLogRepository.Commit();

            return model;
        }
	}
}
