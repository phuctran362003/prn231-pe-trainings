using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Entities;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Services
{
    public class AuthenService : IAuthenService
    {
        private readonly LeopardAccountRepo _repo;
        private readonly IConfiguration _configuration;

        public AuthenService(IConfiguration configuration)
        {
            _repo = new LeopardAccountRepo();
            _configuration = configuration;
        }

        public async Task<LoginResponse> Authenticate(string username, string password)
        {
            var account = await _repo.GetByEmailAndPassword(username, password);
            if (account == null)
            {
                throw new Exception("Invalid credentials or inactive account.");
            }

            var token = GenerateJSONWebToken(account);

            return new LoginResponse
            {
                Token = token,
                RoleId = account.RoleId
            };
        }

        private string GenerateJSONWebToken(LeopardAccount systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"]
                    , _configuration["Jwt:Audience"]
                    , new Claim[]
                    {
                        new(ClaimTypes.Name, systemUserAccount.Email),
                         //new(ClaimTypes.Role, systemUserAccount.Role.ToString()),
                        new("RoleId", systemUserAccount.RoleId.ToString())
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
                5 => "administrator",
                6 => "moderator",
                7 => "developer",
                8 => "member",
                _ => "Unknown"
            };
        }
    }

}
