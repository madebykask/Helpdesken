namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;

    public interface IComputerInventoryRepository
    {
        void Add(ComputerInventory businessModel);

        void DeleteById(int id);

        void DeleteById(int computerId, int inventoryId);

        void DeleteByComputerId(int computerId);

        void DeleteByInventoryId(int inventoryId);
    }
}