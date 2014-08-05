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

    public class ProcessorRepository : Repository<Processor>, IProcessorRepository
    {
        public ProcessorRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerModule businessModel)
        {
            var entity = new Processor
            {
                Id = businessModel.Id,
                Name = businessModel.Name,
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
        }

        public List<ItemOverview> FindOverviews()
        {
            var anonymus =
                this.DbSet
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
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
                query = query.Where(x => x.Processor.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => x.Processor_Id != null)
                    .Select(x => new { Item = x.Processor.Name, Owner = x.ComputerName, ComputerId = x.Id })
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
                query = query.Where(x => x.Processor.Name.ToLower().Contains(pharseInLowerCase));
            }

            var anonymus =
                query.Where(x => x.Processor_Id != null)
                    .Select(x => new { Item = x.Processor.Name, Owner = x.ServerName, ServerId = x.Id })
                    .ToList();

            var models = anonymus.Select(x => new ReportModel(x.Item, x.Owner, x.ServerId)).ToList();

            return models;
        }
    }
}
