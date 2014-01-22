namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.ChangeDetailedOverview;

    public sealed class SearchResultDto
    {
        public SearchResultDto(int changesFound, List<ChangeDetailedOverviewDto> changes)
        {
            ChangesFound = changesFound;
            Changes = changes;
        }

        [MinValue(0)]
        public int ChangesFound { get; private set; }

        [NotNull]
        public List<ChangeDetailedOverviewDto> Changes { get; private set; }
    }
}
