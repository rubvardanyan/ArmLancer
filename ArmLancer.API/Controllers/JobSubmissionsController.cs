using System;
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
    [Authorize]
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
        
        [HttpGet]
        [Authorize]
        public IActionResult GetList(long jobId)
        {
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));

            if (!_jobService.Exists(jobId))
                return Ok(new BaseResponse("Job Not Found!"));

            if (user.Role == UserRole.Admin)
            {
                return Ok(new DataResponse<JobSubmission>(_jobSubmissionService.GetByJobId(jobId).ToList()));
            }
            
            if (user.Role == UserRole.Employeer)
            {
                var job = _jobService.Get(jobId);

                if (job.ClientId != user.Client.Id)
                    return Unauthorized();

                return Ok(new DataResponse<JobSubmission>(_jobSubmissionService.GetByJobId(jobId).ToList()));
            }

            var submission = _jobSubmissionService.GetByClientAndJobId(user.Client.Id, jobId);
            return Ok(new DataResponse<JobSubmission>(submission));
        }

        [HttpPost]
        [Authorize(Roles = "FreeLancer")]
        public IActionResult Create([FromBody] JobSubmissionRequest model)
        {
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));
            var submission = _mapper.Map<JobSubmission>(model);

            if (!_jobService.Exists(submission.JobId))
                return Ok(new BaseResponse("Job Not Found!"));

            if (_jobSubmissionService.AlreadySubmitted(user.Client.Id, submission.JobId))
                return Ok(new BaseResponse("Already Submitted!"));

            submission.ClientId = user.Client.Id;
            var m = _crudService.Create(submission);
            //TODO: Fix response.
            return Ok(new DataResponse<JobSubmission>(m));
        }

        [HttpDelete]
        [Authorize(Roles = "FreeLancer")]
        public IActionResult Remove(long id)
        {
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));
            
            if (!_jobSubmissionService.Exists(id))
                return Ok(new BaseResponse("Submission Not Found!"));

            if (!_jobSubmissionService.DoesClientHaveSubmission(user.Client.Id, id))
                return Unauthorized();

            _crudService.Delete(id);
            return Ok();
        }
    }
}