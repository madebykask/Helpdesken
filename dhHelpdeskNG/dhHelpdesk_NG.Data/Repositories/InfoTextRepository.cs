namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IInfoTextRepository : IRepository<InfoText>
    {
    }

    public class InfoTextRepository : RepositoryBase<InfoText>, IInfoTextRepository
    {
        public InfoTextRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
