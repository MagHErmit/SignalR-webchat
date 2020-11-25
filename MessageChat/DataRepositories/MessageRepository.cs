using MessageChat.DomainModels;
using System;
using System.Collections.Generic;

namespace MessageChat.DataRepositories
{
    public class MessageRepository : IMessageRepository
    {
        public bool AppendMessage(MessageModel message)
        {
            throw new NotImplementedException();
        }

        public List<MessageModel> GetMessages(int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
