using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

namespace PE_PRN232_SU25_SE172360_api.Controllers
{
    public class SystemAccountController : ControllerBase
    {
        private readonly IAuthenService _service;

        public SystemAccountController(IAuthenService service)
        {
            _service = service;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _service.Authenticate(request.UserName, request.Password);

            if (user == null)
                return Unauthorized();

            var token = _service.GenerateJSONWebToken(user);

            var response = new LoginResponse
            {
                Token = token,
                RoleName = _service.GetRoleName(user.Role)
            };

            return Ok(response);
        }
    }
}