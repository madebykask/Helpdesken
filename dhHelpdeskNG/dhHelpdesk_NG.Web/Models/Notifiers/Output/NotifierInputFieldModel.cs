namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.Tools;

    public abstract class NotifierInputFieldModel
    {
        #region Constructors and Destructors

        protected NotifierInputFieldModel(bool show)
        {
            this.Show = show;
        }

        protected NotifierInputFieldModel(bool show, string caption)
        {
            ArgumentsValidator.NotNullAndEmpty(caption, "caption");

            this.Show = show;
            this.Caption = caption;
        }

        #endregion

        #region Public Properties

        public string Caption { get; private set; }

        public bool Show { get; private set; }

        #endregion
    }
}