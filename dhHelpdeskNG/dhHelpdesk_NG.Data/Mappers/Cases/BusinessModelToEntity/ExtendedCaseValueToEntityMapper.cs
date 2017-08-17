namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public sealed class ExtendedCaseValueToEntityMapper : IBusinessModelToEntityMapper<ExtendedCaseValueModel, ExtendedCaseValueEntity>
    {
        public void Map(ExtendedCaseValueModel businessModel, ExtendedCaseValueEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.ExtendedCaseDataId = businessModel.ExtendedCaseDataId;
            entity.FieldId = businessModel.FieldId;
            entity.Value = businessModel.Value;
            entity.SecondaryValue = businessModel.SecondaryValue;
        }
    }
}