namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.NewChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input.UpdatedChange;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Change;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;

    public interface IChangeRepository : INewRepository
    {
        List<ItemOverviewDto> FindOverviewsExcludeChange(int customerId, int changeId);
            
        List<ItemOverviewDto> FindOverviews(int customerId);

        void AddChange(NewChange change);

        Change FindById(int changeId);

        SearchResultDto SearchOverviews(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            Enums.Changes.ChangeStatus status,
            int selectCount);

        IList<ChangeEntity> GetChanges(int customer);

        void DeleteById(int id);

        void Update(UpdatedChange change);
    }
}