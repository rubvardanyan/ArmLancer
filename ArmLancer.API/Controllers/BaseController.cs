using System;
using ArmLancer.API.Models.Responses;
using ArmLancer.Core.Interfaces;
using ArmLancer.Data.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ArmLancer.API.Controllers
{
    public class BaseController<T, TReq> : ControllerBase where T : AbstractEntityModel
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICrudService<T> _crudService;
        private readonly IMapper _mapper;

        public BaseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _mapper = _serviceProvider.GetService<IMapper>();
            _crudService = _serviceProvider.GetService<ICrudService<T>>();
        }

        [HttpPost]
        public virtual IActionResult Create([FromBody] TReq model)
        {
            var m = _crudService.Create(_mapper.Map<T>(model));
            return Ok(new DataResponse<T>(m));
        }

        [HttpDelete]
        public virtual IActionResult Remove(long id)
        {
            _crudService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public virtual IActionResult Get(long id)
        {
            var m = _crudService.Get(id);
            if (m == null)
            {
                return NotFound();
            }

            return Ok(new DataResponse<T>(m));
        }
    }
}