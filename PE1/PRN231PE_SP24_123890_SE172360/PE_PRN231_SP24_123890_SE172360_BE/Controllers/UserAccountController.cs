using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace PE_PRN231_SP24_123890_SE172360_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserAccountService _userAccountService;

        public UserAccountController(IConfiguration configuration, IUserAccountService userAccountService)
        {
            _configuration = configuration;
            _userAccountService = userAccountService;
        }
    }
}
