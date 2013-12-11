
namespace dhHelpdesk_NG.Domain
{
    public class Translation : Entity
    {
        public string Name { get; set; }

        public virtual Language Language { get; set; }
        public virtual Text Text { get; set; }
    }
}
