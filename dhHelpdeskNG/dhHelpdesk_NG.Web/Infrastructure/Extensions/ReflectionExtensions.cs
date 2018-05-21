using System;
using System.Linq;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasAttribute(this Type type, Type attributeType)
        {
            var attr = type.GetCustomAttributes(attributeType, false).FirstOrDefault();
            return attr != null;
        }
    }
}