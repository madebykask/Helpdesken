namespace DH.Helpdesk.Dal.Repositories.Servers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.Dal.Dal;

    public interface IServerRepository : INewRepository
    {
        void Add(Server businessModel);

        void Delete(int id);

        void Update(Server businessModel);

        Server FindById(int id);

        List<ServerOverview> FindOverviews(
            int customerId,
            string searchFor);
    }
}