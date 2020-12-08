using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MessageChat
{
    public class DbHelper
    {
        private readonly string _connectionString;
        
        public DbHelper(IOptions<ConnectionSetting> conn)
        {
            _connectionString = string.IsNullOrEmpty(conn.Value.DefaultConnection) ? "Data Source=localhost;Initial Catalog=kakurin_webchat;Integrated Security=True" : conn.Value.DefaultConnection;
        }
        public async Task<T> ExecuteReaderProcedureAsync<T>(string procedureName, List<SqlParameter> parameters, Func<IDataReader, T> func)
        {
            var entity = default(T);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        entity = func(reader);
                    }
                    await reader.CloseAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return entity;
            }
        }
        public async Task<IEnumerable<T>> ExecuteReaderListProcedureAsync<T>(string procedureName, List<SqlParameter> parameters, Func<IDataReader, T> func)
        {
            
            var list = new List<T>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(func(reader));
                        }
                    }
                    await reader.CloseAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
                return list;
            }
        }
        public async Task<int> ExecuteNonQueryProcedureAsync(string procedureName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                int res = -1;
                try
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    res = await command.ExecuteNonQueryAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
                return res;
            }
        }

        public async Task<object> ExecuteScalarProcedureAsync(string procedureName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                object res;
                try
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    res = await command.ExecuteScalarAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
                return res;
            }
        }

    }
}

