using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRN231_SU25_SE172295.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheetahAccountController : ControllerBase
    {
        private readonly ICheetahAccountService _services;
        public CheetahAccountController(ICheetahAccountService accountService)
        {
            _services = accountService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            var account = await _services.Login(loginDTO.Email, loginDTO.Password);
            if (account == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            //Generate JWT Token
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, account.Email),
                new Claim("Role", account.RoleId.ToString()),
                new Claim("AccountId", account.AccountId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var preparedToken = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(preparedToken);

            var role = account.RoleId.ToString();
            var accountId = account.AccountId.ToString();

            return Ok(new LoginResponseDTO
            {
                Token = token,
                Role = role,
            });
        }
    }
}
   

