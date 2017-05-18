namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public sealed class CaseSolutionConditionToEntityMapper : IBusinessModelToEntityMapper<CaseSolutionConditionModel, CaseSolutionConditionEntity>
    {
        public void Map(CaseSolutionConditionModel businessModel, CaseSolutionConditionEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.Property_Name = businessModel.Property_Name;
            entity.CaseSolutionConditionGUID = businessModel.CaseSolutionConditionGUID;
            entity.CaseSolution_Id = businessModel.CaseSolution_Id;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUser_Id;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.CreatedByUser_Id = businessModel.CreatedByUser_Id;
            entity.Status = businessModel.Status;
            entity.Values = businessModel.Values;
            entity.Description = businessModel.Description;
        }
    }
}