using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.LicenseReporter.Helpers
{
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
            return !reader.IsDBNull(reader.GetOrdinal(colName)) ? reader.GetInt32(reader.GetOrdinal(colName)) : 0;
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
            return !reader.IsDBNull(reader.GetOrdinal(colName)) ? reader.GetDecimal(reader.GetOrdinal(colName)) : decimal.Zero;
        }

        public static bool SafeGetBoolean(this IDataReader reader, string colName)
        {
            return !reader.IsDBNull(reader.GetOrdinal(colName)) && reader.GetBoolean(reader.GetOrdinal(colName));
        }

        public static DateTime? SafeGetNullableDateTime(this IDataReader reader, string colName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                return reader.GetDateTime(reader.GetOrdinal(colName));
            return null;
        }

        public static DateTime SafeGetDateTime(this IDataReader reader, string colName)
        {
            return !reader.IsDBNull(reader.GetOrdinal(colName)) ? reader.GetDateTime(reader.GetOrdinal(colName)) : DateTime.MinValue;
        }

        public static string SafeGetString(this IDataReader reader, string colName)
        {
            return !reader.IsDBNull(reader.GetOrdinal(colName)) ? reader[colName].ToString() : string.Empty;
        }

        public static char? SafeGetNullableChar(this IDataReader reader, string colName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                return reader.GetChar(reader.GetOrdinal(colName));
            return null;
        }

        public static int SafeGetIntFromDecimal(this IDataReader reader, string colName)
        {
            return !reader.IsDBNull(reader.GetOrdinal(colName)) ? decimal.ToInt32(reader.GetDecimal(reader.GetOrdinal(colName))) : 0;
        }

        public static int? SafeGetNullableIntFromDecimal(this IDataReader reader, string colName)
        {
            if (!reader.IsDBNull(reader.GetOrdinal(colName)))
                return decimal.ToInt32(reader.GetDecimal(reader.GetOrdinal(colName)));
            return null;
        }

        public static Guid SafeGetGuid(this IDataReader reader, string colName)
        {
            return !reader.IsDBNull(reader.GetOrdinal(colName)) ? new Guid(reader[colName].ToString()) : Guid.Empty;
        }

        public static Guid? SafeGetNullableGuid(this IDataReader reader, string colName)
        {
            int columnIndex = reader.GetOrdinal(colName);
            if (!reader.IsDBNull(columnIndex))
            {
                return new Guid(reader[colName].ToString());
            }
            return null;
        }

    }
}
