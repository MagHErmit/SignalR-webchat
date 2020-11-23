using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageChat.AuthorizedAccountRepository
{
    public class AuthorizedUsers : IAuthorizedUsersRepository
    {
        private readonly System.Collections.Concurrent.ConcurrentDictionary<string, string> _list = new System.Collections.Concurrent.ConcurrentDictionary<string,string>();

        public IReadOnlyList<string> ToList()
        {
            return _list.Values.ToList();
        }
        public void AddUser(string UserId)
        {
            if (!string.IsNullOrEmpty(UserId))
                _list.TryAdd(UserId,UserId);
        }
        public void RemoveUser(string UserId)
        {
            if(!string.IsNullOrEmpty(UserId)) 
                _list.Remove(UserId, out _);
        }
    }
}
