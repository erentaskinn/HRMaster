using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IK_Project.UI.Models;
using IK_Project.Application.Services.MenuService;
using AutoMapper;
using IK_Project.UI.Areas.Admin.Models.ViewModels.MenuVMs;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Services.AppUserService;

namespace IK_Project.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMenuService _menuService;
    private readonly IMapper _mapper;
    private readonly IAppUserService _appUserService;


    public HomeController(ILogger<HomeController> logger, IMenuService menuService, IMapper mapper, IAppUserService appUserService)
    {
        _logger = logger;
        _menuService = menuService;
        _mapper = mapper;
        _appUserService = appUserService;
    }

    public async Task<IActionResult> Index()
    {
        //var listDTO = await _menuService.GetDefaults(x => x.IsActive == true);
        //var menuList = _mapper.Map<List<AdminMenuListVM>>(listDTO.Data);
        //return View(menuList);
        //return RedirectToAction("Create", "Admin", new { area = "Admin" });
        //return View();
        // return RedirectToAction("Index", "Home");
        //return RedirectToAction("Login", "Account");
        var listDTO = await _menuService.GetDefaults(x => x.IsActive == true);

        var combinedViewModel = new IndexCombineViewModel
        {
            UserInformation = new AppUserInformationVM(),
            //MenuList = new List<AdminMenuListVM>(),
            MenuList = _mapper.Map<List<AdminMenuListVM>>(listDTO.Data),// ListViewModel'i doldurun

        };

        return View(combinedViewModel);
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(IndexCombineViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
      
        var appUserInformationDto = _mapper.Map<AppUserInformationDTO>(model.UserInformation);
        var result = await _appUserService.CreateAsync(appUserInformationDto);
        if (result.IsSuccess)
        {
            return RedirectToAction("Index", "Home");

        }
        return View(model);
    }
    public IActionResult GetOffer()
    {
        return View("GetOffer");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
