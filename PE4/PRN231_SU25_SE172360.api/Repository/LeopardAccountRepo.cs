using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository
{

    public class LeopardAccountRepo : DataAccessObject<LeopardAccount>
    {
        public LeopardAccountRepo()
            : base(new Su25leopardDbContext())
        {
        }

        public async Task<LeopardAccount> GetByEmailAndPassword(string email, string password)
        {
            var user = await Context.LeopardAccounts
                .FirstOrDefaultAsync(a => a.Email == email && a.Password == password);

            if (user == null)
            {
                throw new Exception("Invalid email or password, or account is inactive.");
            }

            return user;
        }
    }

}
