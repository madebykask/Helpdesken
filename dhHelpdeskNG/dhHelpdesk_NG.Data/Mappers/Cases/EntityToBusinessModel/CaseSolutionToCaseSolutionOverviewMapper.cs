using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity;
using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
	public class CaseSolutionToCaseSolutionOverviewMapper : IEntityToBusinessModelMapper<CaseSolution, CaseSolutionOverview>
	{

		public CaseSolutionOverview Map(CaseSolution entity)
		{
			var stateSecondaryMapper = new StateSecondaryToStateSecondaryOverviewMapper();
			var caseSolutionConditionMapper = new CaseSolutionConditionToBusinessModelMapper();

			return new CaseSolutionOverview
			{
				CaseSolutionId = entity.Id,
				CustomerId = entity.Customer_Id,
				Name = entity.Name,
				CategoryId = entity.Category_Id,
				CategoryName = entity.Category?.Name,
				CaseSolutionCategoryId = entity.CaseSolutionCategory_Id,
				CaseSolutionCategoryName = entity.CaseSolutionCategory?.Name,
				Status = entity.Status,
				StateSecondaryId = entity.StateSecondary_Id,
				NextStepState = entity.NextStepState,
				SortOrder = entity.SortOrder,
				ConnectedButton = entity.ConnectedButton,
				WorkingGroupName = entity.WorkingGroup?.WorkingGroupName,
				StateSecondary = entity.StateSecondary_Id.HasValue ? stateSecondaryMapper.Map(entity.StateSecondary) : null,
				Conditions = entity.Conditions != null ? entity.Conditions.Select(caseSolutionConditionMapper.Map).Select(o =>
					new CaseSolutionConditionOverview
					{
						Id = o.Id,
						Property = o.Property_Name,
						Values = o.Values
					}).ToList() : null,
				ShowInsideCase = entity.ShowInsideCase,
				ShowOnCaseOverview = entity.ShowOnCaseOverview,
				WorkingGroupId = entity.WorkingGroup_Id
			};
		}
	}
}
