namespace DH.Helpdesk.Domain.Faq
{
    using global::System;

    public class FaqFileEntity : Entity
    {
        #region Public Properties

        public DateTime CreatedDate { get; set; }

        public virtual FaqEntity FAQ { get; set; }

        public int FAQ_Id { get; set; }

        public string FileName { get; set; }

        #endregion
    }
}