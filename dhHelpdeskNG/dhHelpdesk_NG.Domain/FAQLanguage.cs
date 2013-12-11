namespace dhHelpdesk_NG.Domain
{
    public class FAQLanguage
    {
        public int FAQ_Id { get; set; }
        public int Language_Id { get; set; }
        public string Answer { get; set; }
        public string Answer_Internal { get; set; }
        public string FAQQuery { get; set; }
    }
}
