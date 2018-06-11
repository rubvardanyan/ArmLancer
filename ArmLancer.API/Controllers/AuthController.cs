using System.Linq;
using ArmLancer.API.Auth;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
using ArmLancer.API.Utils.Settings;
using ArmLancer.Core.Interfaces;
using ArmLancer.Core.Utils.Helpers;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ArmLancer.API.Controllers
{
    [Authorize]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        
        private readonly AuthSettings _authSettings;
        private readonly IUserService _userService;

        public AuthController(IUserService userService, IOptions<AuthSettings> authSettingsAccessor)
        {
            _userService = userService;
            _authSettings = authSettingsAccessor.Value;
        }
        
        private AuthTicket CreateTicketResponse(User user)
        {
            var authTicket = new AuthTicket(user, _authSettings);
            return authTicket;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var user = _userService.GetByCredentials(request.UserName, request.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(CreateTicketResponse(user));
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("freelancer/register")]
        public IActionResult RegisterFreeLancer([FromBody]RegisterRequest request)
        {
            if (_userService.All.Any(u => u.UserName == request.UserName))
            {
                return Ok(new BaseResponse("User already exists."));
            }

            var user = _userService.Register(new User
            {
                Password = CryptoHelper.Encrypt(request.Password),
                UserName = request.UserName,
                Role = UserRole.FreeLancer,
                Client = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Email = request.Email
                }
            });
            
            return Ok(CreateTicketResponse(user));
        }
        
        [AllowAnonymous]
        [HttpPost]
        [Route("employeer/register")]
        public IActionResult RegisterEmployeer([FromBody]RegisterRequest request)
        {
            if (_userService.All.Any(u => u.UserName == request.UserName))
            {
                return Ok(new BaseResponse("User already exists."));
            }

            var user = _userService.Register(new User
            {
                Password = CryptoHelper.Encrypt(request.Password),
                UserName = request.UserName,
                Role = UserRole.Employeer,
                Client = new Client
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Email = request.Email
                }
            });
            
            return Ok(CreateTicketResponse(user));
        }
    }
}