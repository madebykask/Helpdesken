using DH.Helpdesk.BusinessData.Models.FilewViewLog;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Mappers.FileViewLog.EntityToBusinessModel
{
	public class FileViewLogToBusinessModelMapper : IEntityToBusinessModelMapper<FileViewLogEntity, FileViewLogModel>
	{
		public FileViewLogModel Map(FileViewLogEntity entity)
		{
			return new FileViewLogModel
			{
				Id = entity.Id,
				Case_Id = entity.Case_Id,
				CreatedDate = entity.CreatedDate,
				FileName = entity.FileName,
				FilePath = entity.FilePath,
				FileSource = (FileViewLogFileSource)entity.FileSource,
				User_Id = entity.User_Id,
				Operation = (FileViewLogOperation)entity.Operation
			};
		}
	}
}
