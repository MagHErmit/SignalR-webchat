using MessageChat.DomainModels;
using System;
using System.Data.SqlClient;

namespace MessageChat.DataRepositories
{
    public class AccountRepository : IAccountRepository
    {
        public UserModel GetUser(string userName)
        {
            string connectionString = "Data Source=localhost;Initial Catalog=kakurin_webchat;Integrated Security=True";
            string sqlExpression = "sp_GetUserCreditals";
            using SqlConnection connection = new SqlConnection(connectionString);
            
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
            throw new NotImplementedException();
        }
    }
}
