namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchResult
    {
        public SearchResult(int notifiersFound, List<NotifierDetailedOverviewDto> notifiers)
        {
            this.NotifiersFound = notifiersFound;
            this.Notifiers = notifiers;
        }

        [NotNull]
        public List<NotifierDetailedOverviewDto> Notifiers { get; private set; }

        public int NotifiersFound { get; private set; }
    }
}
