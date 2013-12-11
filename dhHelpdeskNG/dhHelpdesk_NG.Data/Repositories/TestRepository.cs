using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public class TestRepository : RepositoryBase<User>, ITestRepository
    {
        public TestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    public interface ITestRepository : IRepository<User>
    {
        // expandable ....
    }
}
