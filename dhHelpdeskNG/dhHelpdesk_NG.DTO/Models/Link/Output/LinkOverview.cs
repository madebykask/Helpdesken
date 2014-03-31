namespace DH.Helpdesk.BusinessData.Models.Link.Output
{
    public sealed class LinkOverview
    {
        public int? Customer_Id { get; set; }
        public string CustomerName { get; set; }
        public string URLName { get; set; }
        public string URLAddress { get; set; }
        public int? LinkGroup_Id { get; set; }
        public string LinkGroupName { get; set; }
    }
}