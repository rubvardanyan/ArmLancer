using System;
using System.Linq;
using System.Security.Claims;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ArmLancer.API.Controllers
{
    [Authorize()]
    [Route("api/v1/submissions")]
    public class JobSubmissionsController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICrudService<JobSubmission> _crudService;
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;
        private readonly IJobSubmissionService _jobSubmissionService;
        private readonly IUserService _userService;
        
        public JobSubmissionsController(
            IServiceProvider serviceProvider,
            IJobService jobService,
            IJobSubmissionService jobSubmissionService,
            IUserService userService)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetService<IMapper>();
            _crudService = _serviceProvider.GetService<ICrudService<JobSubmission>>();
            _jobSubmissionService = jobSubmissionService;
            _userService = userService;
            _jobService = jobService;
        }
        
        [HttpPost]
        [Authorize(Roles="FreeLancer")]
        public IActionResult Create([FromBody] JobSubmissionRequest model)
        {
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));
            var submission = _mapper.Map<JobSubmission>(model); 
            var job = _jobService.Get(submission.JobId);
            
            if (job == null)
                return Ok(new BaseResponse("Invalid Job Id"));
            
            if (user.Client.Submissions.FirstOrDefault(x => x.JobId == submission.JobId) != null)
                return Ok(new BaseResponse("Already submitted"));
            
            submission.ClientId = user.Client.Id;
            var m = _crudService.Create(submission);
            return Ok(new DataResponse<JobSubmission>(m));
        }

        [HttpDelete]
        [Authorize(Roles = "FreeLancer")]
        public IActionResult Remove(long id)
        {
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));

            if (user.Client.Submissions.FirstOrDefault(x => x.Id == id) == null)
                return Ok(new BaseResponse("Invalid submission id"));
            
            _crudService.Delete(id);
            return Ok();
        }
    }
}