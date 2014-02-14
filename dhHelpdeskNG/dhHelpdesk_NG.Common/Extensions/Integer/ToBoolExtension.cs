namespace DH.Helpdesk.Common.Extensions.Integer
{
    public static class ToBoolExtension
    {
        public static bool ToBool(this int value)
        {
            return value != 0;
        }
    }
}
