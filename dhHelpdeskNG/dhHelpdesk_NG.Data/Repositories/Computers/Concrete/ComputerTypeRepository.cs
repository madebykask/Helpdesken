namespace DH.Helpdesk.Dal.Repositories.Computers.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public class ComputerTypeRepository : Repository, IComputerTypeRepository
    {
        public ComputerTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}