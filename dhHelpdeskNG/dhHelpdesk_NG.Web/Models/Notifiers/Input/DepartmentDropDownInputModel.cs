namespace dhHelpdesk_NG.Web.Models.Notifiers.Input
{
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class DepartmentDropDownInputModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DropDownContent Content { get; set; }

        public bool Required { get; set; }
    }
}