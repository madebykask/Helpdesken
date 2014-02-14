namespace DH.Helpdesk.Common.Extensions.Boolean
{
    public static class ToIntExtension
    {
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}
