namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public class ComputerUserLogRepository : RepositoryBase<ComputerUserLog>, IComputerUserLogRepository
    {
        public ComputerUserLogRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}