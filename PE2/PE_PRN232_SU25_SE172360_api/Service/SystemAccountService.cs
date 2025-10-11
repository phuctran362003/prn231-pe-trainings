using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly SystemAccountRepo _repo;
        private readonly IConfiguration _configuration;

        public SystemAccountService(IConfiguration configuration)
        {
            _repo = new SystemAccountRepo(); // tự new, không qua DI
            _configuration = configuration;
        }

        public async Task<SystemAccount> Authenticate(string username, string password)
        {
            var account = await _repo.GetByEmailAndPassword(username, password);
            if (account == null)
            {
                throw new Exception("Invalid credentials or inactive account.");
            }
            return account;
        }

        public string GenerateJSONWebToken(SystemAccount systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"]
                    , _configuration["Jwt:Audience"]
                    , new Claim[]
                    {
                        new(ClaimTypes.Name, systemUserAccount.Email),
                        new(ClaimTypes.Role, systemUserAccount.Role.ToString()),
                    },
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public string GetRoleName(int? role)
        {
            return role switch
            {
                1 => "Admin",
                2 => "Manager",
                3 => "Analyst",
                4 => "User",
                _ => "Unknown"
            };
        }
    }
}
