using DH.Helpdesk.BusinessData.Models.FilewViewLog;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Dal.Repositories;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.Services
{
	public interface IFileViewLogService
	{
		FileViewLogModel Log(int caseId, int userId, string fileName, string filePath, FileViewLogFileSource fileSource, FileViewLogOperation operation);
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
		public FileViewLogModel Log(int caseId, int userId, string fileName, string filePath, FileViewLogFileSource fileSource, FileViewLogOperation operation)
		{
			var model = new FileViewLogModel
			{
				Case_Id = caseId,
				User_Id = userId,
				FileName = fileName,
				FilePath = filePath,
				FileSource = fileSource,
				Operation = operation,
				CreatedDate = DateTime.UtcNow
			};

			var entity = new FileViewLogEntity();

			_modelToEntityMapper.Map(model, entity);

			_fileViewLogRepository.Add(entity);
			_fileViewLogRepository.Commit();

			return model;
		}
	}
}
