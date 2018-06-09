using System;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArmLancer.API.Controllers
{
    [Authorize]
    [Route("api/v1/ratings")]
    public class RatingsController: ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICrudService<Rating> _crudService;
        private readonly IMapper _mapper;
    }
}