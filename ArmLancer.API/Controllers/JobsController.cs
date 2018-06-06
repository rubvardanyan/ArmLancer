﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ArmLancer.API.Controllers
{
    [Authorize(Roles="Employeer,Admin")]
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
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var job = _mapper.Map<Job>(model);     
            job.ClientId = _userService.GetByUserName(userName).Client.Id;
            var m = _crudService.Create(job);
            return Ok(new DataResponse<Job>(m));
        }

        [HttpDelete]
        public override IActionResult Remove(long id)
        {
            if (!_jobService.Exists(id))
                return Ok(new BaseResponse("Job Not Found!"));
            
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));

            if (user.Role == UserRole.Admin)
            {
                _crudService.Delete(id);
                return Ok();
            }

            if (!_jobService.DoesEmployeerOwnJob(user.Client.Id, id))
                return Unauthorized();
            
            _crudService.Delete(id);
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