namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Dal;

    public interface ICaseSolutionSettingRepository : INewRepository
    {
        void Add(IEnumerable<CaseSolutionSettingForInsert> businessModels);

        void Update(IEnumerable<CaseSolutionSettingForUpdate> businessModels);

        ReadOnlyCollection<CaseSolutionSettingOverview> Find(int caseSolutionId);

        void DeleteByCaseSolutionId(int id);
    }
}