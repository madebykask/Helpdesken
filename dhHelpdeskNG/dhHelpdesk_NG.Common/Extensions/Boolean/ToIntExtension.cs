namespace DH.Helpdesk.Common.Extensions.Boolean
{
    public static class ToIntExtension
    {
        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }

    public static class ToNullableIntExtension
    {
        public static int? ToNullableInt(this bool? value)
        {
            if (value == null)
            {
                return null;
            }

            return value.Value.ToInt();
        }
    }
}
