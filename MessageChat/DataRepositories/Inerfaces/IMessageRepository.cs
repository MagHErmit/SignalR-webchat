using MessageChat.DomainModels;
using System.Collections.Generic;

namespace MessageChat.DataRepositories
{
    public interface IMessageRepository
    {
        public MessageModel GetMessage(int id);

        public List<MessageModel> GetPartMessages(int offset);
    }
}
