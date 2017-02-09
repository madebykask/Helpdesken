namespace DH.Helpdesk.Domain.Faq
{
    public class FaqLanguageEntity
    {
        #region Public Properties

        public string Answer { get; set; }

        public string Answer_Internal { get; set; }

        public string FAQQuery { get; set; }

        public int FAQ_Id { get; set; }

        public int Language_Id { get; set; }

        public virtual FaqEntity Faq { get; set; }

        #endregion
    }
}