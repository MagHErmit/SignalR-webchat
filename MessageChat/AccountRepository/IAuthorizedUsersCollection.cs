using System.Collections.Generic;

namespace MessageChat.AccountRepository
{
    public interface IAuthorizedUsersRepository
    {
        public IReadOnlyList<string> ToList();

        public void AddUser(string UserId);

        public void RemoveUser(string UserId);
    }
}
