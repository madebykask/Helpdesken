using System;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public  class ExtendedCaseFormToBusinessModelMapper : IEntityToBusinessModelMapper<ExtendedCaseFormEntity, ExtendedCaseFormModel>
    {
        public ExtendedCaseFormModel Map(ExtendedCaseFormEntity entity)
        {
            return new ExtendedCaseFormModel
            {
                Id = entity.Id,
            };
        }
    }
}