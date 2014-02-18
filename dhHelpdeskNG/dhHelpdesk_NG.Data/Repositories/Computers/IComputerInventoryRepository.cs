namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;

    public interface IComputerInventoryRepository
    {
        void Add(NewComputerInventory businessModel);

        void Delete(int id);

        void DeleteByComputerId(int computerId);

        void DeleteByInventoryId(int inventoryId);
    }
}