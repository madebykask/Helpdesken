namespace DH.Helpdesk.Common.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public static class ReflectionHelper
    {
        public static IList<PropertyInfo> GetInstanceProperties(object source)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public;
            return FindInstanceProperties(source, bindingFlags);
        }

        public static TValue GetInstancePropertyValue<TValue>(object source, string property)
        {
            var propertyInfo = GetInstanceProperty(source, property);
            return (TValue)propertyInfo.GetValue(source, null);
        }

        public static bool InstanceHasProperty(object source, string property)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            return FindInstanceProperties(source, bindingFlags).Any(p => p.Name == property);
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            var body = 
                expression.Body as MemberExpression ?? ((UnaryExpression)expression.Body).Operand as MemberExpression;

            return body?.Member?.Name;
        }

        private static List<PropertyInfo> FindInstanceProperties(object source, BindingFlags flags)
        {
            var type = source.GetType();
            return type.GetProperties(flags).ToList();
        }

        private static PropertyInfo GetInstanceProperty(object source, string property)
        {
            var type = source.GetType();
            return type.GetProperty(property, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        }

        
    }
}