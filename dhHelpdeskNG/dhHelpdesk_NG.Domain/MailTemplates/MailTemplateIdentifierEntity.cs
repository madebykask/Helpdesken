namespace DH.Helpdesk.Domain.MailTemplates
{
    public class MailTemplateIdentifierEntity : Entity
    {
        #region Public Properties

        public string IdentifierCode { get; set; }

        public string IdentifierName { get; set; }

        public int SortOrder { get; set; }

        public int Source { get; set; }

        #endregion
    }
}