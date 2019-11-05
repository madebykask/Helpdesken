using DH.Helpdesk.BusinessData.Models.Inventory.Output.Computer;

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
                ChangedDate = businessModel.CreatedDate, 
                ComputerTypeDescription = businessModel.Description,
                Customer_Id = customerId
            };
            this.DbSet.Add(entity);
            this.InitializeAfterCommit(businessModel, entity);
        }

        public void Update(ComputerModule businessModel)
        {
            var entity = this.DbSet.Find(businessModel.Id);
            entity.Name = businessModel.Name;
            entity.ComputerTypeDescription = businessModel.Description;
            entity.ChangedDate = businessModel.ChangedDate;
        }

        public ComputerTypeOverview Get(int id)
        {
            var item = DbSet.Single(x => x.Id == id);
            return new ComputerTypeOverview(item.Id, item.Name)
            {
                Description = item.ComputerTypeDescription
            };
        }

        public List<ItemOverview> FindOverviews(int customerId)
        {
            var overviews =
                DbSet.AsNoTracking()
                    .Where(c => c.InventoryType_Id == null && c.Customer_Id == customerId)
                    .Select(c => new ItemOverview()
                    {
                        Name = c.Name,
                        Value = c.Id.ToString()
                    })
                    .ToList();

            return overviews;
        }
    }
}