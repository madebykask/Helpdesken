namespace dhHelpdesk_NG.Web.Infrastructure.Session
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class PageFilters
    {
        public PageFilters(string pageName, object filters)
        {
            PageName = pageName;
            Filters = filters;
        }

        [NotNullAndEmpty]
        public string PageName { get; private set; }

        [NotNull]
        public object Filters { get; private set; }
    }
}