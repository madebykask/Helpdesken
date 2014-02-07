namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
