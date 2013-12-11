using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{

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
