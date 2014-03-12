namespace DH.Helpdesk.Dal.Repositories.Inventory.Concrete
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class InventoryTypeRepository : Repository, IInventoryTypeRepository
    {
        public InventoryTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(InventoryType businessModel)
        {
            var entity = new Domain.Inventory.InventoryType
            {
                Name = businessModel.Name,
                Customer_Id = businessModel.CustomerId,
                CreatedDate = businessModel.CreatedDate
            };

            this.DbContext.InventoryTypes.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Delete(int id)
        {
            var entity = this.DbContext.InventoryTypes.Find(id);
            this.DbContext.InventoryTypes.Remove(entity);
        }

        public void Update(InventoryType businessModel)
        {
            var entity = this.DbContext.InventoryTypes.Find(businessModel.Id);
            entity.Name = businessModel.Name;
            entity.ChangedDate = entity.ChangedDate;
        }

        public InventoryType FindById(int id)
        {
            var entity = this.DbContext.InventoryTypes.Find(id);
            var businessModel = InventoryType.CreateForEdit(
                entity.Name,
                entity.Id,
                entity.CreatedDate,
                entity.ChangedDate);

            return businessModel;
        }
    }
}