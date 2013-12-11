using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface IEmploymentTypeRepository : IRepository<EmploymentType>
    {
    }

    public class EmploymentTypeRepository : RepositoryBase<EmploymentType>, IEmploymentTypeRepository
    {
        public EmploymentTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
