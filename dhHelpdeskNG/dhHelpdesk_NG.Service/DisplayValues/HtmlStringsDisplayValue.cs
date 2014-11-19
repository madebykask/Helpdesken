namespace DH.Helpdesk.Services.DisplayValues
{
    public class HtmlStringsDisplayValue : StringsDisplayValue
    {
        public HtmlStringsDisplayValue(string[] value)
            : base(value, "<br/>")
        {
        }

        public static explicit operator HtmlStringsDisplayValue(string[] value)
        {
            var displayValue = new HtmlStringsDisplayValue(value);

            return displayValue;
        }
    }
}