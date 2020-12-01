using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MessageChat.DataRepositories;
using MessageChat.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageChat.Controllers
{
    [Route("messages")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messages;
        public MessagesController(IMessageRepository messages)
        {
            _messages = messages;
        }
        [HttpGet("Get")]
        public async Task<IEnumerable<UserChatMessageDto>> GetMessages()
        {
            var currentUserId = HttpContext.User.Claims.ElementAt(0).Value;
            return _messages.GetMessages(0, 20)
                .Select(m => new UserChatMessageDto
                {
                    UserId = m.UserId,
                    UserName = m.UserName,
                    Text = m.Text,
                    IsMy = currentUserId == m.UserId
                }).Reverse();
        }
    }
}
