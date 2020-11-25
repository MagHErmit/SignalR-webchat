using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MessageChat.AuthorizedAccountRepository;
using MessageChat.DataRepositories;
using MessageChat.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

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

        public override async Task OnConnectedAsync()
        {
            if(!string.IsNullOrEmpty(Context.UserIdentifier))
                _usersIdentificators.AddUser(Context.UserIdentifier);
            var mes = new UserChatMessageDto()
            {
                UserIdentificator = Context.UserIdentifier,
                IsMy = true,
                UserName = Context.User.Claims.ToArray()[1].Value,
                Text = "testing init"
            };
            await SendInitMessages(new List<UserChatMessageDto>() { mes });
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

        private async Task SendInitMessages(List<UserChatMessageDto> messages)
        {
            await Clients.User(Context.UserIdentifier).SendAsync("InitMessages", messages);
        }

        private async Task SendMessage(UserChatMessageDto message)
        {
            await Clients.Users(_usersIdentificators.ToList()).SendAsync("ReciveFromServerMessage", message);
        }
    }
}