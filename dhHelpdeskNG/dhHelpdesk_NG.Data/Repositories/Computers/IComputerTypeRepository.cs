using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerTypeRepository : INewRepository
    {
        void Add(ComputerModule businessModel, int customerId);

        void DeleteById(int id);

        void Update(ComputerModule businessModel);

        List<ItemOverview> FindOverviews(int customerId, int? inventoryId);
        ComputerTypeOverview Get(int id);
    }
}