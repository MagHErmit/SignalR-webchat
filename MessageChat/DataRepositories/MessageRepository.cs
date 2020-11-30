using MessageChat.DomainModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MessageChat.DataRepositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(IOptions<ConnectionSetting> conn)
        {
            _connectionString = conn.Value.DefaultConnection;
        }
        public bool AppendMessage(MessageModel message)
        {
            string sqlExpression = "sp_AppendMessage";
            using SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter chatParam = new SqlParameter
            {
                ParameterName = "@chat",
                Value = message.ChatId
            };
            command.Parameters.Add(chatParam);

            SqlParameter userParam = new SqlParameter
            {
                ParameterName = "@user",
                Value = message.UserId
            };
            command.Parameters.Add(userParam);

            SqlParameter createdParam = new SqlParameter
            {
                ParameterName = "@created",
                Value = DateTime.Now
            };
            command.Parameters.Add(createdParam);

            SqlParameter textParam = new SqlParameter
            {
                ParameterName = "@text",
                Value = message.Text
            };
            command.Parameters.Add(textParam);

            
            var res = command.ExecuteNonQuery();
            return res > 0 ? true : false;
        }

        public List<MessageModel> GetMessages(int offset, int count)
        {
            string sqlExpression = "sp_GetMessages";
            using SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter chatParam = new SqlParameter
            {
                ParameterName = "@chat",
                Value = 2 // chat id, while chat's system not use
            };
            command.Parameters.Add(chatParam);

            SqlParameter offsetParam = new SqlParameter
            {
                ParameterName = "@offset",
                Value = offset
            };
            command.Parameters.Add(offsetParam);

            SqlParameter countParam = new SqlParameter
            {
                ParameterName = "@count",
                Value = count
            };
            command.Parameters.Add(countParam);


            var res = command.ExecuteReader();
            List<MessageModel> l = new List<MessageModel>();
            while (res.Read())
            {
                l.Add(new MessageModel()
                {
                    UserId = res.GetString(0),
                    Text = res.GetString(1)
                });
            }
            return l;
        }

        public List<MessageModel> GetMessagesByUserId(string userId)
        {
            string sqlExpression = "sp_GetMessagesByUserId";
            using SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();
            SqlCommand command = new SqlCommand(sqlExpression, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter userParam = new SqlParameter
            {
                ParameterName = "@user",
                Value = userId
            };
            command.Parameters.Add(userParam);

            var res = command.ExecuteReader();
            List<MessageModel> l = new List<MessageModel>();
            while(res.Read())
            {
                l.Add(new MessageModel()
                {
                    ChatId = res.GetInt32(0),
                    UserId = userId,
                    Text = res.GetString(1)
                });
            }
            return l;
        }
    }
}
