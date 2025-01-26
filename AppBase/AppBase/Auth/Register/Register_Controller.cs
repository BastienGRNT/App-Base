using Microsoft.AspNetCore.Mvc;

namespace AppBase.Auth.Register;

[ApiController]
[Route("api/[controller]")]
public class Controller_Register : ControllerBase
{
    [HttpPost]
    public IActionResult ApiResgister([FromBody] Data_Register dataRegister)
    {
        if (dataRegister == null)
        {
            return BadRequest(new { success = false, error = "Les données utilisateur sont manquantes." });
        }

        var result = Services_Register.UserRegister(dataRegister);

        if (result.StartsWith("Utilisateur ajouté avec succès"))
        {
            return Ok(new { success = true, message = result });
        }
        else
        {
            return BadRequest(new { success = false, error = result });
        }
    }
}