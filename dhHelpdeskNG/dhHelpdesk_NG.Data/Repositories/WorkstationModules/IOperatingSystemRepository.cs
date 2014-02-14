namespace DH.Helpdesk.Dal.Repositories.WorkstationModules
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.Dal.Dal;

    public interface IOperatingSystemRepository : INewRepository
    {
        void Add(NewItem businessModel);

        void Delete(int id);

        void Update(UpdatedItem businessModel);

        List<ItemOverview> FindOverviews();
    }
}