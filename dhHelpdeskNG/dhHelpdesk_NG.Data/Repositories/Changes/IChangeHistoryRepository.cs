namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;

    public interface IChangeHistoryRepository : IRepository<ChangeHistoryEntity>
    {
        void DeleteByChangeId(int changeId);

        List<int> FindIdsByChangeId(int changeId);
    }
}
