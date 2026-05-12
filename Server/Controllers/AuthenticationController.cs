using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase {
    [HttpPost("register")]
    public IActionResult Register() {
        return Ok();
    }
    
    [HttpPost("login")]
    public IActionResult Login() {
        return Ok();
    }
}