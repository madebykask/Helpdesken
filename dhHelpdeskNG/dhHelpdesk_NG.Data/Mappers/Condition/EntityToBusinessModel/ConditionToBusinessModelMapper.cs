namespace DH.Helpdesk.Dal.Mappers.Condition
{
    using BusinessData.Models.Condition;    
    using DH.Helpdesk.Domain;

    public  class ConditionToBusinessModelMapper : IEntityToBusinessModelMapper<ConditionEntity, ConditionModel>
    {
        public ConditionModel Map(ConditionEntity entity)
        {
            return new ConditionModel
            {
                Id = entity.Id,
                Property_Name = entity.Property_Name,
                GUID = entity.GUID,
                ChangedDate = entity.ChangedDate,
                Parent_Id = entity.Parent_Id,
                CreatedDate = entity.CreatedDate,
                ChangedByUser_Id = entity.ChangedByUser_Id,
                CreatedByUser_Id = entity.CreatedByUser_Id,
                Status = entity.Status,
                Values = entity.Values,
                Operator = entity.Operator,
                SortOrder = entity.SortOrder,
                ConditionType_Id = entity.ConditionType_Id,
                Description = entity.Description,
                Name = entity.Name
        
            };
        }
    }
}