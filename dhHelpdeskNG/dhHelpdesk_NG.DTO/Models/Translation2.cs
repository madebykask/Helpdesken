namespace DH.Helpdesk.BusinessData.Models
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class Translation2
    {
        #region Constructors and Destructors

        public Translation2(string textId, string culture, string text)
        {
            this.TextId = textId;
            this.Culture = culture;
            this.Text = text;
        }

        #endregion

        #region Public Properties

        public string Culture { get; private set; }

        public string Text { get; private set; }

        public string TextId { get; private set; }

        #endregion
    }
}