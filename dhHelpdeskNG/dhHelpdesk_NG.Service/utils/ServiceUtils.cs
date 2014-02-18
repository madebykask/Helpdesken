namespace DH.Helpdesk.Services.utils
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using DH.Helpdesk.BusinessData.OldComponents.DH.Helpdesk.BusinessData.Utils;
    using DH.Helpdesk.Domain;

    public static class ServiceUtils
    {

        public static string Left(this string value, int lenght)
        {
            return value.Length > lenght ? value.Substring(0, lenght) : value;    
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
                    ret = prop.GetValue(myObject, null).ToString();
                    break; 
                }
            }

            return ret;
        }

        public static string departmentDescription(this Department d, int departmentFilterFormat)
        {
            string ret = string.Empty;
            string sep = " - ";

            if (d != null)
            {
                //if (departmentFilterFormat == 0)
                //    ret = d.DepartmentId;
                if (departmentFilterFormat == 1)
                {
                    // anpassning för Ikea IMS
                    ret = d.DepartmentName;
                    ret = ret.concatStrings(d.DepartmentId, sep);
                    ret = ret.concatStrings(d.SearchKey, sep);
                    if (d.Country != null)
                        ret = ret.concatStrings(d.Country.Name, sep);
                }
                else
                    ret = d.DepartmentName;

            }
            return ret;
        }

    }
}
