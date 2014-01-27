namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class HistoryModel
    {
        public HistoryModel(List<HistoryItemModel> historyItems)
        {
            this.HistoryItems = historyItems;
        }

        [NotNull]
        public List<HistoryItemModel> HistoryItems { get; private set; }
    }
}