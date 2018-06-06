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
        [Route("~/api/v1/jobs/{id}/submissions")]
        public IActionResult GetList(long id)
        {
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));

            if (!_jobService.Exists(id))
                return Ok(new BaseResponse("Job Not Found!"));

            if (user.Role == UserRole.Admin)
            {
                var response = _mapper.Map<JobSubmissionResponse>(_jobSubmissionService.GetByJobId(id).ToList());
                return Ok(new DataResponse<JobSubmissionResponse>(response));            
            }
            
            if (user.Role == UserRole.Employeer)
            {
                var job = _jobService.Get(id);

                if (job.ClientId != user.Client.Id)
                    return Unauthorized();

                var response = _mapper.Map<JobSubmissionResponse>(_jobSubmissionService.GetByJobId(id).ToList());
                return Ok(new DataResponse<JobSubmissionResponse>(response));
            }

            var submission = _jobSubmissionService.GetByClientAndJobId(user.Client.Id, id);
            return Ok(new DataResponse<JobSubmissionResponse>(_mapper.Map<JobSubmissionResponse>(submission)));
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
            
            if (_jobSubmissionService.AlreadyAcceptedOtherSubmit(submission.JobId))
                return Ok(new BaseResponse("Already Accepted Other Submission!"));
            
            submission.ClientId = user.Client.Id;
            var m = _crudService.Create(submission);
            return Ok(new DataResponse<JobSubmissionResponse>(_mapper.Map<JobSubmissionResponse>(m)));
        }

        [HttpDelete]
        [Authorize(Roles = "FreeLancer")]
        [Route("{id}/cancel")]
        public IActionResult Cancel(long id)
        { 
            if (!_jobSubmissionService.Exists(id))
                return Ok(new BaseResponse("Submission Not Found!"));

            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));
           
            if (!_jobSubmissionService.DoesClientHaveSubmission(user.Client.Id, id))
                return Unauthorized();

            _jobSubmissionService.CancelSubmission(id);
            
            return Ok();
        }
        
        [HttpDelete]
        [Authorize(Roles = "Employeer")]
        [Route("{id}/decline")]
        public IActionResult Decline(long id)
        {
            if (!_jobSubmissionService.Exists(id))
                return Ok(new BaseResponse("Submission Not Found!"));
            
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));
            
            var submission = _jobSubmissionService.Get(id);

            if (!_jobService.DoesEmployeerOwnJob(user.Client.Id, submission.JobId))
                return Unauthorized();
            
            if (_jobSubmissionService.AlreadyAcceptedOtherSubmit(submission.JobId))
                return Ok(new BaseResponse("Already Accepted Other Submission!"));

            _jobSubmissionService.DeclineSubmission(id);
            
            return Ok();
        }
        
        [HttpGet]
        [Authorize(Roles = "Employeer")]
        [Route("{id}/accept")]
        public IActionResult Accept(long id)
        {
            if (!_jobSubmissionService.Exists(id))
                return Ok(new BaseResponse("Submission Not Found!"));
            
            var user = _userService.GetByUserName(User.FindFirstValue(ClaimTypes.Name));
            
            var submission = _jobSubmissionService.Get(id);

            if (!_jobService.DoesEmployeerOwnJob(user.Client.Id, submission.JobId))
                return Unauthorized();
            
            if (_jobSubmissionService.AlreadyAcceptedOtherSubmit(submission.JobId))
                return Ok(new BaseResponse("Already Accepted Other Submission!"));

            _jobSubmissionService.AcceptSubmission(id);
            
            return Ok();
        }
    }
}