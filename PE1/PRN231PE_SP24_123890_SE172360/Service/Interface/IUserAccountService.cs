using Repository.Entities;

namespace Service.Interface
{
    public interface IUserAccountService
    {
        public Task<UserAccount> Authenticate(string username, string password);
    }
}
