using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain.ExtendedCaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
	public class ExtendedCaseFormToBusinessModelMapper : IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel>
	{
		public ExtendedCaseFormModel Map(ExtendedCaseFormEntity entity)
		{
			return new ExtendedCaseFormModel
			{
				Id = entity.Id,
				Name = entity.Name
			};
		}
	}
}
