using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Utility.Model.LoginRequest;

namespace GenericRepo_Dapper.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService _jwtService;

        public AuthController(IJwtTokenService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] LoginRequest request)
        {
            if (request.Username != "admin" || request.Password != "123")
                return Unauthorized();

            var token = _jwtService.GenerateToken(request.Username);
            return Ok(new { token });
        }

    }
}
