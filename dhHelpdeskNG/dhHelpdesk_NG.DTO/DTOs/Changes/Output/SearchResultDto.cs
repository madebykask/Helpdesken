namespace dhHelpdesk_NG.DTO.DTOs.Changes.Output
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class SearchResultDto
    {
        public SearchResultDto(int changesFound, List<ChangeDetailedOverview.ChangeDetailedOverview> changes)
        {
            this.ChangesFound = changesFound;
            this.Changes = changes;
        }

        [MinValue(0)]
        public int ChangesFound { get; private set; }

        [NotNull]
        public List<ChangeDetailedOverview.ChangeDetailedOverview> Changes { get; private set; }
    }
}
