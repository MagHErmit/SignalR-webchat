using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MessageChat.Dto;
using MessageChat.AccountRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Data.SqlClient;

namespace MessageChat.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IAuthorizedUsersRepository _usersIdentificators;

        public MessageHub(IAuthorizedUsersRepository list)
        {
            _usersIdentificators = list;
        }

        public override Task OnConnectedAsync()
        {
            if(!string.IsNullOrEmpty(Context.UserIdentifier))
                _usersIdentificators.AddUser(Context.UserIdentifier);
            /*
            string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=kakurin_webchat;Integrated Security=True";
            string sqlExpression = "sp_GetUsers";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3));

                    while (reader.Read())
                    {
                        string id = reader.GetString(0);
                        string name = reader.GetString(1);
                        int age = reader.GetInt32(2);
                        Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                    }
                }
                reader.Close();
            }
            */
            return Task.CompletedTask;
        }

        public async Task ReciveMessage(string text)
        {
            var currentUserIdentificator = Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserName = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            var userChatMessage = new UserChatMessageDto
            {
                Text = text,
                IsMy = false,
                UserIdentificator = currentUserIdentificator,
                UserName = currentUserName
            };
            await SendMessage(userChatMessage);
        }
       
        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            if (!string.IsNullOrEmpty(Context.UserIdentifier))
                _usersIdentificators.RemoveUser(Context.UserIdentifier);
            return Task.CompletedTask;
        }

        private async Task SendMessage(UserChatMessageDto message)
        {
            await Clients.Users(_usersIdentificators.ToList()).SendAsync("ReciveFromServerMessage", message);
        }
    }
}