using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MessageChat
{
    public class DbHelper
    {
        private readonly string _connectionString;
        
        public DbHelper(IOptions<ConnectionSetting> conn)
        {
            _connectionString = string.IsNullOrEmpty(conn.Value.DefaultConnection) ? "Data Source=localhost;Initial Catalog=kakurin_webchat;Integrated Security=True" : conn.Value.DefaultConnection;
        }
        public T ExecuteReaderProcedure<T>(string procedureName, List<SqlParameter> parameters, Func<IDataReader, T> func)
        {
            var entity = default(T);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        entity = func(reader);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return entity;
            }
        }
        public IEnumerable<T> ExecuteReaderListProcedure<T>(string procedureName, List<SqlParameter> parameters, Func<IDataReader, T> func)
        {
            var list = new List<T>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                try
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            list.Add(func(reader));
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return list;
            }
        }
        public int ExecuteNonQueryProcedure(string procedureName, List<SqlParameter> parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(procedureName, connection);
                command.CommandType = CommandType.StoredProcedure;
                int res = -1;
                try
                {
                    command.Parameters.AddRange(parameters.ToArray());
                    res =  command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return res;
            }
        }

    }
}

