namespace dhHelpdesk_NG.Web.Infrastructure.Session
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class PageFilters
    {
        public PageFilters(string pageName, object filters)
        {
            ArgumentsValidator.NotNullAndEmpty(pageName, "pageName");
            ArgumentsValidator.NotNull(filters, "filters");

            PageName = pageName;
            Filters = filters;
        }

        public string PageName { get; private set; }

        public object Filters { get; private set; }
    }
}