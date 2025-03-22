using Repository;
using Repository.Entities;
using Service.Interface;

namespace Service.Service
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserAccountRepo _repo;

        public UserAccountService(UserAccountRepo repo)
        {
            _repo = repo;
        }

        public async Task<UserAccount> Authenticate(string username, string password)
        {
            return await _repo.GetByEmailAndPassword(username, password);
        }
    }
}
