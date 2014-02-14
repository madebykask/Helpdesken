namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class SearchResult
    {
        public SearchResult(int changesFound, List<ChangeDetailedOverview.ChangeDetailedOverview> changes)
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
