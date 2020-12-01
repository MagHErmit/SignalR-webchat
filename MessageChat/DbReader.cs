using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MessageChat
{
    public class DbReader
    {
        private readonly string _connectionString;
        
        public DbReader(IOptions<ConnectionSetting> conn)
        {
            _connectionString = string.IsNullOrEmpty(conn.Value.DefaultConnection) ? "Data Source=localhost;Initial Catalog=kakurin_webchat;Integrated Security=True" : conn.Value.DefaultConnection;
        }
        public void ExecuteReaderProcedure(string procedureName, List<SqlParameter> parameters, Func<IDataReader, bool> func)
        {
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
                            func(reader);
                        }
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

    }
}

