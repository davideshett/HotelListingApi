using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Auth;
using api.Dto.UserDto;
using api.Models;
using api.Response;
using api.Services.TokenService;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace api.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IConfiguration configuration;

        public AuthService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, 
        RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

       

        public async Task<object> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if(user == null || isValidUser == false)
            {
                return new GenericResponse
                {
                    Message = "Email or password not correct",
                    IsSuccessful = false,
                    StatusCode = 401
                };
            }

            var token = await tokenService.CreateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = await tokenService.CreateRefreshToken(user)
            };
            
        }

        public async Task<object> Register(AddUserDto model)
        {
            var user = mapper.Map<AppUser>(model);
            user.UserName = model.Email;

            foreach(var role in model.Roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    return new GenericResponse
                    {
                        Message = "One or more roles does not exist",
                        IsSuccessful = false,
                        StatusCode = 401
                    };
                }
            }

            var result = await userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {

                await userManager.AddToRolesAsync(user,model.Roles);

                return new GenericResponse
                {
                    Message = "Success",
                    IsSuccessful = true,
                    StatusCode = 201
                };
            }

            return result.Errors;
        }
    }
}