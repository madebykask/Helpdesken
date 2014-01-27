namespace dhHelpdesk_NG.Web.Models.Changes
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class HistoryModel
    {
        public HistoryModel(List<HistoryItemModel> histories)
        {
            this.Histories = histories;
        }

        [NotNull]
        public List<HistoryItemModel> Histories { get; private set; }
    }
}