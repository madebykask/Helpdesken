namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerHistoryRepository : Repository, IComputerHistoryRepository
    {
        public ComputerHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}