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
            entity.CaseField_Name = businessModel.CaseField_Name;
            entity.CaseSolutionConditionGUID = businessModel.CaseSolutionConditionGUID;
            entity.CaseSolution_Id = businessModel.CaseSolution_Id;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.Sequence = businessModel.Sequence;
            entity.Status = businessModel.Status;
            entity.Values = businessModel.Values;
        }
    }
}