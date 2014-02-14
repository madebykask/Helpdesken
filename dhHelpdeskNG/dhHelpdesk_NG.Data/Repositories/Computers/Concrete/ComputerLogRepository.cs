namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerLogRepository : Repository, IComputerLogRepository
    {
        public ComputerLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}