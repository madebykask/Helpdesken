namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Computers;

    public class ComputerUsersBlackListRepository : RepositoryBase<ComputerUsersBlackList>, IComputerUsersBlackListRepository
    {
        public ComputerUsersBlackListRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}