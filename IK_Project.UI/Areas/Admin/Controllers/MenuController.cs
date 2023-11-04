using AutoMapper;
using IK_Project.Application.Models.DTOs.AppRoleDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Application.Services.MenuService;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.Admin.Models.ViewModels.MenuVMs;
using IK_Project.UI.Areas.Admin.Models.ViewModels.RoleVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IK_Project.UI.Areas.Admin.Controllers
{

    public class MenuController : AdminBaseController
    {

        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuController(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllMenus()
        {
            var listDTO = await _menuService.AllMenus();
            var listVM = _mapper.Map<List<AdminMenuListVM>>(listDTO.Data);

            return View(listVM);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminMenuCreateVM vm)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var dto = _mapper.Map<MenuCreateDTO>(vm);
                    await _menuService.Create(dto);
                    return RedirectToAction("GetAllMenus");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }

            }
            return View(vm);
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _menuService.Remove(id);
                return RedirectToAction("GetAllMenus");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("GetAllMenus");

            }

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var updateDto = await _menuService.GetById(id);
            var updateVm = _mapper.Map<AdminMenuUpdateVM>(updateDto.Data);

            return View(updateVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminMenuUpdateVM vm)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var updateDto = _mapper.Map<MenuUpdateDTO>(vm);
                    await _menuService.Edit(updateDto);
                    return RedirectToAction("GetAllMenus");
                }
                catch (Exception ex)
                {

                    TempData["error"] = ex.Message;
                }
            }

            return View(vm);
        }
        [HttpPost]

        public async Task<IActionResult> UpdateMenuStatus(Guid menuId, bool newStatus)
        {

            var menuUpdateDto = await _menuService.GetById(menuId);
          
            await _menuService.EditToggle(menuUpdateDto.Data, newStatus);
            return View();

        }


    }
}
