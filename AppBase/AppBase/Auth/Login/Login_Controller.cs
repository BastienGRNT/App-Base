using Microsoft.AspNetCore.Mvc;

namespace AppBase.Auth.Login;

[ApiController]
[Route("api/[controller]")]
public class Controllers_Login : ControllerBase
{
    [HttpPost]
    public IActionResult ApiLogin([FromBody] Data_Login dataLogin)
    {
        if (dataLogin == null)
        {
            return BadRequest(new { success = false, error = "Les données sont manquantes." });
        }

        var checkLoginService = new CheckLoginClass();
        var result = checkLoginService.UserLogin(dataLogin);

        if (result.StartsWith("Mot de passe correct !"))
        {
            return Ok(new { success = true, message = result });
        }
        else
        {
            return BadRequest(new { success = false, error = result });
        }
    }
}