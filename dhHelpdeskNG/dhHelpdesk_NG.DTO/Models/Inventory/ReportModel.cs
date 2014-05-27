namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    public class ReportModel
    {
        public ReportModel(string item, string owner)
        {
            this.Item = item;
            this.Owner = owner;
        }

        public string Item { get; set; }

        public string Owner { get; set; }
    }
}