namespace MyApp.Data
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Not Implement Log Service
    /// When Error Throw Exception Only.
    /// </summary>
    public class Mssql : IDatabase
    {
        private string _connectionString;

        public Mssql(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbParameter AddParameter(string name, object value)
        {
            SqlParameter p = new SqlParameter();

            try
            {
                p.ParameterName = name.Substring(0, 1).ToString() == "@" ? name : "@" + name;
                p.Value = value;

                return p;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DbDataReader DataReader(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.Parameters.AddRange(parameters);

                        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            return reader;
                        }
                    }

                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<DbDataReader> DataReaderAsync(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.Parameters.AddRange(parameters);

                        using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                        {
                            return reader;
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataSet DataSet(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.Parameters.AddRange(parameters);

                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(ds, "ds");

                            return ds;
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<DataSet> DataSetAsync(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.Parameters.AddRange(parameters);

                        using (var da = new SqlDataAdapter(cmd))
                        {
                            await Task.Run(() => { da.Fill(ds, "ds"); });

                            return ds;
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable DataTable(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.Parameters.AddRange(parameters);

                        using (var da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);

                            return dt;
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<DataTable> DataTableAsync(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.Parameters.AddRange(parameters);

                        using (var da = new SqlDataAdapter(cmd))
                        {
                            await Task.Run(() => { da.Fill(dt); });

                            return dt;
                        }
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public bool ExecuteNonQuery(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            var result = false;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery();

                        result = true;
                    };

                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<bool> ExecuteNonQueryAsync(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddRange(parameters);
                        await cmd.ExecuteNonQueryAsync();

                        return true;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int ExecuteNonQueryReturnIdentity(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            var result = -1;

            try
            {

                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddRange(parameters);
                        cmd.Parameters.Add("@return_id", SqlDbType.Int, 0, "return_id");
                        cmd.Parameters["@return_id"].Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();

                        result = (int)cmd.Parameters["@return_id"].Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<int> ExecuteNonQueryReturnIdentityAsync(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddRange(parameters);
                        cmd.Parameters.Add("@return_id", SqlDbType.Int, 0, "return_id");
                        cmd.Parameters["@return_id"].Direction = ParameterDirection.Output;

                        await cmd.ExecuteNonQueryAsync();

                        return (int)cmd.Parameters["@return_id"].Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public object SingleValue(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            var sw = Stopwatch.StartNew();
            object result = null;

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddRange(parameters);

                        result = cmd.ExecuteScalar();
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return result;
        }

        public async Task<object> SingleValueAsync(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddRange(parameters);

                        return cmd.ExecuteScalarAsync();
                    }
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public async Task<string> ExecuteNonQueryReturnStringAsync(string sqlCommand, CommandType type, DbParameter[] parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = type;
                        cmd.CommandText = sqlCommand;
                        cmd.CommandTimeout = 600;
                        cmd.Parameters.AddRange(parameters);
                        cmd.Parameters.Add("@return_id", SqlDbType.VarChar, 500, "return_id");
                        cmd.Parameters["@return_id"].Direction = ParameterDirection.Output;
                        cmd.Parameters["@return_id"].Size = 500;

                        await cmd.ExecuteNonQueryAsync();

                        return (string)cmd.Parameters["@return_id"].Value;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
