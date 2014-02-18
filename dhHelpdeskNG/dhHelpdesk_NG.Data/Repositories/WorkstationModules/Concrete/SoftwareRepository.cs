namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class SoftwareRepository : Repository, ISoftwareRepository
    {
        public SoftwareRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<SoftwareOverview> Find(int computerId)
        {
            var anonymus = this.DbContext.Softwares.Where(x => x.Computer_Id == computerId).Select(c => new { c.Id, c.Computer_Id, c.Name, c.Product_key, c.Version, c.Registration_code, c.Manufacturer }).ToList();

            var overviews = anonymus.Select(c => new SoftwareOverview(c.Id, c.Computer_Id, c.Manufacturer, c.Name, c.Product_key, c.Registration_code, c.Version)).ToList();

            return overviews;
        }
    }
}
