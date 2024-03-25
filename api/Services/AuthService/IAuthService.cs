using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dto.Auth;
using api.Dto.UserDto;
using api.Models;

namespace api.Services.AuthService
{
    public interface IAuthService
    {
        Task<object> Register (AddUserDto model);
        Task<object> Login(LoginDto loginDto);
       
    }
}