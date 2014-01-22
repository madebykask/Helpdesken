namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;

    public sealed class ChangeHistoryRepository : RepositoryBase<ChangeHistoryEntity>, IChangeHistoryRepository
    {
        public ChangeHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByChangeId(int changeId)
        {
            var histories = this.DataContext.ChangeHistories.Where(h => h.Change_Id == changeId).ToList();
            histories.ForEach(h => this.DataContext.ChangeHistories.Remove(h));
        }

        public List<int> FindIdsByChangeId(int changeId)
        {
            return this.DataContext.ChangeHistories.Where(h => h.Change_Id == changeId).Select(h => h.Id).ToList();
        }
    }
}
