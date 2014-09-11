namespace DH.Helpdesk.Dal.Mappers.ProductArea.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Domain;

    public sealed class ProductAreaToEntityMapper : IBusinessModelToEntityMapper<ProductAreaOverview, ProductArea>
    {
        public void Map(ProductAreaOverview businessModel, ProductArea entity)
        {
            entity.Parent_ProductArea_Id = businessModel.ParentId;
            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description;
            entity.Customer_Id = businessModel.CustomerId;
        }
    }
}