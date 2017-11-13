namespace DH.Helpdesk.BusinessData.OldComponents
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;

    using global::DH.Helpdesk.Domain;

    namespace DH.Helpdesk.BusinessData.Utils
    {
        using global::System.Linq;

        public static class DataReaderHelper
        {
            public static byte[] SafeGetVarBinary(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return (byte[])reader[colName];
                return null;
            }

            public static int SafeGetInteger(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetInt32(reader.GetOrdinal(colName));
                return 0;
            }

            public static string SafeGetIntegerAsYesNo(this IDataReader reader, int col, bool ShowNoAsEmpty = false)
            {
                if (!reader.IsDBNull(col))
                    return reader.GetInt32(col).formatYesNo(ShowNoAsEmpty);
                return string.Empty;
            }

            public static int? SafeGetNullableInteger(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetInt32(reader.GetOrdinal(colName));
                return null;
            }

            public static decimal? SafeGetNullableDecimal(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetDecimal(reader.GetOrdinal(colName));
                return null;
            }

            public static decimal SafeGetDecimal(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetDecimal(reader.GetOrdinal(colName));
                return decimal.Zero;
            }

            public static double? SafeGetNullableDouble(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetDouble(reader.GetOrdinal(colName));
                return null;
            }

            public static double SafeGetDouble(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetDouble(reader.GetOrdinal(colName));
                return 0;
            }

            public static bool SafeGetBoolean(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetBoolean(reader.GetOrdinal(colName));
                return false;
            }

            public static DateTime? SafeGetNullableDateTime(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    if (reader.GetDateTime(reader.GetOrdinal(colName)) != Convert.ToDateTime("1900-01-01"))
                        return reader.GetDateTime(reader.GetOrdinal(colName));

                return null;
            }

            public static DateTime SafeGetDateTime(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetDateTime(reader.GetOrdinal(colName));
                return DateTime.MinValue;
            }

            public static DateTime SafeGetDate(this IDataReader reader, int col)
            {
                if (!reader.IsDBNull(col))
                    return reader.GetDateTime(col);
                return DateTime.MinValue;
            }

            public static string SafeGetFormatedDateTime(this IDataReader reader, int col)
            {
                if (!reader.IsDBNull(col))
                    return reader.GetDateTime(col).formatDate();
                return string.Empty;
            }

            public static string SafeGetString(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader[colName].ToString();
                return string.Empty;
            }

            public static char? SafeGetNullableChar(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return reader.GetChar(reader.GetOrdinal(colName));
                return null;
            }

            public static int SafeGetIntFromDecimal(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return decimal.ToInt32(reader.GetDecimal(reader.GetOrdinal(colName)));
                return 0;
            }

            public static int? SafeGetNullableIntFromDecimal(this IDataReader reader, string colName)
            {
                if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                    return decimal.ToInt32(reader.GetDecimal(reader.GetOrdinal(colName)));
                return null;
            }

            public static string SafeGetDateTimeWithWeek(this IDataReader reader, int col)
            {
                string ret = string.Empty;

                if (!reader.IsDBNull(col))
                {
                    DateTime d = reader.GetDateTime(col);
                    ret = d.Year.ToString() + "/" + d.weekNumber().ToString().PadLeft(2, '0');
                }
                return ret;
            }

            public static string SafeForSqlInject(this string valueToCheck)
            {
                if (!string.IsNullOrWhiteSpace(valueToCheck))
                    return valueToCheck.Replace("'", "''");
                else
                    return string.Empty;
            }
            public static string SafeForSqlInjectForInOperator(this string valueToCheck)
            {
                if (!string.IsNullOrWhiteSpace(valueToCheck))
                {
                    var args = valueToCheck.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    var ret = string.Empty;
                     
                    foreach (var arg in args)
                    {
                        var hasQutationAtStart = false;
                        var isSurroundedByQutaion = false;  
                        var curArg = arg;
                        if (curArg.StartsWith("'"))
                        {
                            hasQutationAtStart = true;
                            curArg = curArg.Substring(1, curArg.Length - 1);
                        }

                        if (curArg.EndsWith("'"))
                        {
                            isSurroundedByQutaion = hasQutationAtStart;
                            curArg = curArg.Substring(0, curArg.Length - 1);
                        }

                        curArg = curArg.Replace("'", "''");
                        if (isSurroundedByQutaion)
                        {
                            if (ret == string.Empty)
                                ret = string.Format("'{0}'", curArg);
                            else
                                ret += string.Format(",'{0}'", curArg);
                        }else
                        {
                            if (ret == string.Empty)
                                ret = string.Format("{0}", curArg);
                            else
                                ret += string.Format(",{0}", curArg);
                        }
                    }
                    return ret;
                }
                else
                    return string.Empty;
            }

            public static string FreeTextSafeForSqlInject(this string valueToCheck)
            {
                if (!string.IsNullOrWhiteSpace(valueToCheck))
                    return valueToCheck.Replace("'", "''");
                else
                    return string.Empty;
            }

            public static int[] ToIds(this string ids)
            {
                if (string.IsNullOrEmpty(ids))
                {
                    return new int[0];
                }

                return ids.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse)
                        .ToArray();
            }

            public static string createDBsearchstring(this string value)
            {
                string ret = string.Empty;
                if (!string.IsNullOrWhiteSpace(value))
                {
                    ret = value.Replace("*", "%").Replace("?", "_");
                }
                if (!ret.Contains("%")) 
                    ret = "%" + ret + "%";
                return ret;
            }

            public static int weekNumber(this global::System.DateTime value)
            {
                return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }

            public static string formatDate(this global::System.DateTime value)
            {
                return value.Year.ToString() + "-" + value.Month.ToString().PadLeft(2, '0') + "-" + value.Day.ToString().PadLeft(2, '0');
            }

            public static string formatYesNo(this int value, bool ShowNoAsEmpty = false)
            {
                string NoValue = "Nej";

                if (ShowNoAsEmpty)
                    NoValue = string.Empty;

                return (value == 1) ? "Ja" : (value == 2) ? "Nej" : NoValue;
            }

            public static ProductArea getProductAreaItem(this int id, IList<ProductArea> list)
            {
                ProductArea ret = null;

                foreach (ProductArea c in list)
                {
                    if (c.Id == id)
                    {
                        ret = c;
                        break;
                    }
                    //else
                    //    if (c.SubProductAreas != null && !listIncludesAllRecords)
                    //        ret = getProductAreaItem(id, c.SubProductAreas.ToList());

                    //if (ret != null)
                    //    break;
                }
                return ret;
            }

            public static string getProductAreaParentPath(this ProductArea o, string separator = " - ")
            {
                string ret = string.Empty;

                if (o.ParentProductArea == null)
                    ret += o.Name;
                else
                    ret += getProductAreaParentPath(o.ParentProductArea, separator) + separator + o.Name;

                return ret;
            }

            public static string GetFinishingCauseParentPath(this FinishingCause fc, string separator = " - ")
            {
                string ret = string.Empty;

                if (fc.ParentFinishingCause == null)
                {
                    ret += fc.Name;
                }
                else
                {
                    ret += GetFinishingCauseParentPath(fc.ParentFinishingCause, separator) + separator + fc.Name;
                }

                if (ret == string.Empty)
                {
                    return "--";
                }

                return ret;
            }

            public static CaseType getCaseTypeItem(this int id, IList<CaseType> list)
            {
                CaseType ret = null;

                foreach (CaseType c in list)
                {
                    if (c.Id == id)
                    {
                        ret = c;
                        break;
                    }
                    //else
                    //    if (c.SubCaseTypes != null && !listIncludesAllRecords)
                    //        ret = getCaseTypeItem(id, c.SubCaseTypes.ToList());

                    //if (ret != null)
                    //    break;
                }
                return ret;
            }

            public static string getCaseTypeParentPath(this CaseType o, string separator = " - ")
            {
                string ret = string.Empty;

                if (o.ParentCaseType == null)
                    ret += o.Name;
                else
                    ret += getCaseTypeParentPath(o.ParentCaseType, separator) + separator + o.Name;

                return ret;
            }

            public static string getOUParentPath(this OU o, string separator = " - ")
            {
                string ret = string.Empty;
                if (o != null)
                {
                    if (o.Parent_OU_Id == null)
                        ret += o.Name;
                    else
                        ret += getOUParentPath(o.Parent, separator) + separator + o.Name;
                }
                return ret;
            }

            public static string getFinishingCauseParentPath(this FinishingCause o, string separator = " - ")
            {
                string ret = string.Empty;

                if (o.ParentFinishingCause == null)
                    ret += o.Name;
                else
                    ret += getFinishingCauseParentPath(o.ParentFinishingCause, separator) + separator + o.Name;

                return ret;
            }

            public static string concatStrings(this string orginal, string valueToAdd, string sep)
            {
                string ret = string.Empty;

                if (!string.IsNullOrWhiteSpace(orginal))
                    ret = orginal;

                if (!string.IsNullOrWhiteSpace(valueToAdd))
                {
                    ret += sep + valueToAdd;
                }

                return ret;
            }

            public static string GetUserFromAdPath(this string path)
            {
                string s = path;
                int stop = s.IndexOf("\\", StringComparison.Ordinal);
                if (stop <= -1)
                {
                    return string.Empty;
                }

                s = s.Substring(stop + 1, s.Length - stop - 1);
                if (s.Length > 20)
                {
                    s = s.Substring(0, 20);
                }

                return s;
            }

            public static string GetDomainFromAdPath(this string path)
            {
                string s = path;
                int stop = s.IndexOf("\\");
                return (stop > -1) ? s.Substring(0, stop) : string.Empty;
            }

        }
    }

}
