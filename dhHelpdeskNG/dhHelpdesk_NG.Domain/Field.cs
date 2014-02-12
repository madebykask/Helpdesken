namespace DH.Helpdesk.Domain
{
    public class Field : Entity
    {
        public string Key { get; set; }
        public string StringValue { get; set; }
        public bool TranslateThis { get; set; }
    }
}
