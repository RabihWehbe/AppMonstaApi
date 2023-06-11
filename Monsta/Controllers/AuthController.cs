using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monsta.dto;
using Monsta.Models;
using Monsta.Services;
using Newtonsoft.Json;

namespace Monsta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        RegisterService _registerService { get; set; }
        LoginService _loginService { get; set; }


        public AuthController(RegisterService registerService,LoginService loginService)
        {
            _registerService = registerService;
            _loginService = loginService;
        }



        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var register = await _registerService.register(request);

            return Ok(register);
        }


        [HttpPost("login")]
        public async Task<ActionResult<dynamic>> Login(UserDto request)
        {
            LoginDto login = await _loginService.verify(request);

            if(login == null)
            {
                dynamic msg = new {errorMsg = "user not found"};
                return BadRequest(msg);
            }
            else if(login.token == null)
            {
                dynamic msg = new { errorMsg = "password not correct" };
                return BadRequest(msg);
            }

            return Ok(login);

        }


    }
}
