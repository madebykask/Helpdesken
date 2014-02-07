namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeHistoryRepository : IRepository<ChangeHistoryEntity>
    {
        void DeleteByChangeId(int changeId);

        List<int> FindIdsByChangeId(int changeId);

        List<History> FindByChangeId(int changeId);
    }
}
