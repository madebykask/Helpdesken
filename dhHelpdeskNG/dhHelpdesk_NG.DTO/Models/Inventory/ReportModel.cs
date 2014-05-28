namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ReportModel
    {
        public ReportModel(string item, string owner)
        {
            this.Item = item;
            this.Owner = owner;
        }

        [NotNull]
        public string Item { get; set; }

        public string Owner { get; set; }
    }
}