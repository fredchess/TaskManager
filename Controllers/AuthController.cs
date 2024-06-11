using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {


        public AuthController()
        {
        }

        [HttpPost("logout"), Authorize]
        public async Task<ActionResult> Logout(SignInManager<User> signInManager)
        {
            await signInManager.SignOutAsync();

            return Ok();
        }
        
    }
}