namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerLogRepository : INewRepository
    {
        void Add(NewComputerLog businessModel);

        void Delete(int id);

        void DeleteByComputerId(int computerId);

        List<ComputerLogOverview> Find(int computerId);
    }
}