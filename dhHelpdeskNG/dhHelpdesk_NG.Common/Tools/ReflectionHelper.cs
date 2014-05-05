namespace DH.Helpdesk.Common.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionHelper
    {
        public static TValue GetInstancePropertyValue<TValue>(object source, string property)
        {
            var propertyInfo = GetInstanceProperty(source, property);
            return (TValue)propertyInfo.GetValue(source, null);
        }

        public static bool InstanceHasProperty(object source, string property)
        {
            return FindInstanceProperties(source).Any(p => p.Name == property);
        }

        private static List<PropertyInfo> FindInstanceProperties(object source)
        {
            var type = source.GetType();
            return type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).ToList();
        }

        private static PropertyInfo GetInstanceProperty(object source, string property)
        {
            var type = source.GetType();
            return type.GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }
    }
}