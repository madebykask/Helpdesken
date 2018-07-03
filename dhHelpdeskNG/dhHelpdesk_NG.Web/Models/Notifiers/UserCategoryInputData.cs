namespace DH.Helpdesk.Web.Models.Notifiers
{
    public class UserCategoryInputData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsEmpty { get; set; }
    }
}