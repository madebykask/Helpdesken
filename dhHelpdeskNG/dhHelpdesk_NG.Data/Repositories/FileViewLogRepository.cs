using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.Dal.Enums;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Dal.Mappers;

namespace DH.Helpdesk.Dal.Repositories
{
	public interface IFileViewLogRepository : IRepository<FileViewLogEntity>
	{
		IList<FileViewLogModel> Find(string args);
	}
	public class FileViewLogRepository : RepositoryBase<FileViewLogEntity>, IFileViewLogRepository
	{
		private readonly IEntityToBusinessModelMapper<FileViewLogEntity, FileViewLogModel> _entityToModelMapper;
		private readonly IBusinessModelToEntityMapper<FileViewLogModel, FileViewLogEntity> _modelToEntityMapper;

		public FileViewLogRepository(IDatabaseFactory databaseFactory, 
			IEntityToBusinessModelMapper<FileViewLogEntity, FileViewLogModel> entityToModelMapper,
			IBusinessModelToEntityMapper<FileViewLogModel, FileViewLogEntity> modelToEntityMapper)
            : base(databaseFactory)
        {
			_entityToModelMapper = entityToModelMapper;
			_modelToEntityMapper = modelToEntityMapper;
		}

		public IList<FileViewLogModel> Find(string args)
		{
			throw new NotImplementedException();
		}
	}
}
