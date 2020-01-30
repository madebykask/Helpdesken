namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain.ExtendedCaseEntity;

    public sealed class ExtendedCaseFormForCaseToEntityMapper : IBusinessModelToEntityMapper<ExtendedCaseFormForCaseModel, ExtendedCaseFormEntity>
    {
        public void Map(ExtendedCaseFormForCaseModel businessModel, ExtendedCaseFormEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.Name = businessModel.Name;
            entity.Version = businessModel.Version;
        }
    }
}