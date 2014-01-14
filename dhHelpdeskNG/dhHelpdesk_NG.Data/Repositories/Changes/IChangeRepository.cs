namespace dhHelpdesk_NG.Data.Repositories.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;
    using dhHelpdesk_NG.Domain.Changes;

    public interface IChangeRepository : IRepository<Change>
    {
        SearchResultDto SearchOverviews(
            int customerId,
            List<int> statusIds,
            List<int> objectIds,
            List<int> ownerIds,
            List<int> processAffectedIds,
            List<int> workingGroupIds,
            List<int> administratorIds,
            string pharse,
            Enums.Changes.ChangeStatus status,
            int selectCount);

        IList<Change> GetChanges(int customer);
    }
}