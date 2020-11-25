using MessageChat.DomainModels;
using System.Collections.Generic;

namespace MessageChat.DataRepositories
{
    public interface IMessageRepository
    {
        public bool AppendMessage(MessageModel message);

        public List<MessageModel> GetMessages(int offset, int count);
    }
}
