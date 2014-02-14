namespace DH.Helpdesk.Web.Models.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class HistoriesModel
    {
        public HistoriesModel(List<HistoryModel> histories)
        {
            this.Histories = histories;
        }

        [NotNull]
        public List<HistoryModel> Histories { get; private set; }
    }
}