namespace DH.Helpdesk.Common.Extensions.Boolean
{
    public static class BoolExtensions
    {
        public static string ToBoolString(this bool value)
        {
            return value ? "true" : "false";
        }
    }
}