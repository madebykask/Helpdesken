namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerTypeRepository : Repository<Domain.Computers.ComputerType>, IComputerTypeRepository
    {
        public ComputerTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerModule businessModel, int customerId)
        {
            var entity = new Domain.Computers.ComputerType
            {
                Id = businessModel.Id,
                Name = businessModel.Name,
                CreatedDate = businessModel.CreatedDate,
                ChangedDate = businessModel.CreatedDate, // todo
                ComputerTypeDescription = string.Empty,
                Customer_Id = customerId
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

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var anonymus =
                this.DbSet
                    .Select(c => new { c.Name, c.Id })
                    .ToList();

            var overviews =
                anonymus.Select(c => new ItemOverview(c.Name, c.Id.ToString(CultureInfo.InvariantCulture))).ToList();

            return overviews;
        }
    }
}