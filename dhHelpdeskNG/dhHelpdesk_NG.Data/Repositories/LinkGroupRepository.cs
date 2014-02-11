namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface ILinkGroupRepository : IRepository<LinkGroup>
    {
    }

    public class LinkGroupRepository : RepositoryBase<LinkGroup>, ILinkGroupRepository
    {
        public LinkGroupRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
