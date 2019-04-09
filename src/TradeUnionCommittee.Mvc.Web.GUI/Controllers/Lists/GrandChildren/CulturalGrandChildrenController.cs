﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using TradeUnionCommittee.BLL.DTO.GrandChildren;
using TradeUnionCommittee.BLL.Enums;
using TradeUnionCommittee.BLL.Interfaces.Helpers;
using TradeUnionCommittee.BLL.Interfaces.Lists.GrandChildren;
using TradeUnionCommittee.BLL.Interfaces.SystemAudit;
using TradeUnionCommittee.Mvc.Web.GUI.Controllers.Directory;
using TradeUnionCommittee.Mvc.Web.GUI.Extensions;
using TradeUnionCommittee.ViewModels.ViewModels.GrandChildren;

namespace TradeUnionCommittee.Mvc.Web.GUI.Controllers.Lists.GrandChildren
{
    //ICulturalGrandChildrenService
    //CulturalGrandChildrenDTO
    //CreateCulturalGrandChildrenViewModel
    //UpdateCulturalGrandChildrenViewModel

    public class CulturalGrandChildrenController : Controller
    {
        private readonly ICulturalGrandChildrenService _services;
        private readonly IDirectories _directories;
        private readonly IMapper _mapper;
        private readonly ISystemAuditService _systemAuditService;
        private readonly IHttpContextAccessor _accessor;
        private readonly IReferenceParent _referenceParent;

        public CulturalGrandChildrenController(ICulturalGrandChildrenService services, IDirectories directories, IMapper mapper, ISystemAuditService systemAuditService, IHttpContextAccessor accessor, IReferenceParent referenceParent)
        {
            _services = services;
            _mapper = mapper;
            _systemAuditService = systemAuditService;
            _accessor = accessor;
            _referenceParent = referenceParent;
            _directories = directories;
        }

        //------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Authorize(Roles = "Admin,Accountant,Deputy")]
        public async Task<IActionResult> Index([Required] string id)
        {
            var result = await _services.GetAllAsync(id);
            var referenceParent = _referenceParent.GetHashIdEmployee(id, ReferenceParentType.GrandChildren);
            if (result.IsValid && referenceParent.IsValid)
            {
                ViewData["HashIdGrandChildren"] = id;
                ViewData["HashIdEmployee"] = referenceParent.Result;
                return View(result.Result);
            }
            var errorsList = new List<string>();
            errorsList.AddRange(result.ErrorsList);
            errorsList.AddRange(referenceParent.ErrorsList);
            TempData["ErrorsList"] = errorsList;
            return View();
        }

        //------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Authorize(Roles = "Admin,Accountant,Deputy")]
        public async Task<IActionResult> Create([Required] string id)
        {
            ViewBag.Cultural = await _directories.GetCultural();
            return View(new CreateCulturalGrandChildrenViewModel { HashIdGrandChildren = id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Accountant,Deputy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCulturalGrandChildrenViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.CreateAsync(_mapper.Map<CulturalGrandChildrenDTO>(vm));
                if (result.IsValid)
                {
                    await _systemAuditService.AuditAsync(User.GetEmail(), _accessor.GetIp(), Operations.Insert, Tables.CulturalGrandChildrens);
                    return RedirectToAction("Index", new { id = vm.HashIdGrandChildren });
                }
                TempData["ErrorsList"] = result.ErrorsList;
            }
            ViewBag.Cultural = await _directories.GetCultural();
            return View(vm);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Authorize(Roles = "Admin,Accountant,Deputy")]
        public async Task<IActionResult> Update([Required] string id)
        {
            var result = await _services.GetAsync(id);
            if (result.IsValid)
            {
                ViewBag.Cultural = await _directories.GetCultural(result.Result.HashIdCultural);
                return View(_mapper.Map<UpdateCulturalGrandChildrenViewModel>(result.Result));
            }
            TempData["ErrorsList"] = result.ErrorsList;
            return View();
        }

        [HttpPost, ActionName("Update")]
        [Authorize(Roles = "Admin,Accountant,Deputy")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCulturalGrandChildrenViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _services.UpdateAsync(_mapper.Map<CulturalGrandChildrenDTO>(vm));
                if (result.IsValid)
                {
                    await _systemAuditService.AuditAsync(User.GetEmail(), _accessor.GetIp(), Operations.Update, Tables.CulturalGrandChildrens);
                    return RedirectToAction("Index", new { id = vm.HashIdGrandChildren });
                }
                TempData["ErrorsListConfirmed"] = result.ErrorsList;
            }
            ViewBag.Cultural = await _directories.GetCultural(vm.HashIdCultural);
            return View(vm);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([Required] string id)
        {
            var result = await _services.GetAsync(id);
            if (result.IsValid)
            {
                return View(result.Result);
            }
            TempData["ErrorsList"] = result.ErrorsList;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed([Required] string hashId, [Required] string hashIdGrandChildren)
        {
            var result = await _services.DeleteAsync(hashId);
            if (result.IsValid)
            {
                await _systemAuditService.AuditAsync(User.GetEmail(), _accessor.GetIp(), Operations.Delete, Tables.CulturalGrandChildrens);
                return RedirectToAction("Index", new { id = hashIdGrandChildren });
            }
            TempData["ErrorsList"] = result.ErrorsList;
            return View();
        }

        //------------------------------------------------------------------------------------------------------------------------------------------

        protected override void Dispose(bool disposing)
        {
            _services.Dispose();
            base.Dispose(disposing);
        }
    }
}