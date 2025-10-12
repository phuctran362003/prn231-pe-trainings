using BOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAOs
{
    public class CheetahAccountDAO
    {
        private static CheetahAccountDAO instance = null;
        private readonly Su25cheetahDbContext context;

        private CheetahAccountDAO()
        {
            context = new Su25cheetahDbContext();
        }

        public static CheetahAccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CheetahAccountDAO();
                }
                return instance;
            }
        }

        public async Task<CheetahAccount> Login(string email, string password)
        {
            var account = await context.CheetahAccounts.FirstOrDefaultAsync(account => account.Email == email && account.Password == password);
            return account;
        }
    }
}