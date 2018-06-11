using System;
using System.Collections.Generic;
using System.Security.Claims;
using ArmLancer.API.Models.Requests;
using ArmLancer.API.Models.Responses;
using ArmLancer.API.Utils.Attributes;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ArmLancer.API.Controllers
{
    [AuthorizeRole(UserRole.FreeLancer)]
    [Route("api/v1/favorites")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IMapper _mapper;
        
        public FavoritesController(IServiceProvider serviceProvider)
        {
            _favoriteService = serviceProvider.GetService<IFavoriteService>();
            _mapper = serviceProvider.GetService<IMapper>();
        }

        [HttpGet]
        public IActionResult GetList()
        {
            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var favorites = _favoriteService.GetByClientId(clientId);
            return Ok(new DataResponse<IEnumerable<Favorite>>(favorites));
        }

        [HttpGet]
        public IActionResult Get(long id)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var m = _favoriteService.Get(id);
            
            if (m == null)
                return NotFound(new BaseResponse("Favorite Not Found!"));
            
            if (m.ClientId != long.Parse(clientId))
                return Forbid();
            
            return Ok(new DataResponse<Favorite>(m));
        }

        [HttpPost]
        public IActionResult Create(FavoriteRequest model)
        {
            var clientId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var map = _mapper.Map<Favorite>(model);
            map.ClientId = long.Parse(clientId);
            var m = _favoriteService.Create(map);
            return CreatedAtAction(nameof(Get), m);
        }

        [HttpDelete]
        public IActionResult Remove(long id)
        {
            var clientId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!_favoriteService.Exists(id))
                return NotFound(new BaseResponse("Favorite Not Found!"));
            
            if (!_favoriteService.DoesFreelancerOwnFavorite(clientId, id))
                return Forbid();

            _favoriteService.Delete(id);

            return Ok();
        }
    }
}