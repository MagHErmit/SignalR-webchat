using MessageChat.DomainModels;

namespace MessageChat.DataRepositories
{
    public interface IAccountRepository
    {
        public UserModel GetUser(string userName);

        public bool RegisterUser(UserModel user);
    }
}
