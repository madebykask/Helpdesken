namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.UpdatedChange;

    public interface IChangeRepository : IRepository<ChangeEntity>
    {
        DTO.DTOs.Changes.Change.Change FindById(int changeId);

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