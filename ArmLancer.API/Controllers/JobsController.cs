using System;
using System.Collections.Generic;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArmLancer.API.Controllers
{
    [Route("api/v1/jobs")]
    public class JobsController : BaseController<Job, JobRequest>
    {
        private readonly IJobService _jobService;
        
        public JobsController(IServiceProvider serviceProvider, IJobService jobService) : base(serviceProvider)
        {
            _jobService = jobService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetByCategory(long categoryId)
        {
            var res = _jobService.GetByCategoryId(categoryId);
            return Ok(new DataResponse<IEnumerable<Job>>(res));
        }
    }
}