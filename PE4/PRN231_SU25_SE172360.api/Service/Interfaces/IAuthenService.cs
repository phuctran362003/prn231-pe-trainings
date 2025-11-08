namespace Service.Interfaces
{
    public interface IAuthenService
    {
        public Task<LoginResponse> Authenticate(string username, string password);
        string GetRoleName(int? role);
    }

    public enum UserRole
    {
        administrator = 5,
        moderator = 6,
        developer = 7,
        member = 8
    }

    public class LoginRequest()
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public int RoleId { get; set; }
    }
}
