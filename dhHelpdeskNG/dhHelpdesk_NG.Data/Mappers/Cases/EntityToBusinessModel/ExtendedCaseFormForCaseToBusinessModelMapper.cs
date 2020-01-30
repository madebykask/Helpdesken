using System;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public  class ExtendedCaseFormForCaseToBusinessModelMapper : IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormForCaseModel>
    {
        public ExtendedCaseFormForCaseModel Map(ExtendedCaseFormEntity entity)
        {
            if (entity == null)
                return null;

            return new ExtendedCaseFormForCaseModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Version = entity.Version
                            
            };
        }
    }
}