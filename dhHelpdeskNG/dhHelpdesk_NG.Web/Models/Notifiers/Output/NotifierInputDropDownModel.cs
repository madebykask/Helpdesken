namespace DH.Helpdesk.Web.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

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