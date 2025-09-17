using Microsoft.Data.Sqlite;
using System;

namespace VaxFlow.Data.Repositories
{
    public class SqliteHelper
    {
        public const string DateFormat = "yyyy-MM-dd";
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public static object FromStringNull(string? value)
        {
            return string.IsNullOrEmpty(value) ? DBNull.Value : value;
        }
        public static object FromBoolean(bool value)
        {
            return value ? 1 : 0;
        }
        public static object FromDateTime(DateTime dt)
        {
            return dt.ToString(DateTimeFormat);
        }
        public static object FromDate(DateTime d)
        {
            return d.ToString(DateFormat);
        }

        public static bool GetBoolean(SqliteDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return false;

            int result = reader.GetInt32(ordinal);
            return result == 1;
        }
        public static DateTime? GetDateNull(SqliteDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;

            var dateString = reader.GetString(ordinal);
            return Convert.ToDateTime(dateString);
        }
        public static DateTime? GetDateTimeNull(SqliteDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;

            var dateTimeString = reader.GetString(ordinal);
            return Convert.ToDateTime(dateTimeString);
        }
        public static DateTime GetDate(SqliteDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return DateTime.MinValue;

            var dateString = reader.GetString(ordinal);
            return Convert.ToDateTime(dateString);
        }
        public static DateTime GetDateTime(SqliteDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return DateTime.MinValue;

            var dateTimeString = reader.GetString(ordinal);
            return Convert.ToDateTime(dateTimeString);
        }
        public static string? GetStringNull(SqliteDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;

            return reader.GetString(ordinal);
        }
    }
}
