using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeRepository : INewRepository
    {
        List<ItemOverview> FindOverviewsExcludeSpecified(int customerId, int changeId);
            
        List<ItemOverview> FindOverviews(int customerId);

        List<CustomerChange> GetCustomersChanges(int[] customersIds); 

        void AddChange(NewChange change);

        ChangeEntity GetById(int changeId);

        ChangeEntity FindById(int changeId);

        SearchResult Search(SearchParameters parameters);

        IList<ChangeOverview> GetChanges(int customerId);
        Task<IList<ChangeOverview>> GetChangesAsync(int customerId);

        void DeleteById(int changeId);

        /// <summary>
        /// The get change overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ChangeOverview"/>.
        /// </returns>
        ChangeOverview GetChangeOverview(int id);
    }
}