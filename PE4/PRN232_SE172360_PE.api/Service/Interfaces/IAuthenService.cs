using Repo.Entities;

namespace Service.Interfaces
{
    public interface IAuthenService
    {
        public Task<UserAccount> Authenticate(string username, string password);
        string GenerateJSONWebToken(UserAccount systemUserAccount);
        string GetRoleName(int? role);
    }

    public enum UserRole
    {
        Admin = 1,
        Staff = 2,
        Manager = 3,
        Customer = 4
    }

    public class LoginRequest()
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string RoleName { get; set; }
    }

}
