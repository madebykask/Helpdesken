namespace DH.Helpdesk.Dal.Mappers.Condition
{
    using BusinessData.Models.Condition;
    using DH.Helpdesk.Domain;

    public sealed class ConditionToEntityMapper : IBusinessModelToEntityMapper<ConditionModel, ConditionEntity>
    {
        public void Map(ConditionModel businessModel, ConditionEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            entity.Id = businessModel.Id;
            entity.ConditionType_Id = businessModel.ConditionType_Id;
            entity.GUID = businessModel.GUID;
            entity.Parent_Id = businessModel.Parent_Id;
            entity.Name = businessModel.Name;
            entity.Property_Name = businessModel.Property_Name;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.ChangedByUser_Id = businessModel.ChangedByUser_Id;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.CreatedByUser_Id = businessModel.CreatedByUser_Id;
            entity.Status = businessModel.Status;
            entity.SortOrder = businessModel.SortOrder;
            entity.Values = businessModel.Values;
            entity.Description = businessModel.Description;
            entity.Operator = businessModel.Operator;
            
        }
    }
}