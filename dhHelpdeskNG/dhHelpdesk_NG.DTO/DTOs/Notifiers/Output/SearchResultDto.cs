namespace dhHelpdesk_NG.DTO.DTOs.Notifiers.Output
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class SearchResultDto
    {
        public SearchResultDto(int notifiersFound, List<NotifierDetailedOverviewDto> notifiers)
        {
            ArgumentsValidator.NotNull(notifiers, "notifiers");

            NotifiersFound = notifiersFound;
            Notifiers = notifiers;
        }

        public List<NotifierDetailedOverviewDto> Notifiers { get; private set; }

        public int NotifiersFound { get; private set; }
    }
}
