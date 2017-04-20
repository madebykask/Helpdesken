using System;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;    
    using DH.Helpdesk.Domain;

    public  class CaseSolutionConditionToBusinessModelMapper : IEntityToBusinessModelMapper<CaseSolutionConditionEntity, CaseSolutionConditionModel>
    {
        public CaseSolutionConditionModel Map(CaseSolutionConditionEntity entity)
        {
            return new CaseSolutionConditionModel
            {
                Id = entity.Id,
                CaseField_Name = entity.CaseField_Name,
                CaseSolutionConditionGUID = entity.CaseSolutionConditionGUID,
                ChangedDate = entity.ChangedDate,
                CaseSolution_Id = entity.CaseSolution_Id,
                CreatedDate = entity.CreatedDate,
                Sequence = entity.Sequence,
                Status = entity.Status,
                Values = entity.Values
            };
        }
    }
}