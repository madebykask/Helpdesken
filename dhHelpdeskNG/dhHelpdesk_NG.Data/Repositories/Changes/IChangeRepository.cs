namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface IChangeRepository : IRepository<Change>
    {
        void SearchOverviews(List<int> statusIds, List<int> objectIds, List<int> ownerIds);

        IList<Change> GetChanges(int customer);
    }
}