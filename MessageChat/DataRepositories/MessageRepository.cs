using MessageChat.DomainModels;
using System;
using System.Collections.Generic;

namespace MessageChat.DataRepositories
{
    public class MessageRepository : IMessageRepository
    {
        public MessageModel GetMessage(int id)
        {
            throw new NotImplementedException();
        }

        public List<MessageModel> GetPartMessages(int offset)
        {
            throw new NotImplementedException();
        }
    }
}
