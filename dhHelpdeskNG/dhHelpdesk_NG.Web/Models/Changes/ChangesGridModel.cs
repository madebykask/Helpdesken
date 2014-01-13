namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class ChangesGridModel
    {
        public ChangesGridModel(int changesFound, List<GridColumnHeaderModel> headers, List<ChangeOverviewModel> changes)
        {
            this.ChangesFound = changesFound;
            this.Headers = headers;
            this.Changes = changes;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<ChangeOverviewModel> Changes { get; set; }

        [MinValue(0)]
        public int ChangesFound { get; set; }
    }
}