namespace DH.Helpdesk.Web.Infrastructure.Session
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class PageFilters
    {
        public PageFilters(string pageName, object filters)
        {
            this.PageName = pageName;
            this.Filters = filters;
        }

        [NotNullAndEmpty]
        public string PageName { get; private set; }

        [NotNull]
        public object Filters { get; private set; }
    }
}