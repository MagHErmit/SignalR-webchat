using MessageChat.DomainModels;
using Microsoft.Extensions.Options;
using System;
using System.Data.SqlClient;

namespace MessageChat.DataRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IOptions<ConnectionSetting> conn)
        {
            _connectionString = conn.Value.DefaultConnection;
        }
        public UserModel GetUser(string userName)
        {
            // TODO: Добавить обработку ошибок
            string sqlExpression = "sp_GetUserCreditals";
            using SqlConnection connection = new SqlConnection(_connectionString);
            
            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@name",
                Value = userName
            };
            command.Parameters.Add(nameParam);
            var res = command.ExecuteReader();
            res.Read();
            return new UserModel()
            {
                Id = res.GetString(0),
                Name = userName,
                Password = res.GetString(1),
                Email = res.GetString(2)
            };
                
        }

        public bool RegisterUser(UserModel user)
        {
            string sqlExpression = "sp_RegisterUser";
            using SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = user.Id
            };
            command.Parameters.Add(idParam);

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@name",
                Value = user.Name
            };
            command.Parameters.Add(nameParam);

            SqlParameter emailParam = new SqlParameter
            {
                ParameterName = "@email",
                Value = user.Email
            };
            command.Parameters.Add(emailParam);

            SqlParameter passwordParam = new SqlParameter
            {
                ParameterName = "@password",
                Value = user.Password
            };
            command.Parameters.Add(passwordParam);

            SqlParameter createdParam = new SqlParameter
            {
                ParameterName = "@created",
                Value = DateTime.Now
            };

            command.Parameters.Add(createdParam);
            var res = command.ExecuteNonQuery();
            return res > 0 ? true : false;
        }
    }
}
