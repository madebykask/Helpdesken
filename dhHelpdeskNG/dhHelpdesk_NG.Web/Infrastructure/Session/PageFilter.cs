namespace dhHelpdesk_NG.Web.Infrastructure.Session
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class PageFilter
    {
        public PageFilter(string name, object value)
        {
            ArgumentsValidator.NotNullAndEmpty(name, "name");

            Name = name;
            Value = value;
        }

        public string Name { get; private set; }

        public object Value { get; private set; }
    }
}