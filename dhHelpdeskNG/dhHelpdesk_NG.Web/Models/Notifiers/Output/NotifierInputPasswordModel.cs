namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    public sealed class NotifierInputPasswordModel : NotifierInputRequiredFieldModel
    {
        public NotifierInputPasswordModel(bool show)
            : base(show)
        {
        }

        public NotifierInputPasswordModel(bool show, string caption, bool hasValue, bool required, int? minLength)
            : base(show, caption, required)
        {
            this.HasValue = hasValue;
            this.MinLength = minLength;
        }

        public int? MinLength { get; private set; }

        public bool HasValue { get; private set; }
    }
}