namespace MyApp.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    public class DataHelper
    {
        private readonly IDatabase _db;

        public DataHelper(IDatabase db)
        {
            if (db == null) throw new ArgumentNullException("db");
            _db = db;
        }

        public bool Execute(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            return _db.ExecuteNonQuery(sqlCommand, type, parameters);
        }

        public async Task<bool> ExecuteAsync(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            return await _db.ExecuteNonQueryAsync(sqlCommand, type, parameters);
        }

        public int ExecuteReturnIdentity(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            return _db.ExecuteNonQueryReturnIdentity(sqlCommand, type, parameters);
        }

        public async Task<int> ExecuteReturnIdentityAsync(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            return await _db.ExecuteNonQueryReturnIdentityAsync(sqlCommand, type, parameters);
        }

        public async Task<string> ExecuteReturnStringAsync(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            return await _db.ExecuteNonQueryReturnStringAsync(sqlCommand, type, parameters);
        }

        public IEnumerable<T> GetMany<T>(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            var dataTable = _db.DataTable(sqlCommand, type, parameters);
            if (dataTable.Rows.Count <= 0) yield break;
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                yield return InvokeBinding<T>(dataTable.Rows[i]);
            }
        }

        public IEnumerable<Task<T>> GetManyAsync<T>(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            var dataTable = _db.DataTableAsync(sqlCommand, type, parameters).GetAwaiter().GetResult();

            if (dataTable.Rows.Count <= 0) yield break;

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                yield return InvokeBindingAsync<T>(dataTable.Rows[i]);
            }
        }

        public T InvokeBinding<T>(DataRow row)
        {
            var parameters = new object[] { row };
            var dummy = Mock<T>();
            var method = typeof(T).GetMethod("DataBind", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
               method.Invoke(dummy, parameters);
            }

            return dummy;
        }

        public async Task<T> InvokeBindingAsync<T>(DataRow row)
        {
            Task<T> result = null;
            var parameters = new object[] { row };
            var dummy = Mock<T>();
            var method = typeof(T).GetMethod("DataBind", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                result = (Task<T>)method.Invoke(dummy, parameters);
            }

            return await result;
        }

        private static T Mock<T>()
        {
            return (T)FormatterServices.GetUninitializedObject(typeof(T));
        }


        #region collapse 

        public T GetOne<T>(string sqlCommand, string columnName, CommandType type, params DbParameter[] parameters)
        {
            var result = default(T);
            var dataTable = _db.DataTable(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                result = Binding<T>(dataTable.Rows[0], columnName);
            }

            return result;
        }

        public T GetOne<T>(string sqlCommand, int columnIndex, CommandType type, params DbParameter[] parameters)
        {
            var result = default(T);
            var dataTable = _db.DataTable(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                result = Binding<T>(dataTable.Rows[0]);
            }

            return result;
        }

        public T GetOne<T>(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            var result = default(T);
            var dataTable = _db.DataTable(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                result = InvokeBinding<T>(dataTable.Rows[0]);
            }

            return result;
        }

        public async Task<T> GetOneAsync<T>(string sqlCommand, string columnName, CommandType type, params DbParameter[] parameters)
        {
            var result = default(T);
            var dataTable = await _db.DataTableAsync(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                result = Binding<T>(dataTable.Rows[0], columnName);
            }

            return result;
        }

        public async Task<T> GetOneAsync<T>(string sqlCommand, int columnIndex, CommandType type, params DbParameter[] parameters)
        {
            var result = default(T);
            var dataTable = await _db.DataTableAsync(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                result = Binding<T>(dataTable.Rows[0]);
            }

            return result;
        }

        public async Task<T> GetOneAsync<T>(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            var result = default(T);
            var dataTable = await _db.DataTableAsync(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                result = InvokeBinding<T>(dataTable.Rows[0]);
            }

            return result;
        }

        public bool Exists(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            var dataTable = _db.DataTable(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> ExistsAsync(string sqlCommand, CommandType type, params DbParameter[] parameters)
        {
            var dataTable = await _db.DataTableAsync(sqlCommand, type, parameters);
            if (dataTable.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }

        #endregion


        //public async Task<T> InvokeBindingAsync<T>(DataRow row)
        //{
        //    var parameters = new object[] { row };
        //    var dummy = Mock<T>();
        //    var method = typeof(T).GetMethod("DataBind", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        //    return await Task<T>.Factory.StartNew(() =>
        //    {
        //        if (method != null)
        //        {
        //            method.Invoke(dummy, parameters);
        //        }

        //        return dummy;
        //    });
        //}

        private static T Binding<T>(DataRow row, int index = 0)
        {
            var generic = default(T);

            switch (typeof(T).FullName)
            {
                case "System.String":
                    var stringValue = (string)row[index];
                    return (T)(object)stringValue;

                case "System.Int32":
                    var intValue = (int)row[index];
                    return (T)(object)intValue;

                case "System.Decimal":
                    var decimalValue = (decimal)row[index];
                    return (T)(object)decimalValue;
            }

            return generic;
        }

        public T Binding<T>(DataRow row, string name)
        {
            var generic = default(T);

            switch (typeof(T).FullName)
            {
                case "System.String":
                    var stringValue = (string)row[name];
                    return (T)(object)stringValue;

                case "System.Int32":
                    var intValue = (int)row[name];
                    return (T)(object)intValue;

                case "System.Decimal":
                    var decimalValue = (decimal)row[name];
                    return (T)(object)decimalValue;
            }

            return generic;
        }
    }
}
