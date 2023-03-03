using DH.Helpdesk.BusinessData.Models.User.Interfaces;

namespace DH.Helpdesk.Services.utils
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Domain;

    public static class ServiceUtils
    {

        public static string Left(this string value, int lenght)
        {
            return value.Length > lenght ? value.Substring(0, lenght) : value;    
        }

        public static string RemoveNonNumericValuesFromString(this string value)
        {
            return Regex.Replace(value, "[^0-9]", "");
        }

        public static string getObjectValue(this object myObject, string valueToRetun)
        {
            string ret = string.Empty; 
            Type myType = myObject.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                if (string.Compare(valueToRetun, prop.Name, true) == 0)
                {
                    try
                    {
                        ret = prop.GetValue(myObject, null).ToString();
                        break;
                    }
                    catch
                    {
                        ret = string.Empty;
                        break;
                    }
                }
            }

            return ret;
        }

        public static string departmentDescription(this Department d, int departmentFilterFormat)
        {
            var ret = string.Empty;
            if (d != null)
            {
                var departmentInfoAdapter = new DepartmentInfoAdapter(d);
                ret = BuildDepartmentDescription(departmentInfoAdapter, departmentFilterFormat);

            }
            return ret;
        }

        public static string departmentDescription(this IDepartmentInfo d, int departmentFilterFormat)
        {
            var ret = string.Empty;
            if (d != null)
            {
                ret = BuildDepartmentDescription(d, departmentFilterFormat);
            }

            return ret;
        }

        private static string BuildDepartmentDescription(IDepartmentInfo info, int departmentFilterFormat)
        {
            var ret = string.Empty;
            var sep = " - ";

            //if (departmentFilterFormat == 0)
            //    ret = d.DepartmentId;

            if (departmentFilterFormat == 1)
            {
                // anpassning för Ikea IMS
                ret = info.DepartmentName;
                ret = ret.concatStrings(info.DepartmentId, sep);
                ret = ret.concatStrings(info.SearchKey, sep);
                if (!string.IsNullOrEmpty(info.CountryName))
                    ret = ret.concatStrings(info.CountryName, sep);
            }
            else
            {
                ret = info.DepartmentName;
            }
            
            return ret;
        }
    }
}
