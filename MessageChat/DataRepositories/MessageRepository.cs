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
        private List<MessageModel> _messages;
        public MessageRepository(DbHelper helper)
        {
            _messages = new List<MessageModel>();
            _dbHelper = helper;
        }
        public bool AppendMessage(MessageModel message)
        {
            string sqlExpression = "sp_AppendMessage";
            var paramList = new List<SqlParameter>();
            SqlParameter chatParam = new SqlParameter
            {
                ParameterName = "@chat",
                Value = message.ChatId
            };
            paramList.Add(chatParam);

            SqlParameter userParam = new SqlParameter
            {
                ParameterName = "@user",
                Value = message.UserId
            };
            paramList.Add(userParam);

            SqlParameter createdParam = new SqlParameter
            {
                ParameterName = "@created",
                Value = DateTime.Now
            };
            paramList.Add(createdParam);

            SqlParameter textParam = new SqlParameter
            {
                ParameterName = "@text",
                Value = message.Text
            };
            paramList.Add(textParam);


            var res = _dbHelper.ExecuteNonQueryProcedure(sqlExpression, paramList);
            return res > 0 ? true : false;
        }

        private bool pushMessages(IDataReader reader)
        {
            _messages.Add(new MessageModel()
            {
                UserId = reader.GetString(0),
                UserName = reader.GetString(1),
                Text = reader.GetString(2)
            });
            return true;
        }

        public List<MessageModel> GetMessages(int offset, int count)
        {
            string sqlExpression = "sp_GetMessages";
            var paramList = new List<SqlParameter>();

            SqlParameter chatParam = new SqlParameter
            {
                ParameterName = "@chat",
                Value = 2 // chat id, while chat's system not use
            };
            paramList.Add(chatParam);

            SqlParameter offsetParam = new SqlParameter
            {
                ParameterName = "@offset",
                Value = offset
            };
            paramList.Add(offsetParam);

            SqlParameter countParam = new SqlParameter
            {
                ParameterName = "@count",
                Value = count
            };
            paramList.Add(countParam);
            _messages = new List<MessageModel>();
            _dbHelper.ExecuteReaderProcedure(sqlExpression, paramList, pushMessages);
            return _messages;
        }

        public List<MessageModel> GetMessagesByUserId(string userId)
        {
            string sqlExpression = "sp_GetMessagesByUserId";
            var paramList = new List<SqlParameter>();

            SqlParameter userParam = new SqlParameter
            {
                ParameterName = "@user",
                Value = userId
            };
            paramList.Add(userParam);
            _messages = new List<MessageModel>();
            _dbHelper.ExecuteReaderProcedure(sqlExpression, paramList, pushMessages);
            return _messages;
        }
    }
}
