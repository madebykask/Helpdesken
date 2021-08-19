namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory.Input;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public class ComputerModelRepository : Repository<ComputerModel>, IComputerModelRepository
    {
        public ComputerModelRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerModule businessModel)
        {
            var entity = new ComputerModel
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
    }
}