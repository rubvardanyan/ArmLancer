using System;
using System.Security.Claims;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Utils.Attributes;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ArmLancer.API.Controllers
{
    [Authorize]
    [Route("api/v1/ratings")]
    public class RatingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;

        public RatingsController(IServiceProvider serviceProvider)
        {
            _ratingService = serviceProvider.GetService<IRatingService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        [HttpPost]
        [Route("{jobId}")]
        [AuthorizeRole(UserRole.Employeer, UserRole.FreeLancer)]
        public IActionResult Create(long jobId, [FromBody] RatingRequest model)
        {
            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserRole userRole;

            if (Enum.TryParse(typeof(UserRole), User.FindFirstValue(ClaimTypes.Role), true, out var role))
            {
                userRole = (UserRole) role;
            }
            else
            {
                return BadRequest();
            }

            return userRole == UserRole.FreeLancer
                ? CreateAsFreeLancer(clientId, jobId, model)
                : CreateAsEmployeer(clientId, jobId, model);
        }

        private IActionResult CreateAsFreeLancer(long clientId, long jobId, RatingRequest model)
        {
            if (!_ratingService.FreeLancerCanWriteReview(jobId, clientId))
                return Forbid();

            return Ok();
        }

        private IActionResult CreateAsEmployeer(long clientId, long jobId, RatingRequest model)
        {
            if (!_ratingService.EmployeerCanWriteReview(jobId, clientId))
                return Forbid();

            return Ok();
        }
    }
}