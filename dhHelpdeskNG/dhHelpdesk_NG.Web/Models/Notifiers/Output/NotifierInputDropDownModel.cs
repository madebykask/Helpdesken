namespace dhHelpdesk_NG.Web.Models.Notifiers.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class NotifierInputDropDownModel : NotifierInputRequiredFieldModel
    {
        public NotifierInputDropDownModel(bool show) : base(show) 
        {
        }

        public NotifierInputDropDownModel(bool show, string caption, DropDownContent content, bool required)
            : base(show, caption, required)
        {
            this.Content = content;
        }

        [NotNull]
        public DropDownContent Content { get; private set; }
    }
}