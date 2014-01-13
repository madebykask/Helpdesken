namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public sealed class ChangeRepository : RepositoryBase<Change>, IChangeRepository
    {
        public ChangeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void SearchOverviews(List<int> statusIds, List<int> objectIds, List<int> ownerIds)
        {
            throw new NotImplementedException();
        }

        public IList<Change> GetChanges(int customer)
        {
            return (from w in this.DataContext.Set<Change>()
                    where w.Customer_Id == customer
                    select w).ToList();
        }
    }
}
