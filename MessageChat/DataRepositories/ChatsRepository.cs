using MessageChat.DataRepositories.Inerfaces;
using MessageChat.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MessageChat.DataRepositories
{
    public class ChatsRepository : IChatsRepository
    {
        private readonly DbHelper _helper;

        public ChatsRepository(DbHelper helper)
        {
            _helper = helper;
        }

        public async Task<bool> IsUserAlreadyInChatAsync(string userName, int chatId)
        {
            string sqlExpression = "sp_GetUserCreditals";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@name", Value = userName }
            };
            var res = await GetChatsByUserIdAsync(
            (await _helper.ExecuteReaderProcedureAsync(sqlExpression, paramList, reader =>
            {
                return new UserModel()
                {
                    Id = (string)reader["user_id"],
                    Name = userName,
                    Password = (string)reader["user_password"],
                    Email = (string)reader["user_email"]
                };
            })).Id);

            return res.Where(c => c.Id == chatId).Count() > 0;
        }
        public async Task<IEnumerable<ChatModel>> GetChatsByUserIdAsync(string userId)
        {
            string sqlExpression = "sp_GetChatsByUserId";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@user", Value = userId}
            };
            return await _helper.ExecuteReaderListProcedureAsync(sqlExpression, paramList, reader =>
            {
                return new ChatModel()
                {
                    Id = (int)reader["id"],
                    Name = (string)reader["chat_name"],
                    UserCreatorId = (string)reader["user_id_creator"]
                };
            });
        }

        public async Task<int> CreateChatAsync(string name, string userId)
        {
            string sqlExpression = "sp_AddChat";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@name", Value = name },
                new SqlParameter() { ParameterName = "@creator", Value = userId },
                new SqlParameter() { ParameterName = "@created_at", Value = DateTime.Now}
            };
            return (int)await _helper.ExecuteScalarProcedureAsync(sqlExpression, paramList);
        }

        public async Task<bool> AddUserToChatAsync(string userName, int chatId)
        {
            string sqlExpression = "sp_AddUserToChat";
            var paramlist = new List<SqlParameter>()
            {
                new SqlParameter() { ParameterName = "@user", Value = userName },
                new SqlParameter() { ParameterName = "@chat_id", Value = chatId }
            };
            return await _helper.ExecuteNonQueryProcedureAsync(sqlExpression, paramlist) > 0;
        }
    }
}
