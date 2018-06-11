using System;
using ArmLancer.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ArmLancer.API.Controllers
{
    [Authorize]
    [Route("api/v1/ratings")]
    public class RatingsController: ControllerBase
    {
        private readonly IRatingService _ratingService;
        
        public RatingsController(IServiceProvider serviceProvider)
        {
            _ratingService = serviceProvider.GetService<IRatingService>();
        }
    }
}