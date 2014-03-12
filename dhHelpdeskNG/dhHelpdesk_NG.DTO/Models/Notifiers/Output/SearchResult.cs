namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchResult
    {
        public SearchResult(int notifiersFound, List<NotifierDetailedOverview> notifiers)
        {
            this.NotifiersFound = notifiersFound;
            this.Notifiers = notifiers;
        }

        [NotNull]
        public List<NotifierDetailedOverview> Notifiers { get; private set; }

        public int NotifiersFound { get; private set; }
    }
}
