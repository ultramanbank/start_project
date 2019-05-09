using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace MyApp.Data
{
    public interface IDatabase
    {
        /// <summary>
        /// This class to add your parameter
        /// </summary>
        /// <param name="name">Parameter Name</param>
        /// <param name="value">Value of Parameter</param>
        /// <returns></returns>
        DbParameter AddParameter(string name, object value);

        /// <summary>
        /// Return row of datasource(SqlDataReader, OracleDataReader)
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        DbDataReader DataReader(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Return DataSet
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        DataSet DataSet(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Return DataTable
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        DataTable DataTable(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        object SingleValue(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number
        //  of rows affected.
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        bool ExecuteNonQuery(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number
        //  of scope identity
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        int ExecuteNonQueryReturnIdentity(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Return row of datasource(SqlDataReader, OracleDataReader) with asynchronous
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        Task<DbDataReader> DataReaderAsync(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Return DataSet with asynchronous
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        Task<DataSet> DataSetAsync(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Return DataTable with asynchronous
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        Task<DataTable> DataTableAsync(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Get object single value with asynchronous
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        Task<object> SingleValueAsync(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number
        //  of rows affected with asynchronous
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        Task<bool> ExecuteNonQueryAsync(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the number
        //  of scope identity with asynchronous
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        Task<int> ExecuteNonQueryReturnIdentityAsync(string sqlCommand, CommandType type, DbParameter[] parameters);

        /// <summary>
        /// Executes a Transact-SQL statement against the connection and returns the string type varchar
        //  of scope identity with asynchronous
        /// </summary>
        /// <param name="sqlCommand">Query or Store name</param>
        /// <param name="type">Text, Store Procedure</param>
        /// <param name="parameters">Array of Parameter</param>
        /// <returns></returns>
        Task<string> ExecuteNonQueryReturnStringAsync(string sqlCommand, CommandType type, DbParameter[] parameters);
    }
}
