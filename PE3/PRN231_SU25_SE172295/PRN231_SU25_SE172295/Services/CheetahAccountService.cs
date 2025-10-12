using BOs;
using Repos;

namespace Services
{
    public interface ICheetahAccountService
    {
        Task<CheetahAccount> Login(string email, string password);
    }
    public class CheetahAccountService : ICheetahAccountService
    {
        private readonly ICheetahAccountRepo _repo;
        public CheetahAccountService(ICheetahAccountRepo accountRepo)
        {
            _repo = accountRepo;
        }
        public async Task<CheetahAccount> Login(string email, string password)
        {
            return await _repo.Login(email, password);
        }
    }
}
