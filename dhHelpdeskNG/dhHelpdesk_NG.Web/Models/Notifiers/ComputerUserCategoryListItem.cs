namespace DH.Helpdesk.Web.Models.Notifiers
{
    public class ComputerUserCategoryListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsDefaultInitiator { get; set; }
        public bool IsDefaultRegarding { get; set; }
    }
}