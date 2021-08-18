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
    using DH.Helpdesk.Domain.WorkstationModules;

    public class RAMRepository : Repository<RAM>, IRAMRepository
    {
        public RAMRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerModule businessModel)
        {
            var entity = new RAM
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

        public List<ReportModel> FindConnectedToComputerOverviews(int customerId, int? departmentId, string searchFor)
        {
            var query = this.DbContext.Computers.Where(x => x.Customer_Id == customerId);

            if (departmentId.HasValue)
            {
                query = query.Where(x => x.Department_Id == departmentId);
            }

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.RAM.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => x.RAM_ID != null)
                    .Select(x => new { Item = x.RAM.Name, Owner = x.ComputerName, ComputerId = x.Id })
                    .ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner, x.ComputerId)).ToList();

            return models;
        }

        public List<ReportModel> FindConnectedToServerOverviews(int customerId, string searchFor)
        {
            var query = this.DbContext.Servers.Where(x => x.Customer_Id == customerId);

            if (!string.IsNullOrEmpty(searchFor))
            {
                var pharseInLowerCase = searchFor.ToLower();
                query = query.Where(x => x.RAM.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => x.RAM_Id != null).Select(x => new { Item = x.RAM.Name, Owner = x.ServerName, ServerId = x.Id }).ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner, x.ServerId)).ToList();

            return models;
        }
    }
}
