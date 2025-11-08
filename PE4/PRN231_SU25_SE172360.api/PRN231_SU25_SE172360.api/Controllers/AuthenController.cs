using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace PRN231_SU25_SE172360.api.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenController : ControllerBase
    {
        private readonly IAuthenService _service;

        public AuthenController(IAuthenService service)
        {
            _service = service;
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _service.Authenticate(request.Email, request.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }

}
