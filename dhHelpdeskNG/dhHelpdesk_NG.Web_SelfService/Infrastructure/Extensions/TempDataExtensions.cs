using System;
using System.Web.Mvc;

namespace DH.Helpdesk.SelfService.Infrastructure.Extensions
{
    public static class TempDataExtensions
    {
        public static T GetSafe<T>(this TempDataDictionary tempData, string key)
        {
            if (tempData.ContainsKey(key))
            {
                T val = (T) Convert.ChangeType(tempData[key], typeof(T));
                return val;
            }
            return default(T);
        }
    }
}