using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
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
