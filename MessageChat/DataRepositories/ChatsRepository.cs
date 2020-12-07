using MessageChat.DataRepositories.Inerfaces;
using MessageChat.DomainModels;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    }
}
