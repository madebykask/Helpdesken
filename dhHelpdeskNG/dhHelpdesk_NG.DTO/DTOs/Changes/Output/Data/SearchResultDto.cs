namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output.Data
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class SearchResultDto
    {
        public SearchResultDto(int changesFound, List<ChangeDetailedOverviewDto> changes)
        {
            Changes = changes;
            ChangesFound = changesFound;
        }

        [MinValue(0)]
        public int ChangesFound { get; private set; }

        [NotNull]
        public List<ChangeDetailedOverviewDto> Changes { get; private set; }
    }
}
