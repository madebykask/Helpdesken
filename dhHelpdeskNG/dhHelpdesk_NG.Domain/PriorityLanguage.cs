namespace DH.Helpdesk.Domain
{
    public class PriorityLanguage
    {
        public int Language_Id { get; set; }
        public int Priority_Id { get; set; }
        public string InformUserText { get; set; }

        public virtual Priority Priority { get; set; }
        public virtual Language Language { get; set; }
    }
}
