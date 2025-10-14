using Repository.Entities;

namespace Service.Interfaces
{
    public interface IAuthenService
    {
        public Task<SystemAccount> Authenticate(string username, string password);

        string GenerateJSONWebToken(SystemAccount systemUserAccount);

        string GetRoleName(int? role);
    }

    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        Analyst = 3,
        User = 4
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string RoleName { get; set; }
    }

    public sealed record LoginRequest(string UserName, string Password);
}