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
    [Authorize(Roles="Employeer,Admin")]
    [Route("api/v1/jobs")]
    public class JobsController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICrudService<Job> _crudService;
        private readonly IMapper _mapper;
        private readonly IJobService _jobService;
        private readonly IUserService _userService;
        
        public JobsController(
            IServiceProvider serviceProvider,
            IJobService jobService,
            IUserService userService)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetService<IMapper>();
            _crudService = _serviceProvider.GetService<ICrudService<Job>>();
            _jobService = jobService;
            _userService = userService;
        }
        
        [HttpPost]
        public IActionResult Create([FromBody] JobRequest model)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var job = _mapper.Map<Job>(model);     
            job.ClientId = _userService.GetByUserName(userName).Client.Id;
            var m = _crudService.Create(job);
            return Ok(new DataResponse<Job>(m));
        }

        [HttpDelete]
        public IActionResult Remove(long id)
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var user = _userService.GetByUserName(userName);

            if (user.Role == UserRole.Admin)
            {
                _crudService.Delete(id);
                return Ok();
            }

            var job = user.Client.Jobs.FirstOrDefault(x => x.Id == id);
            
            if (job == null) return Ok(new BaseResponse("You can only delete your jobs"));
            
            _crudService.Delete(id);
            return Ok();

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