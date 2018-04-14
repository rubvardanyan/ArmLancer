using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArmLancer.API.Auth;
using ArmLancer.API.Models.Auth;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Utils.Settings;
using ArmLancer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ArmLancer.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        
        private readonly AuthSettings _authSettings;

        public AuthController( IOptions<AuthSettings> authSettingsAccessor)
        {
            //_userSevice = userService;
            _authSettings = authSettingsAccessor.Value;
        }
        
        private AuthTicket CreateTicketResponse(User user)
        {
            var authTicket = new AuthTicket(user, _authSettings);
            return authTicket;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var user = _userSevice.GetByCredentials(request.UserName, request.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(CreateTicketResponse(user));
        }
        
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]RegisterRequest request)
        {
            if (_userSevice.All.Any(u => u.UserName == request.UserName))
            {
                return Ok(new BaseResponse("User already exists."));
            }

            var user = _userSevice.Register(new User //use extension based/automapper mapping, should discuss
            {
                Password = CryptoHelper.Encrypt(request.Password),
                UserName = request.UserName,
                Role = UserRole.Client,
                Confirmed = false
            });
            return Ok(CreateTicketResponse(user));
        }
    }
}