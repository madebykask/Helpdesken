namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Linq;

    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class ChangeCouncilRepository : Repository, IChangeCouncilRepository
    {
        public ChangeCouncilRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByChangeId(int changeId)
        {
            var councils = this.DbContext.ChangeCouncils.Where(c => c.Change_Id == changeId).ToList();
            councils.ForEach(c => this.DbContext.ChangeCouncils.Remove(c));
        }
    }
}