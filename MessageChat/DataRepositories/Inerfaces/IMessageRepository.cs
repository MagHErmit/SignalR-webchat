using MessageChat.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MessageChat.DataRepositories
{
    public interface IMessageRepository
    {
        public Task<bool> AppendMessageAsync(MessageModel message);

        public Task<IEnumerable<MessageModel>> GetMessagesAsync(int offset, int count, int chatId);

        public Task<IEnumerable<MessageModel>> GetMessagesByUserIdAsync(string userId);
    }
}
