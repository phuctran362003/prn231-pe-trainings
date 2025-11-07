using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repo;
using Repo.Entities;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class AuthenService : IAuthenService
    {
        private readonly UserAccountRepo _repo;
        private readonly IConfiguration _configuration;

        public AuthenService(IConfiguration configuration)
        {
            _repo = new UserAccountRepo(); // tự new, không qua DI
            _configuration = configuration;
        }

        public async Task<UserAccount> Authenticate(string username, string password)
        {
            var account = await _repo.GetByEmailAndPassword(username, password);
            if (account == null)
            {
                throw new Exception("Invalid credentials or inactive account.");
            }
            return account;
        }

        public string GenerateJSONWebToken(UserAccount systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"]
                    , _configuration["Jwt:Audience"]
                    , new Claim[]
                    {
                        new(ClaimTypes.Name, systemUserAccount.UserEmail),
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
                2 => "Staff",
                3 => "Manager",
                4 => "Customer",
                _ => "Unknown"
            };
        }
    }

}
