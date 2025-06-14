﻿using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.FileViewLog;
using DH.Helpdesk.Common.Enums.FileViewLog;

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
				FileSource = entity.FileSource,
				User_Id = entity.User_Id,
                UserName = entity.UserName,
				Operation = entity.Operation ?? FileViewLogOperation.Legacy
			};
		}
	}
}
