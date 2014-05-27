namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public class SoftwareRepository : Repository<Software>, ISoftwareRepository
    {
        public SoftwareRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<SoftwareOverview> Find(int computerId)
        {
            var anonymus =
                this.DbContext.Softwares.Where(x => x.Computer_Id == computerId)
                    .Select(
                        c =>
                        new
                            {
                                c.Id,
                                c.Computer_Id,
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
                        c.Computer_Id,
                        c.Manufacturer,
                        c.Name,
                        c.Product_key,
                        c.Registration_code,
                        c.Version)).ToList();

            return overviews;
        }

        public List<ReportModel> FindAllComputerSoftware(int customerId, int? departmentId, string searchFor)
        {
            var query = this.DbSet.Where(x => x.Computer.Customer_Id == customerId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Computer.Department_Id == departmentId);
            }

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus = query.Select(x => new { x.Name, x.Computer.ComputerName }).ToList();

            var models = anonymus.Select(x => new ReportModel(x.Name, x.ComputerName)).ToList();

            return models;
        }
    }
}
