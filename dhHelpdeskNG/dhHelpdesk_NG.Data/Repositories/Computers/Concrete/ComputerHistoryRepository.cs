namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Inventory;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerHistoryRepository : Repository<Domain.Computers.ComputerHistory>, IComputerHistoryRepository
    {
        public ComputerHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void Add(ComputerHistory businessModel)
        {
            var entity = new Domain.Computers.ComputerHistory
                             {
                                 Computer_Id = businessModel.ComputerId,
                                 UserId = businessModel.UserId,
                                 CreatedDate = businessModel.CreatedDate
                             };

            this.DbSet.Add(entity);
        }

        public void DeleteByComputerId(int id)
        {
            var models = this.DbSet.Where(x => x.Computer_Id == id).ToList();
            models.ForEach(x => this.DbSet.Remove(x));
        }
    }
}