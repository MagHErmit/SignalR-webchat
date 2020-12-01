using MessageChat.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageChat.DomainModels
{
    public class MessageModel
    {
        public MessageModel() { }

        public MessageModel(UserChatMessageDto message)
        {
            UserName = message.UserName;
            ChatId = message.ChatId;
            UserId = message.UserId;
            Text = message.Text;
        }
        public string UserName { get; set; }

        public int ChatId { get; set; } = 2; // chat id, while chat's system not use

        public string UserId { get; set; }

        public string Text { get; set; }
    }
}
