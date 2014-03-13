namespace DH.Helpdesk.Common.Tools
{
    public static class ReflectionHelper
    {
        public static TValue GetPropertyValue<TValue>(object source, string property)
        {
            return (TValue)source.GetType().GetProperty(property).GetValue(source, null);
        }
    }
}