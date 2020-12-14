using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MessageChat.DataRepositories;
using MessageChat.DataRepositories.Inerfaces;
using MessageChat.DomainModels;
using MessageChat.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace MessageChat.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IChatsRepository _chats;
        private readonly IMessageRepository _messages;

        public MessageHub(IMessageRepository messages, IChatsRepository chats)
        {
            _messages = messages;
            _chats = chats;
        }

        public async override Task OnConnectedAsync()
        {
            foreach(var c in await _chats.GetChatsByUserIdAsync(Context.UserIdentifier))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, c.Id.ToString());
            }
        }

        public async Task ReciveMessage(string text, int chatId)
        {
            var currentUserIdentificator = Context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserName = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            var userChatMessage = new MessageModel
            {
                Text = text,     
                ChatId = chatId,
                UserId = currentUserIdentificator,
                UserName = currentUserName
            };
            await _messages.AppendMessageAsync(userChatMessage);
            await SendMessage(new UserChatMessageDto(userChatMessage)
            {
                IsMy = false
            });
        }
        
        public async override Task OnDisconnectedAsync(Exception exp)
        {
            foreach (var c in await _chats.GetChatsByUserIdAsync(Context.UserIdentifier))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, c.Id.ToString());
            }
        }

        private async Task SendMessage(UserChatMessageDto message)
        {
            await Clients.Group(message.ChatId.ToString()).SendAsync("ReciveFromServerMessage", message);
        }
    }
}