namespace DH.Helpdesk.SelfService.Models.Notifiers
{
    using DH.Helpdesk.SelfService.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class SearchDropDownModel
    {
        public SearchDropDownModel(bool show)
        {
            this.Show = show;
        }

        public SearchDropDownModel(bool show, DropDownContent content)
        {
            this.Show = show;
            this.Content = content;
        }

        public DropDownContent Content { get; private set; }

        public bool Show { get; private set; }
    }
}