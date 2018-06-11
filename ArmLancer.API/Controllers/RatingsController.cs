using System;
using ArmLancer.API.Models.Requests;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArmLancer.API.Controllers
{
    [Authorize]
    [Route("api/v1/ratings")]
    public class RatingsController: BaseController<Rating, RatingRequest>
    {
        private readonly IRatingService _ratingService;
        
        public RatingsController(
            IServiceProvider serviceProvider,
            IRatingService ratingService) : base(serviceProvider)
        {
            _ratingService = ratingService;
        }
    }
}