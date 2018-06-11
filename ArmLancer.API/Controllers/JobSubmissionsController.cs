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
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;
        private readonly IJobSubmissionService _jobSubmissionService;

        public JobSubmissionsController(
            IServiceProvider serviceProvider,
            IJobService jobService,
            IJobSubmissionService jobSubmissionService)
        {
            _mapper = serviceProvider.GetService<IMapper>();
            _jobSubmissionService = jobSubmissionService;
            _jobService = jobService;
        }

        /// <summary>
        /// Get List Of Submissions For Job
        /// For Employeers return all submissions
        /// For FreeLancers return only owned submissions
        /// </summary>
        /// <param name="id">Job ID</param>
        /// <returns>Submission List</returns>
        [HttpGet]
        [Authorize]
        [Route("~/api/v1/jobs/{id}/submissions")]
        public IActionResult GetList(long id)
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

            if (!_jobService.Exists(id))
                return NotFound(new BaseResponse("Job Not Found!"));

            // if User is Admin Return all submissions.
            if (userRole == UserRole.Admin)
            {
                var response = _mapper.Map<JobSubmissionResponse>(_jobSubmissionService.GetByJobId(id).ToList());
                return Ok(new DataResponse<JobSubmissionResponse>(response));
            }

            // if User is Employeer returns submissions only for his jobs.
            if (userRole == UserRole.Employeer)
            {
                var job = _jobService.Get(id);

                if (job.ClientId != clientId)
                    return Forbid();

                var response = _mapper.Map<JobSubmissionResponse>(_jobSubmissionService.GetByJobId(id).ToList());
                return Ok(new DataResponse<JobSubmissionResponse>(response));
            }

            // if User is FreeLancer return only his submissions.
            var submission = _jobSubmissionService.GetByClientAndJobId(clientId, id);
            return Ok(new DataResponse<JobSubmissionResponse>(_mapper.Map<JobSubmissionResponse>(submission)));
        }

        /// <summary>
        /// Create a New Job Submission
        /// </summary>
        /// <param name="model">Job Submission Request model</param>
        /// <returns>Created (201) response with a Location header</returns>
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
            var m = _jobSubmissionService.Create(submission);

            return CreatedAtAction(nameof(GetList), new {id = m.JobId},
                new DataResponse<JobSubmissionResponse>(_mapper.Map<JobSubmissionResponse>(m)));
        }

        /// <summary>
        /// Cancel Already Sent Job Submission
        /// </summary>
        /// <param name="id">Submission ID</param>
        /// <returns>200 OK Result</returns>
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

        /// <summary>
        /// Decline Job Submission Sent By FreeLancer
        /// </summary>
        /// <param name="id">Submission ID</param>
        /// <returns>200 OK Result</returns>
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

        /// <summary>
        /// Accept Submission Sent By FreeLancer
        /// </summary>
        /// <param name="id">Submission ID</param>
        /// <returns>200 OK Result</returns>
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