using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.FileViewLog;

namespace DH.Helpdesk.Dal.Mappers.FileViewLog.BusinessModelToEntity
{
	public class FileViewLogToEntityMapper : IBusinessModelToEntityMapper<FileViewLogModel, FileViewLogEntity>
	{
		public void Map(FileViewLogModel businessModel, FileViewLogEntity entity)
		{
			if (entity == null)
			{
				return;
			}

			entity.Id = businessModel.Id;
			entity.Case_Id = businessModel.Case_Id;
			entity.CreatedDate = businessModel.CreatedDate;
			entity.FileName = businessModel.FileName;
			entity.FilePath = businessModel.FilePath;
			entity.FileSource = (int)businessModel.FileSource;
			entity.User_Id = businessModel.User_Id;
			entity.Operation = (int)businessModel.Operation;
		}
	}
}
