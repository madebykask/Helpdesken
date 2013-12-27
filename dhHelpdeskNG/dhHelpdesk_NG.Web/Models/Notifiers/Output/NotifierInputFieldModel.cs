namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public abstract class NotifierInputFieldModel
    {
        #region Constructors and Destructors

        protected NotifierInputFieldModel(bool show)
        {
            this.Show = show;
        }

        protected NotifierInputFieldModel(bool show, string caption)
        {
            this.Show = show;
            this.Caption = caption;
        }

        #endregion

        #region Public Properties

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public bool Show { get; private set; }

        #endregion
    }
}