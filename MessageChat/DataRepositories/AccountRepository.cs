using MessageChat.DomainModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MessageChat.DataRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbHelper _dbHelper;

        public AccountRepository(DbHelper helper)
        {
            _dbHelper = helper;
        }

        public async Task<UserModel> GetUserAsync(string userName)
        {
            // TODO: Добавить обработку ошибок
            string sqlExpression = "sp_GetUserCreditals";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@name", Value = userName }
            };

            return await _dbHelper.ExecuteReaderProcedureAsync(sqlExpression, paramList, reader => 
            {
                return new UserModel()
                {
                    Id = (string)reader["user_id"],
                    Name = userName,
                    Password = (string)reader["user_password"],
                    Email = (string)reader["user_email"]
                };
            });
        }

        public async Task<bool> RegisterUserAsync(UserModel user)
        {
            string sqlExpression = "sp_RegisterUser";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter{ ParameterName = "@id", Value = user.Id },
                new SqlParameter{ ParameterName = "@name", Value = user.Name },
                new SqlParameter{ ParameterName = "@email", Value = user.Email },
                new SqlParameter{ ParameterName = "@password", Value = user.Password },
                new SqlParameter{ ParameterName = "@created", Value = DateTime.Now }
            };
            return await _dbHelper.ExecuteNonQueryProcedureAsync(sqlExpression, paramList) > 0;
        }
    }
}
