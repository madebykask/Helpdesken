using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Web.Infrastructure.Extensions
{
    public static class DicExtentions
    {
        public static T? GetParamValue<T>(this Dictionary<string, object> dic, string paramName) where T : struct
        {
            var typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.Int32:
                    return GetIntValue(dic, paramName) as T?;

                case TypeCode.Int64:
                    return GetIntValue(dic, paramName) as T?;

                case TypeCode.String:
                    return GetStringValue(dic, paramName) as T?;

                case TypeCode.DateTime:
                    return GetStringValue(dic, paramName) as T?;

                case TypeCode.Decimal:
                    return GetStringValue(dic, paramName) as T?;
                               
                default: return null;
            }
        }

        public static T LoadDictionaryToObject<T>(this Dictionary<string, object> dic, T defObject) where T: class
        {                        
            foreach (var propName in dic.Keys)
            {
                var objProperty = defObject.GetType().GetProperty(propName);
               
                if (objProperty != null)
                {
                    var type = objProperty.PropertyType;
                    var typeCode = Type.GetTypeCode(type);

                    switch (typeCode)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                            var intValue = GetIntValue(dic, propName);
                            objProperty.SetValue(defObject, intValue);
                            break;

                        case TypeCode.String:
                            var strValue = GetStringValue(dic, propName);
                            objProperty.SetValue(defObject, strValue);
                            break;

                        case TypeCode.DateTime:
                            var dateTimeValue = GetDateTimeValue(dic, propName);
                            objProperty.SetValue(defObject, dateTimeValue);
                            break;

                        case TypeCode.Decimal:
                            var decimalValue = GetDecimalValue(dic, propName);
                            objProperty.SetValue(defObject, decimalValue);
                            break;

                        case TypeCode.Object:
                            if (type == typeof(int?))
                            {
                                var nullableIntValue = GetIntValue(dic, propName);
                                objProperty.SetValue(defObject, nullableIntValue);
                            }
                            else
                            if (type == typeof(DateTime?))
                            {
                                var nullableDateTimeValue = GetDateTimeValue(dic, propName);
                                objProperty.SetValue(defObject, nullableDateTimeValue);
                            }
                            else
                            if (type == typeof(Guid))
                            {
                                var guidValue = GetGuidValue(dic, propName);
                                objProperty.SetValue(defObject, guidValue);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            return defObject as T;
        }

        private static int? GetIntValue(Dictionary<string, object> dic, string paramName)
        {
            if (!dic.Any() || !dic.ContainsKey(paramName))
                return null;

            var objValue = dic[paramName];
            if (objValue == null)
                return null;

            var strValue = objValue.ToString();  
            int intValue = 0;
            if (int.TryParse(strValue, out intValue))
                return intValue;

            return null;
        }

        private static decimal? GetDecimalValue(Dictionary<string, object> dic, string paramName)
        {
            if (!dic.Any() || !dic.ContainsKey(paramName))
                return null;

            var objValue = dic[paramName];
            if (objValue == null)
                return null;

            var strValue = objValue.ToString();
            decimal decimalValue = 0;
            if (decimal.TryParse(strValue, out decimalValue))
                return decimalValue;

            return null;
        }

        private static string GetStringValue(Dictionary<string, object> dic, string paramName)
        {
            if (!dic.Any() || !dic.ContainsKey(paramName))
                return null;

            return dic[paramName].ToString();
        }

        private static DateTime? GetDateTimeValue(Dictionary<string, object> dic, string paramName)
        {
           if (!dic.Any() || !dic.ContainsKey(paramName))
                return null;

            var objValue = dic[paramName];
            if (objValue == null)
                return null;

            var strValue = objValue.ToString();
            DateTime dateTimeValue;
            if (DateTime.TryParse(strValue, out dateTimeValue))
                return dateTimeValue;

            return null;            
        }

        private static Guid? GetGuidValue(Dictionary<string, object> dic, string paramName)
        {
            if (!dic.Any() || !dic.ContainsKey(paramName))
                return null;

            var objValue = dic[paramName];
            if (objValue == null)
                return null;

            var strValue = objValue.ToString();
            Guid guidValue;
            if (Guid.TryParse(strValue, out guidValue))
                return guidValue;

            return null;
        }
    }
}