using MessageChat.DomainModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace MessageChat.DataRepositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DbHelper _dbHelper;
        public MessageRepository(DbHelper helper)
        {
            _dbHelper = helper;
        }
        public async Task<bool> AppendMessageAsync(MessageModel message)
        {
            string sqlExpression = "sp_AppendMessage";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@chat", Value = message.ChatId },
                new SqlParameter { ParameterName = "@user", Value = message.UserId },
                new SqlParameter { ParameterName = "@created", Value = DateTime.Now },
                new SqlParameter { ParameterName = "@text", Value = message.Text }
            };
            return await _dbHelper.ExecuteNonQueryProcedureAsync(sqlExpression, paramList) > 0;
        }

        public async Task<IEnumerable<MessageModel>> GetMessagesAsync(int offset, int count, int chatId)
        {
            string sqlExpression = "sp_GetMessages";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@chat", Value = chatId},
                new SqlParameter { ParameterName = "@offset", Value = offset},
                new SqlParameter { ParameterName = "@count", Value = count}
            };
            return await _dbHelper.ExecuteReaderListProcedureAsync(sqlExpression, paramList, reader =>
            {
                return new MessageModel()
                {
                    UserId = reader.GetString(0),
                    UserName = reader.GetString(1),
                    Text = reader.GetString(2)
                };
            });
        }

        public async Task<IEnumerable<MessageModel>> GetMessagesByUserIdAsync(string userId)
        {
            string sqlExpression = "sp_GetMessagesByUserId";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@user", Value = userId }
            };

            return await _dbHelper.ExecuteReaderListProcedureAsync(sqlExpression, paramList, reader =>
            {
                return new MessageModel()
                {
                    UserId = reader.GetString(0),
                    UserName = reader.GetString(1),
                    Text = reader.GetString(2)
                };
            });
        }
    }
}
