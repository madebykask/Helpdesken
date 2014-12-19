namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class HistoryModel
    {
        public HistoryModel(List<HistoryRecordModel> historyRecords)
        {
            this.HistoryRecords = historyRecords;
        }

        [NotNull]
        public List<HistoryRecordModel> HistoryRecords { get; set; }
    }
}