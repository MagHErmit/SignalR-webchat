using MessageChat.DomainModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MessageChat.DataRepositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DbHelper _dbHelper;
        public MessageRepository(DbHelper helper)
        {
            _dbHelper = helper;
        }
        public bool AppendMessage(MessageModel message)
        {
            string sqlExpression = "sp_AppendMessage";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@chat", Value = message.ChatId },
                new SqlParameter { ParameterName = "@user", Value = message.UserId },
                new SqlParameter { ParameterName = "@created", Value = DateTime.Now },
                new SqlParameter { ParameterName = "@text", Value = message.Text }
            };
            var res = _dbHelper.ExecuteNonQueryProcedure(sqlExpression, paramList);
            return res > 0 ? true : false;
        }

        public IEnumerable<MessageModel> GetMessages(int offset, int count)
        {
            string sqlExpression = "sp_GetMessages";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@chat", Value = 2 /* chat id, while chat's system not use*/},
                new SqlParameter { ParameterName = "@offset", Value = offset},
                new SqlParameter { ParameterName = "@count", Value = count}
            };
            return _dbHelper.ExecuteReaderProcedure(sqlExpression, paramList, reader =>
            {
                return new MessageModel()
                {
                    UserId = reader.GetString(0),
                    UserName = reader.GetString(1),
                    Text = reader.GetString(2)
                };
            });
        }

        public IEnumerable<MessageModel> GetMessagesByUserId(string userId)
        {
            string sqlExpression = "sp_GetMessagesByUserId";
            var paramList = new List<SqlParameter>()
            {
                new SqlParameter { ParameterName = "@user", Value = userId }
            };

            return _dbHelper.ExecuteReaderProcedure(sqlExpression, paramList, reader =>
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
