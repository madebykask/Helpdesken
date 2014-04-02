namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Server;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ServerRepository : Repository<Domain.Servers.Server>, IServerRepository
    {
        public ServerRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(Server businessModel)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Server businessModel)
        {
            throw new System.NotImplementedException();
        }

        public Server FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<ServerOverview> FindOverviews(int customerId, string searchFor)
        {
            throw new System.NotImplementedException();
        }
    }
}