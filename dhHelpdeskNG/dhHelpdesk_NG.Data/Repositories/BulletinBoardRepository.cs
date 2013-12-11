using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Data.Repositories
{
    public interface IBulletinBoardRepository : IRepository<BulletinBoard>
    {
    }

    public class BulletinBoardRepository : RepositoryBase<BulletinBoard>, IBulletinBoardRepository
    {
        public BulletinBoardRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
