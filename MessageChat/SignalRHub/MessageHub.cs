using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MessageChat.AuthorizedAccountRepository;
using MessageChat.DataRepositories;
using MessageChat.DomainModels;
using MessageChat.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace MessageChat.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IAuthorizedUsersRepository _usersIdentificators;
        private readonly IMessageRepository _messages;

        public MessageHub(IAuthorizedUsersRepository list, IMessageRepository messages)
        {
            _usersIdentificators = list;
            _messages = messages;
        }

        public override Task OnConnectedAsync()
        {
            if(!string.IsNullOrEmpty(Context.UserIdentifier))
                _usersIdentificators.AddUser(Context.UserIdentifier);
            return Task.CompletedTask;
        }

        public async Task ReciveMessage(string text, int chatId)
        {
            var currentUserIdentificator = Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserName = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            var userChatMessage = new UserChatMessageDto
            {
                Text = text,
                IsMy = false,
                ChatId = chatId,
                UserId = currentUserIdentificator,
                UserName = currentUserName
            };
            await _messages.AppendMessageAsync(new MessageModel(userChatMessage));
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