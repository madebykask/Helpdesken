namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Filters.Notifiers;
    using dhHelpdesk_NG.Web.Infrastructure.Session;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public interface IIndexModelFactory
    {
        IndexModel Create(
            FieldSettingsDto displaySettings,
            List<ItemOverviewDto> searchDomains,
            List<ItemOverviewDto> searchRegions,
            List<ItemOverviewDto> searchDepartments,
            List<ItemOverviewDto> searchDivisions,
            NotifierFilters predefinedFilters,
            Enums.Show showDefaultValue,
            int recordsOnPageDefaultValue,
            SearchResultDto searchResult,
            List<ItemOverviewDto> languages,
            int selectedLanguageId);
    }
}