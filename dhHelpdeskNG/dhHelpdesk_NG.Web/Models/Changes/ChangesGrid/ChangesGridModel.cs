namespace DH.Helpdesk.Web.Models.Changes.ChangesGrid
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Models.Shared;

    public sealed class ChangesGridModel
    {
        public ChangesGridModel(
            int changesFound,
            List<GridColumnHeaderModel> headers,
            List<ChangeOverviewModel> changes,
            SortField sortField)
        {
            this.ChangesFound = changesFound;
            this.Headers = headers;
            this.Changes = changes;
            this.SortField = sortField;
        }

        [NotNull]
        public List<GridColumnHeaderModel> Headers { get; set; }

        [NotNull]
        public List<ChangeOverviewModel> Changes { get; set; }

        [MinValue(0)]
        public int ChangesFound { get; set; }

        public SortField SortField { get; set; }
    }
}