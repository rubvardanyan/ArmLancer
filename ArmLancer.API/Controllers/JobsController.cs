﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
 using ArmLancer.API.Utils.Attributes;
 using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArmLancer.API.Controllers
{
    [AuthorizeRole(UserRole.Employeer, UserRole.Admin)]
    [Route("api/v1/jobs")]
    public class JobsController : BaseController<Job, JobRequest>
    {
        private readonly IJobService _jobService;
        private readonly IUserService _userService;

        public JobsController(
            IServiceProvider serviceProvider,
            IJobService jobService,
            IUserService userService) : base(serviceProvider)
        {
            _jobService = jobService;
            _userService = userService;
        }

        [HttpPost]
        public override IActionResult Create([FromBody] JobRequest model)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var job = _mapper.Map<Job>(model);
            job.ClientId = long.Parse(clientId);
            var m = _crudService.Create(job);
            return CreatedAtAction("Get", new { id = m.Id }, new DataResponse<Job>(m));
        }

        [HttpDelete]
        public override IActionResult Remove(long id)
        {
            if (!_jobService.Exists(id))
                return NotFound(new BaseResponse("Job Not Found!"));
            
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));

            if (user.Role == UserRole.Admin)
            {
                _crudService.Delete(id);
                return Ok();
            }

            if (!_jobService.DoesEmployeerOwnJob(user.Client.Id, id))
                return Forbid();
            
            _crudService.Delete(id);
            return Ok();
        }
        
        [HttpGet]
        [Route("~/api/v1/jobs/{id}/finish")]
        public IActionResult Finish(long id)
        {
            if (!_jobService.Exists(id))
                return NotFound(new BaseResponse("Job Not Found!"));
            
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));
            
            if (!_jobService.DoesEmployeerOwnJob(user.Client.Id, id))
                return Forbid();

            if (!_jobService.IsInProgress(id))
                return Ok( new BaseResponse("You Cannot Finish Non-Started Job!"));
            
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