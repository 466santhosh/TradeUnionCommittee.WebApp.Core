﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TradeUnionCommittee.BLL.DTO;
using TradeUnionCommittee.BLL.Enums;
using TradeUnionCommittee.BLL.Interfaces.Directory;
using TradeUnionCommittee.BLL.Interfaces.SystemAudit;
using TradeUnionCommittee.ViewModels.ViewModels;

namespace TradeUnionCommittee.Web.Api.Controllers.Directory
{
    [Route("api/[controller]")]
    [ApiController]
    public class CulturalController : ControllerBase
    {
        private readonly ICulturalService _services;
        private readonly ISystemAuditService _systemAuditService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public CulturalController(ICulturalService services, ISystemAuditService systemAuditService, IMapper mapper, IHttpContextAccessor accessor)
        {
            _services = services;
            _systemAuditService = systemAuditService;
            _mapper = mapper;
            _accessor = accessor;
        }

        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "Admin,Accountant,Deputy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _services.GetAllAsync());
        }

        [HttpGet]
        [Route("Get/{id}")]
        [Authorize(Roles = "Admin,Accountant,Deputy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get([Required] string id)
        {
            var result = await _services.GetAsync(id);
            if (result.IsValid)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "Admin,Accountant,Deputy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] CreateCulturalViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.CreateAsync(_mapper.Map<DirectoryDTO>(vm));
                if (result.IsValid)
                {
                    await _systemAuditService.AuditAsync(User.Identity.Name, _accessor.HttpContext.Connection.RemoteIpAddress.ToString(), Operations.Insert, Tables.Cultural);
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = "Admin,Accountant,Deputy", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Update([FromBody] UpdateCulturalViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.UpdateAsync(_mapper.Map<DirectoryDTO>(vm));
                if (result.IsValid)
                {
                    await _systemAuditService.AuditAsync(User.Identity.Name, _accessor.HttpContext.Connection.RemoteIpAddress.ToString(), Operations.Update, Tables.Cultural);
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Delete([Required] string id)
        {
            var result = await _services.DeleteAsync(id);
            if (result.IsValid)
            {
                await _systemAuditService.AuditAsync(User.Identity.Name, _accessor.HttpContext.Connection.RemoteIpAddress.ToString(), Operations.Delete, Tables.Cultural);
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}