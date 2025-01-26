using Microsoft.AspNetCore.Mvc;
using AppBase.Auth.Token;

namespace AppBase.Auth.Token
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("generate")]
        public IActionResult GenerateToken([FromBody] TokenData request)
        {
            if (request == null || string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest(new { success = false, error = "Invalid request data" });
            }

            var accessToken = _tokenService.GenerateAccessToken(request.UserId);
            return Ok(new { success = true, accessToken });
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            var claims = _tokenService.ValidateToken(token);
            if (claims == null)
            {
                return Unauthorized(new { success = false, error = "Invalid token" });
            }

            return Ok(new { success = true, message = "Token is valid" });
        }
    }
}