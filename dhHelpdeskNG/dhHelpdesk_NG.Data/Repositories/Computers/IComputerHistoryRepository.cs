namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerHistoryRepository : INewRepository
    {
        void Add(ComputerHistory businessModel);

        void DeleteByComputerId(int id);
    }
}