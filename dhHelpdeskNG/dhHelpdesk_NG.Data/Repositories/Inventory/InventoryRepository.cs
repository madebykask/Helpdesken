namespace DH.Helpdesk.Dal.Repositories.Inventory
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Inventory;

    #region INVENTORY

    public interface IInventoryRepository : IRepository<Inventory>
    {
    }

    public class InventoryRepository : RepositoryBase<Inventory>, IInventoryRepository
    {
        public InventoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region INVENTORYTYPEPROPERTY

    public interface IInventoryTypePropertyRepository : IRepository<InventoryTypeProperty>
    {
    }

    public class InventoryTypePropertyRepository : RepositoryBase<InventoryTypeProperty>, IInventoryTypePropertyRepository
    {
        public InventoryTypePropertyRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region INVENTORYTYPEPROPERTYVALUE

    public interface IInventoryTypePropertyValueRepository : IRepository<InventoryTypePropertyValue>
    {
    }

    public class InventoryTypePropertyValueRepository : RepositoryBase<InventoryTypePropertyValue>, IInventoryTypePropertyValueRepository
    {
        public InventoryTypePropertyValueRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    #region INVENTORYTYPE

    public interface IInventoryTypeRepository : IRepository<InventoryType>
    {
    }

    public class InventoryTypeRepository : RepositoryBase<InventoryType>, IInventoryTypeRepository
    {
        public InventoryTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
