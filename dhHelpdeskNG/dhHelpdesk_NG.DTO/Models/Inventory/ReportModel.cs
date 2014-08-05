namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportModel
    {
        public ReportModel(string item, string owner, int? ownerId)
        {
            this.Item = item;
            this.Owner = owner;
            this.OwnerId = ownerId;
        }

        [NotNull]
        public string Item { get; set; }

        public string Owner { get; set; }

        public int? OwnerId { get; set; }
    }
}