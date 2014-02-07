namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

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
