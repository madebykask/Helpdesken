namespace DH.Helpdesk.Dal.Repositories.WorkstationModules.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;
    using DH.Helpdesk.Domain.Servers;
    using DH.Helpdesk.Domain.WorkstationModules;

    public class OperatingSystemRepository : Repository<OperatingSystem>, IOperatingSystemRepository
    {
        public const string ServicePackMask = "service pack";

        public OperatingSystemRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerModule businessModel)
        {
            var entity = new OperatingSystem
                             {
                                 Id = businessModel.Id,
                                 Name = businessModel.Name,
                                 Customer_Id = businessModel.Customer_Id,
                                 CreatedDate = businessModel.CreatedDate,
                                 ChangedDate = businessModel.CreatedDate, // todo
                             };
            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(ComputerModule businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            entity.Name = businessModel.Name;
            entity.ChangedDate = businessModel.ChangedDate;
            entity.Customer_Id = businessModel.Customer_Id;
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var query =
                DbSet.AsNoTracking()
                    .Where(c => c.Customer_Id == customerId || c.Customer_Id == null);

            return query.Select(c => new ItemOverview
            {
                Name = c.Name,
                Value = c.Id.ToString()
            }).ToList();
        }

        public List<ReportModel> FindConnectedToComputerOperatingSystemOverviews(
            int customerId,
            int? departmentId,
            string searchFor)
        {
            var query = this.DbContext.Computers.Where(x => !x.OS.Name.ToLower().Contains(ServicePackMask));
            var models = FindConnectedToComputerOverviews(query, customerId, departmentId, searchFor);

            return models;
        }

        public List<ReportModel> FindConnectedToComputerServicePackOverviews(
            int customerId,
            int? departmentId,
            string searchFor)
        {
            var query = this.DbContext.Computers.Where(x => x.OS.Name.ToLower().Contains(ServicePackMask));
            var models = FindConnectedToComputerOverviews(query, customerId, departmentId, searchFor);

            return models;
        }

        public List<ReportModel> FindConnectedToServerOperatingSystemOverviews(int customerId, string searchFor)
        {
            var query = this.DbContext.Servers.Where(x => !x.OperatingSystem.Name.ToLower().Contains(ServicePackMask));
            var models = FindConnectedToServerOverviews(query, customerId, searchFor);

            return models;
        }

        public List<ReportModel> FindConnectedToServerServicePackOverviews(int customerId, string searchFor)
        {
            var query = this.DbContext.Servers.Where(x => x.OperatingSystem.Name.ToLower().Contains(ServicePackMask));
            var models = FindConnectedToServerOverviews(query, customerId, searchFor);

            return models;
        }

        private static List<ReportModel> FindConnectedToComputerOverviews(
            IQueryable<Computer> queryable,
            int customerId,
            int? departmentId,
            string searchFor)
        {
            var query = queryable.Where(x => x.Customer_Id == customerId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.OS.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => x.OS_Id != null).Select(x => new { Item = x.OS.Name, Owner = x.ComputerName, ComputerId = x.Id }).ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner, x.ComputerId)).ToList();

            return models;
        }

        private static List<ReportModel> FindConnectedToServerOverviews(
            IQueryable<Server> queryable,
            int customerId,
            string searchFor)
        {
            var query = queryable.Where(x => x.Customer_Id == customerId);

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.OperatingSystem.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => x.OperatingSystem_Id != null)
                    .Select(x => new { Item = x.OperatingSystem.Name, Owner = x.ServerName, ServerId = x.Id })
                    .ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner, x.ServerId)).ToList();

            return models;
        }
    }
}
