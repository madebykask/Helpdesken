namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public sealed class ExtendedCaseDataToEntityMapper : IBusinessModelToEntityMapper<ExtendedCaseDataModel, ExtendedCaseDataEntity>
    {
        public void Map(ExtendedCaseDataModel businessModel, ExtendedCaseDataEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.ExtendedCaseGuid = businessModel.ExtendedCaseGuid;
            entity.ExtendedCaseFormId = businessModel.ExtendedCaseFormId;
            entity.CreatedBy = businessModel.CreatedBy;
            entity.CreatedOn = businessModel.CreatedOn;
        }
    }
}