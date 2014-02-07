namespace DH.Helpdesk.Domain
{
    public class MailTemplateIdentifier : Entity
    {
        public int SortOrder { get; set; }
        public int Source { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
