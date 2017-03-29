namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using BusinessData.Models.ExtendedCase;
    using DH.Helpdesk.Domain.Cases;

    public sealed class WorkFlowToEntityMapper : IBusinessModelToEntityMapper<WorkFlowModel, WorkFlowEntity>
    {
        public void Map(WorkFlowModel businessModel, WorkFlowEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            entity.Id = businessModel.Id;
            entity.Customer_Id = businessModel.Customer_Id;
            entity.ItemCaption = businessModel.ItemCaption;
            entity.Status = businessModel.IsActive ? 1 : 0;
            entity.CreatedDate = businessModel.CreatedDate;
            entity.ChangedDate = businessModel.ChangedDate;
        }
    }
}