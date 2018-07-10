using System;
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
    [AuthorizeRole(UserRole.Admin)]
    public class BaseController<T, TReq> : ControllerBase where T : AbstractEntityModel
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICrudService<T> _crudService;
        protected readonly IMapper _mapper;

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
            return CreatedAtAction(nameof(Get), new {id = m.Id}, m);
        }

        [HttpDelete]
        public virtual IActionResult Remove(long id)
        {
            _crudService.Delete(id);
            return NoContent();
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