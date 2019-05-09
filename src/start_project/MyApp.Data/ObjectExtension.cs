namespace MyApp.Data
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;

    public static class ObjectExtension
    {
        public static IEnumerable Append(this IEnumerable first, params object[] second)
        {
            return first.OfType<object>().Concat(second);
        }
        public static IEnumerable<T> Append<T>(this IEnumerable<T> first, params T[] second)
        {
            return first.Concat(second);
        }
        public static IEnumerable Prepend(this IEnumerable first, params object[] second)
        {
            return second.Concat(first.OfType<object>());
        }
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> first, params T[] second)
        {
            return second.Concat(first);
        }

        public static T GetValueWithDefault<T>(this object obj, T defaultValue)
        {
            var value = default(T);

            if (obj == null || obj is DBNull)
            {
                return value;
            }

            if (obj.GetDate() <= DateTime.MinValue)
            {
                return defaultValue;
            }

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static decimal GetDecimal(this object obj)
        {
            return obj.GetValueWithDefault(0M);
        }

        public static bool GetBoolean(this object obj)
        {
            return obj.GetValueWithDefault(false);
        }

        public static string GetString(this object obj)
        {
            return obj.GetValueWithDefault(string.Empty);
        }

        public static int GetInt(this object obj)
        {
            return obj.GetValueWithDefault(0);
        }

        public static DateTime GetDate(this object obj)
        {
            try
            {
                if (obj.GetType() == Type.GetType("System.String"))
                {
                    return DateTime.ParseExact(obj.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
                }

                return obj.GetValueWithDefault(new DateTime(1900, 01, 01));
            }
            catch (Exception)
            {
                return new DateTime(1900, 01, 01);
            }

        }

        public static byte[] GetByte(this object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static string ParameterValueForSql(this SqlParameter sp)
        {
            if (sp == null || sp.Value == null) return "NULL";

            string retval;

            switch (sp.SqlDbType)
            {
                case SqlDbType.Char:
                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.Time:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    retval = "'" + sp.Value.ToString().Replace("'", "''") + "'";
                    break;
                case SqlDbType.Date:
                case SqlDbType.DateTime:
                case SqlDbType.DateTime2:
                case SqlDbType.DateTimeOffset:
                    retval = "'" + ((DateTime)(sp.Value)).ToString("yyyy-MM-dd HH:mm:ss").Replace("'", "''") + "'";
                    break;

                case SqlDbType.Bit:
                    bool bit;
                    bool.TryParse(sp.Value.ToString(), out bit);
                    retval = bit ? "1" : "0";
                    break;

                default:
                    retval = sp.Value.ToString().Replace("'", "''");
                    break;
            }

            return retval;
        }
    }
}
