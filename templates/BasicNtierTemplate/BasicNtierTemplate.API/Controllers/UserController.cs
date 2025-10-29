using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BasicNtierTemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/Users/test
        [HttpPost("test")]
        [AllowAnonymous]
        public ActionResult Test(string test)
        {
            return Ok(test);
        }
    }
}