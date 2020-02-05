using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
	public class StateSecondaryToStateSecondaryOverviewMapper : IEntityToBusinessModelMapper<StateSecondary, StateSecondaryOverview>
	{
		public StateSecondaryOverview Map(StateSecondary entity)
		{
			return new StateSecondaryOverview
			{
				Id = entity.Id,
				Name = entity.Name,
				StateSecondaryId = entity.StateSecondaryId
			};
		}
	}
}
