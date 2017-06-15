namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public sealed class ExtendedCaseFormToEntityMapper : IBusinessModelToEntityMapper<ExtendedCaseFormModel, ExtendedCaseFormEntity>
    {
        public void Map(ExtendedCaseFormModel businessModel, ExtendedCaseFormEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
        }
    }
}