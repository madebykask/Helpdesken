namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.ExtendedCase;    
    using DH.Helpdesk.Domain.Cases;

    public sealed class WorkFlowToBusinessModelMapper : IEntityToBusinessModelMapper<WorkFlowEntity, WorkFlowModel>
    {
        public WorkFlowModel Map(WorkFlowEntity entity)
        {
            return new WorkFlowModel
            {
                Id = entity.Id,
                Customer_Id = entity.Customer_Id,
                ItemCaption = entity.ItemCaption,
                User_Id = entity.User_Id,
                IsActive = entity.Status != 0,
                CreatedDate = entity.CreatedDate,
                ChangedDate = entity.ChangedDate
            };
        }
    }
}