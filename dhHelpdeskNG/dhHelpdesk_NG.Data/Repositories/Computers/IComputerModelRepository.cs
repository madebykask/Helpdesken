namespace DH.Helpdesk.Dal.Repositories.Computers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Dal;

    public interface IComputerModelRepository : INewRepository
    {
        void Add(NewItem businessModel);

        void Delete(int id);

        void Update(UpdatedItem businessModel);

        List<ItemOverview> FindOverviews();
    }
}