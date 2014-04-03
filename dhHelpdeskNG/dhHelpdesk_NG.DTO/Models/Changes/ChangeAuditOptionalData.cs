namespace DH.Helpdesk.BusinessData.Models.Changes
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ChangeAuditOptionalData
    {
        public ChangeAuditOptionalData(int historyId, Change existingChange)
        {
            this.HistoryId = historyId;
            this.ExistingChange = existingChange;
        }

        [IsId]
        public int HistoryId { get; private set; }

        public Change ExistingChange { get; private set; }
    }
}