using MessageChat.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageChat.DataRepositories.Inerfaces
{
    public interface IChatsRepository
    {
        public Task<IEnumerable<ChatModel>> GetChatsByUserIdAsync(string userId);
    }
}
