using MessageChat.DomainModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MessageChat.DataRepositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DbHelper _dbHelper;

        private bool userRead(IDataReader res)
        {
            
            return true;
        }
        public AccountRepository(DbHelper helper)
        {
            _dbHelper = helper;
        }
        public UserModel GetUser(string userName)
        {
            // TODO: Добавить обработку ошибок
            string sqlExpression = "sp_GetUserCreditals";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@name", Value = userName }
            };

            return _dbHelper.ExecuteReaderProcedure(sqlExpression, paramList, reader => 
            {
                return new UserModel()
                {
                    Id = reader.GetString(0),
                    Name = userName,
                    Password = reader.GetString(1),
                    Email = reader.GetString(2)
                };
            });
        }

        public bool RegisterUser(UserModel user)
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
            var res = _dbHelper.ExecuteNonQueryProcedure(sqlExpression, paramList);
            return res > 0;
        }
    }
}
