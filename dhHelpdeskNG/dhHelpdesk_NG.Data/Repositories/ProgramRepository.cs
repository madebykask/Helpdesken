namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IProgramRepository : IRepository<Program>
    {
    }

    public class ProgramRepository : RepositoryBase<Program>, IProgramRepository
    {
        public ProgramRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
