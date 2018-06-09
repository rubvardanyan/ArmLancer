using System;
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
    [Route("api/v1/submissions")]
    public class JobSubmissionsController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICrudService<JobSubmission> _crudService;
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;
        private readonly IJobSubmissionService _jobSubmissionService;

        public JobSubmissionsController(
            IServiceProvider serviceProvider,
            IJobService jobService,
            IJobSubmissionService jobSubmissionService)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetService<IMapper>();
            _crudService = _serviceProvider.GetService<ICrudService<JobSubmission>>();
            _jobSubmissionService = jobSubmissionService;
            _jobService = jobService;
        }
        
        [HttpGet]
        [Authorize]
        [Route("~/api/v1/jobs/{id}/submissions")]
        public IActionResult GetList(long id)
        {
            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            UserRole userRole;
            
            if (Enum.TryParse(typeof(UserRole), User.FindFirstValue(ClaimTypes.Role), true, out var role))
            {
                userRole = (UserRole)role;
            }
            else
            {
                return BadRequest();
            }

            if (!_jobService.Exists(id))
                return NotFound(new BaseResponse("Job Not Found!"));

            if (userRole == UserRole.Admin)
            {
                var response = _mapper.Map<JobSubmissionResponse>(_jobSubmissionService.GetByJobId(id).ToList());
                return Ok(new DataResponse<JobSubmissionResponse>(response));            
            }
            
            if (userRole == UserRole.Employeer)
            {
                var job = _jobService.Get(id);

                if (job.ClientId != clientId)
                    return Forbid();

                var response = _mapper.Map<JobSubmissionResponse>(_jobSubmissionService.GetByJobId(id).ToList());
                return Ok(new DataResponse<JobSubmissionResponse>(response));
            }

            var submission = _jobSubmissionService.GetByClientAndJobId(clientId, id);
            return Ok(new DataResponse<JobSubmissionResponse>(_mapper.Map<JobSubmissionResponse>(submission)));
        }

        [HttpPost]
        [AuthorizeRole(UserRole.FreeLancer)]
        public IActionResult Create([FromBody] JobSubmissionRequest model)
        {
            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var submission = _mapper.Map<JobSubmission>(model);

            if (!_jobService.Exists(submission.JobId))
                return NotFound(new BaseResponse("Job Not Found!"));

            if (_jobSubmissionService.AlreadySubmitted(clientId, submission.JobId))
                return Ok(new BaseResponse("Already Submitted!"));
            
            if (_jobSubmissionService.AlreadyAcceptedOtherSubmit(submission.JobId))
                return Ok(new BaseResponse("Already Accepted Other Submission!"));
            
            submission.ClientId = clientId;
            var m = _crudService.Create(submission);
            return Ok(new DataResponse<JobSubmissionResponse>(_mapper.Map<JobSubmissionResponse>(m)));
        }

        [HttpPut]
        [AuthorizeRole(UserRole.FreeLancer)]
        [Route("{id}/cancel")]
        public IActionResult Cancel(long id)
        { 
            if (!_jobSubmissionService.Exists(id))
                return NotFound(new BaseResponse("Submission Not Found!"));

            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
           
            if (!_jobSubmissionService.DoesClientHaveSubmission(clientId, id))
                return Forbid();

            _jobSubmissionService.CancelSubmission(id);
            
            return Ok();
        }
        
        [HttpPut]
        [AuthorizeRole(UserRole.Employeer)]
        [Route("{id}/decline")]
        public IActionResult Decline(long id)
        {
            if (!_jobSubmissionService.Exists(id))
                return NotFound(new BaseResponse("Submission Not Found!"));
            
            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var submission = _jobSubmissionService.Get(id);

            if (!_jobService.DoesEmployeerOwnJob(clientId, submission.JobId))
                return Forbid();
            
            if (submission.Status != SubmissionStatus.Waiting)
                return Ok(new BaseResponse("Submission is not Waiting"));

            _jobSubmissionService.DeclineSubmission(id);
            
            return Ok();
        }
        
        [HttpGet]
        [AuthorizeRole(UserRole.Employeer)]
        [Route("{id}/accept")]
        public IActionResult Accept(long id)
        {
            if (!_jobSubmissionService.Exists(id))
                return NotFound(new BaseResponse("Submission Not Found!"));
            
            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var submission = _jobSubmissionService.Get(id);

            if (!_jobService.DoesEmployeerOwnJob(clientId, submission.JobId))
                return Forbid();
            
            if (_jobSubmissionService.AlreadyAcceptedOtherSubmit(submission.JobId))
                return Ok(new BaseResponse("Already Accepted Other Submission!"));
            
            if (submission.Status != SubmissionStatus.Waiting)
                return Ok(new BaseResponse("Submission is not Waiting"));
            
            _jobSubmissionService.AcceptSubmission(id);
            
            return Ok();
        }
    }
}