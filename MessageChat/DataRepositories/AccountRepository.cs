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
        private readonly string _connectionString;
        private readonly DbReader _reader;
        private UserModel _um;

        private bool userRead(IDataReader res)
        {
            _um = new UserModel()
            {
                Id = res.GetString(0),
                //Name = userName,
                Password = res.GetString(1),
                Email = res.GetString(2)
            };
            return true;
        }
        public AccountRepository(IOptions<ConnectionSetting> conn, DbReader reader)
        {
            _connectionString = conn.Value.DefaultConnection;
            _reader = reader;
        }
        public UserModel GetUser(string userName)
        {
            // TODO: Добавить обработку ошибок
            string sqlExpression = "sp_GetUserCreditals";
            var paramList = new List<SqlParameter>();
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@name",
                Value = userName
            };
            paramList.Add(nameParam);
            _reader.ExecuteReaderProcedure(sqlExpression, paramList, userRead);
            return _um;
        }

        public bool RegisterUser(UserModel user)
        {
            string sqlExpression = "sp_RegisterUser";
            var paramList = new List<SqlParameter>();

            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@id",
                Value = user.Id
            };
            paramList.Add(idParam);

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@name",
                Value = user.Name
            };
            paramList.Add(nameParam);

            SqlParameter emailParam = new SqlParameter
            {
                ParameterName = "@email",
                Value = user.Email
            };
            paramList.Add(emailParam);

            SqlParameter passwordParam = new SqlParameter
            {
                ParameterName = "@password",
                Value = user.Password
            };
            paramList.Add(passwordParam);

            SqlParameter createdParam = new SqlParameter
            {
                ParameterName = "@created",
                Value = DateTime.Now
            };
            paramList.Add(passwordParam);

            //_reader.ExecuteReaderProcedure()
            return true;// res > 0 ? true : false;
        }
    }
}
