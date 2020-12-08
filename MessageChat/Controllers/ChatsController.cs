using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessageChat.DataRepositories.Inerfaces;
using MessageChat.DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageChat.Controllers
{
    [Authorize]
    [Route("dialogs")]
    public class ChatsController : ControllerBase
    {
        private readonly IChatsRepository _chats;
        public ChatsController(IChatsRepository messages)
        {
            _chats = messages;
        }
        [HttpGet("Get")]
        public async Task<IEnumerable<ChatModel>> GetDialogs()
        {
           return await _chats.GetChatsByUserIdAsync(HttpContext.User.Claims.ToArray()[0].Value);
        }

        [HttpPost("Create")]
        public async Task<JsonResult> CreateChat(string name)
        {
            var user_id = HttpContext.User.Claims.ElementAt(0).Value;
            var id = await _chats.CreateChatAsync(name, user_id);
            return new JsonResult(new ChatModel()
            {
                Id = id,
                Name = name,
                UserCreatorId = user_id
            });
            
        }
    }
}
