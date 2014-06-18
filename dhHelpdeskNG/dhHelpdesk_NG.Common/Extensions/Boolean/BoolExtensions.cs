namespace DH.Helpdesk.Common.Extensions.Boolean
{
    public static class BoolExtensions
    {
        public static string ToYesNoString(this bool value)
        {
            return value ? "yes" : "no";
        }
    }
}