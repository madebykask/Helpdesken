namespace dhHelpdesk_NG.Web.Infrastructure.Session
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class PageFilters
    {
        public PageFilters(string pageName, List<PageFilter> filters)
        {
            ArgumentsValidator.NotNullAndEmpty(pageName, "pageName");
            ArgumentsValidator.NotNull(filters, "filters");

            PageName = pageName;
            Filters = filters;
        }

        public string PageName { get; private set; }

        public List<PageFilter> Filters { get; private set; }
    }
}