using System;
using System.Collections.Generic;
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
    [AuthorizeRole(UserRole.Employeer)]
    [Route("api/v1/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;

        public JobsController(IServiceProvider serviceProvider)
        {
            _jobService = serviceProvider.GetService<IJobService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        [HttpPost]
        public IActionResult Create([FromBody] JobRequest model)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = _mapper.Map<Job>(model);
            job.ClientId = long.Parse(clientId);
            var m = _jobService.Create(job);
            return CreatedAtAction(nameof(Get), new {id = m.Id}, new DataResponse<Job>(m));
        }

        [HttpDelete]
        public IActionResult Remove(long id)
        {
            if (!_jobService.Exists(id))
                return NotFound(new BaseResponse("Job Not Found!"));

            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!_jobService.DoesEmployeerOwnJob(clientId, id))
                return Forbid();

            var job = _jobService.Get(id);

            if (job.Status != JobStatus.Waiting)
                return Ok(new BaseResponse("You Cannot Delete Non-Waiting Job."));

            _jobService.Delete(id);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            var m = _jobService.Get(id);
            if (m == null)
            {
                return NotFound();
            }

            return Ok(new DataResponse<Job>(m));
        }

        [HttpGet]
        [Route("~/api/v1/jobs/{id}/finish")]
        public IActionResult Finish(long id)
        {
            if (!_jobService.Exists(id))
                return NotFound(new BaseResponse("Job Not Found!"));

            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!_jobService.DoesEmployeerOwnJob(clientId, id))
                return Forbid();

            if (!_jobService.IsInProgress(id))
                return Ok(new BaseResponse("You Cannot Finish Non-Started Job!"));

            _jobService.FinishJob(id);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/v1/categories/{id}/jobs")]
        public IActionResult GetByCategory(long id)
        {
            var res = _jobService.GetByCategoryId(id);
            return Ok(new DataResponse<IEnumerable<Job>>(res));
        }
    }
}