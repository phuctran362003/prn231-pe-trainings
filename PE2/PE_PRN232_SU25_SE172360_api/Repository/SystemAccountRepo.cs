using Microsoft.EntityFrameworkCore;
using Repository.Entities;

namespace Repository
{
    public class SystemAccountRepo : DataAccessObject<SystemAccount>
    {
        public SystemAccountRepo()
            : base(new Spring2025productinventorydbContext())
        {
        }

        public async Task<SystemAccount> GetByEmailAndPassword(string email, string password)
        {
            return await Context.SystemAccounts
                .FirstOrDefaultAsync(a => a.Username == email && a.Password == password && a.IsActive == true);
        }
    }

}
