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

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            this.authService = authService;
            this.tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register ([FromBody] AddUserDto model)
        {
            var data = await authService.Register(model);
            return Ok(data);

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