using Microsoft.EntityFrameworkCore;
using Repo.Entities;

namespace Repo
{


    public class UserAccountRepo : DataAccessObject<UserAccount>
    {
        public UserAccountRepo()
            : base(new WatercolorsPainting2024DbContext())
        {
        }

        public async Task<UserAccount> GetByEmailAndPassword(string email, string password)
        {
            var user = await _context.UserAccounts
                .FirstOrDefaultAsync(a => a.UserEmail == email && a.UserPassword == password);

            if (user == null)
            {
                throw new Exception("Invalid credentials or inactive account.");
            }

            return user;
        }
    }
}
