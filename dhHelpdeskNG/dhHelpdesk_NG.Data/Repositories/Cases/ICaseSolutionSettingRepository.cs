namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.ObjectModel;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Dal;

    public interface ICaseSolutionSettingRepository : INewRepository
    {
        void Add(ReadOnlyCollection<CaseSolutionSettingForInsert> businessModels);

        void Update(ReadOnlyCollection<CaseSolutionSettingForUpdate> businessModels);

        ReadOnlyCollection<CaseSolutionSettingOverview> Find(int caseSolutionId);

        void DeleteByCaseSolutionId(int id);
    }
}