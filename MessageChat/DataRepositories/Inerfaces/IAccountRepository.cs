using MessageChat.DomainModels;
using System.Threading.Tasks;

namespace MessageChat.DataRepositories
{
    public interface IAccountRepository
    {
        public Task<UserModel> GetUserAsync(string userName);

        public Task<bool> RegisterUserAsync(UserModel user);

    }
}
