namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IImpactRepository : IRepository<Impact>
    {
    }

    public class ImpactRepository : RepositoryBase<Impact>, IImpactRepository
    {
        public ImpactRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
