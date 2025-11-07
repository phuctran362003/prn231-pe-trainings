using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthenService _service;

        public AuthenController(IAuthenService service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _service.Authenticate(request.Email, request.Password);
                var token = _service.GenerateJSONWebToken(user);

                var response = new LoginResponse
                {
                    Token = token,
                    RoleName = _service.GetRoleName(user.Role)
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi không mong muốn trong quá trình đăng nhập." });
                throw;
            }

        }
    }

}
