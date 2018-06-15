using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
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
        private readonly IJobService _jobService;

        public RatingsController(IServiceProvider serviceProvider)
        {
            _ratingService = serviceProvider.GetService<IRatingService>();
            _jobService = serviceProvider.GetService<IJobService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        [HttpGet]
        [Authorize]
        [Route("~/api/v1/{clientId}/ratings")]
        public IActionResult GetByClientTo(long clientId)
        {
            var ratings = _ratingService.GetByClientTo(clientId);
            return Ok(new DataResponse<IEnumerable<Rating>>(ratings));
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

            var jobOwnerId = _jobService.Get(jobId).ClientId;

            var rating = _mapper.Map<Rating>(model);
            rating.ClientIdFrom = clientId;
            rating.ClientIdTo = jobOwnerId;
            rating.JobId = jobId;

            _ratingService.Create(rating);
            
            return Ok();
        }

        private IActionResult CreateAsEmployeer(long clientId, long jobId, RatingRequest model)
        {
            if (!_ratingService.EmployeerCanWriteReview(jobId, clientId))
                return Forbid();

            var freeLancerId = _jobService.GetJobFreeLancerId(jobId);

            if (freeLancerId == null)
            {
                return BadRequest(new BaseResponse("Job is not started"));
            }

            var rating = _mapper.Map<Rating>(model);
            rating.ClientIdFrom = clientId;
            rating.ClientIdTo = freeLancerId.Value;
            rating.JobId = jobId;
            
            _ratingService.Create(rating);
            
            return Ok();
        }
    }
}