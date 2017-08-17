using System;

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public  class ExtendedCaseValueToBusinessModelMapper : IEntityToBusinessModelMapper<ExtendedCaseValueEntity, ExtendedCaseValueModel>
    {
        public ExtendedCaseValueModel Map(ExtendedCaseValueEntity entity)
        {
            return new ExtendedCaseValueModel
            {
                Id = entity.Id,
                ExtendedCaseDataId = entity.ExtendedCaseDataId,
                FieldId = entity.FieldId,
                SecondaryValue = entity.SecondaryValue,
                Value = entity.Value
            };
        }
    }
}