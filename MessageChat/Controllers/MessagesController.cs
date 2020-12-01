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
        public async Task<JsonResult> GetMessages()
        {
            var l = _messages.GetMessages(0, 5);
            var mess = new List<UserChatMessageDto>();
            foreach (var m in l)
            {
                mess.Add(new UserChatMessageDto()
                {
                    UserId = m.UserId,
                    UserName = m.UserName,
                    Text = m.Text
                });
            }
            mess.Reverse();
            return new JsonResult("asd");
        }
    }
}
