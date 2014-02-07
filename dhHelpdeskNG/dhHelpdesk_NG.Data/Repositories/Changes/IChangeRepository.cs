namespace DH.Helpdesk.Dal.Repositories.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.NewChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Input.UpdatedChange;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeRepository : INewRepository
    {
        List<ItemOverviewDto> FindOverviewsExcludeChange(int customerId, int changeId);
            
        List<ItemOverviewDto> FindOverviews(int customerId);

        void AddChange(NewChange change);

        Change FindById(int changeId);

        SearchResultDto SearchOverviews(SearchParameters parameters);

        IList<ChangeEntity> GetChanges(int customer);

        void DeleteById(int id);

        void Update(UpdatedChange change);
    }
}