using BOs;
using DAOs;

namespace Repos
{
    public interface ICheetahAccountRepo
    {
        Task<CheetahAccount> Login(string email, string password);
    }
    public class CheetahAccountRepo : ICheetahAccountRepo
    {
        public async Task<CheetahAccount> Login(string email, string password)
        {
            return await CheetahAccountDAO.Instance.Login(email, password);
        }
    }
}
