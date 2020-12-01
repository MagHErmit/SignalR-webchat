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
        private string _userName;
        private UserModel _userModel;

        private bool userRead(IDataReader res)
        {
            _userModel = new UserModel()
            {
                Id = res.GetString(0),
                Name = _userName,
                Password = res.GetString(1),
                Email = res.GetString(2)
            };
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
            var paramList = new List<SqlParameter>();
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@name",
                Value = userName
            };
            paramList.Add(nameParam);
            _userName = userName;
            _dbHelper.ExecuteReaderProcedure(sqlExpression, paramList, userRead);
            return _userModel;
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
            paramList.Add(createdParam);

            var res = _dbHelper.ExecuteNonQueryProcedure(sqlExpression, paramList);
            return res > 0 ? true : false;
        }
    }
}
