using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Auth;
using api.Dto.UserDto;
using api.Services.AuthService;
using api.Services.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class AuthController: BaseController
    {
        private readonly IAuthService authService;
        private readonly ITokenService tokenService;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuthService authService, 
        ITokenService tokenService, ILogger<AuthController> logger)
        {
            this.authService = authService;
            this.tokenService = tokenService;
            this.logger = logger;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register ([FromBody] AddUserDto model)
        {
            logger.LogInformation($"Registration attempt for {model.Email}");

            try
            {
                var data = await authService.Register(model);
                return Ok(data);
            }
            catch (Exception ex)
            {
                 logger.LogError($"Something went wrong in the {nameof(AddUserDto)}. Please contact support");
                 return Problem($"Something went wrong in the {nameof(AddUserDto)}. Please contact support. {ex.Message}");
            }
            

        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var data = await authService.Login(model);
            return Ok(data);

        }

        [HttpPost("refreshtoken")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] AuthResponseDto model)
        {
            var data = await tokenService.VerifyRefreshToken(model);

            if(data == null)
            {
                return Unauthorized();
            }
            return Ok(data);

        }
    }
}