using System;
using Newtonsoft.Json;

namespace DH.Helpdesk.Common.Extensions.Object
{
    public static class ObjectExtension
    {
        public static T Use<T>(this T t, Func<T, T> use)
        {
            return use(t);
        }

        public static T DeepClone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
