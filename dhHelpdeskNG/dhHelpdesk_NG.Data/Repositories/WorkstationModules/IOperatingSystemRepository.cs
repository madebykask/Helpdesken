namespace DH.Helpdesk.Dal.Repositories.WorkstationModules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;

    public interface IOperatingSystemRepository : INewRepository
    {
        void Add(ComputerModule businessModel);

        void DeleteById(int id);

        void Update(ComputerModule businessModel);

        List<ItemOverview> FindOverviews();
    }
}