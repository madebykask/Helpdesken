namespace DH.Helpdesk.Services.Services
{
    using System.Collections.ObjectModel;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Services.Requests.Cases;

    public interface ICaseSolutionSettingService
    {
        void AddCaseSolutionSettings(CaseSettingsSolutionAggregate model);

        void UpdateCaseSolutionSettings(CaseSettingsSolutionAggregate model);

        ReadOnlyCollection<CaseSolutionSettingOverview> GetCaseSolutionSettingOverviews(int caseSolutionId);
    }
}