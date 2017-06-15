using System;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public  class ExtendedCaseDataToBusinessModelMapper : IEntityToBusinessModelMapper<ExtendedCaseDataEntity, ExtendedCaseDataModel>
    {
        public ExtendedCaseDataModel Map(ExtendedCaseDataEntity entity)
        {
            return new ExtendedCaseDataModel
            {
                Id = entity.Id,
                ExtendedCaseGuid = entity.ExtendedCaseGuid,
                ExtendedCaseFormId = entity.ExtendedCaseFormId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn
            };
        }
    }
}