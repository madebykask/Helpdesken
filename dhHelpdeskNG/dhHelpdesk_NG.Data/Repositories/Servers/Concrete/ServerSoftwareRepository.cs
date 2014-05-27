namespace DH.Helpdesk.Dal.Repositories.Servers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Servers;

    public class ServerSoftwareRepository : Repository<ServerSoftware>, IServerSoftwareRepository
    {
        public ServerSoftwareRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<SoftwareOverview> Find(int serverId)
        {
            var anonymus =
                this.DbContext.ServerSoftwares.Where(x => x.Server_Id == serverId)
                    .Select(
                        c =>
                        new
                            {
                                c.Id, 
                                c.Server_Id, 
                                c.Name, 
                                c.Product_key, 
                                c.Version, 
                                c.Registration_code, 
                                c.Manufacturer
                            })
                    .ToList();

            var overviews =
                anonymus.Select(
                    c =>
                    new SoftwareOverview(
                        c.Id,
                        c.Server_Id,
                        c.Manufacturer,
                        c.Name,
                        c.Product_key,
                        c.Registration_code,
                        c.Version)).ToList();

            return overviews;
        }

        public List<ReportModel> FindAllServerSoftware(int customerId, string searchFor)
        {
            var query = this.DbSet.Where(x => x.Server.Customer_Id == customerId);

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus = query.Select(x => new { x.Name, x.Server.ServerName }).ToList();

            var models = anonymus.Select(x => new ReportModel(x.Name, x.ServerName)).ToList();

            return models;
        }
    }
}