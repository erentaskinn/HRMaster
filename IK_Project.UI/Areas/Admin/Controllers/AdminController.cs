using AutoMapper;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.AdminSevice;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.UserVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace IK_Project.UI.Areas.Admin.Controllers
{

    public class AdminController : AdminBaseController
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;
        private readonly IAccountService accountService;
        IWebHostEnvironment _webHostEnvironment;


		public AdminController(IAdminService adminService, IMapper mapper, IWebHostEnvironment webHostEnvironment, IAccountService accountService)
		{
			_adminService = adminService;
			_mapper = mapper;
			_webHostEnvironment = webHostEnvironment;
			this.accountService = accountService;
		}
		public IActionResult Index()
        {
            
            return View();
        }
		[HttpGet]
		public async Task<IActionResult> ListAllAdmin()
		{
			var adminList=await _adminService.AllAdminsAsync();
			if (!adminList.IsSuccess)
			{
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
			var adminListVm=_mapper.Map<List<AdminAdminListVM>>(adminList.Data);
			return View(adminListVm);
		}

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //AdminAdminCreateVM model = new AdminAdminCreateVM();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminAdminCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Password = "BilgeAdamHS-10";
            var adminDto = _mapper.Map<AdminCreateDTO>(model);
            var addAdminResult = await _adminService.CreateAsync(adminDto);
            if (addAdminResult.IsSuccess)
            {
                Console.WriteLine($"{model.Name} {model.LastName} Admin olarak eklendi");
                return RedirectToAction("Index", "Home", new { area = "Admin" });

                //return RedirectToAction(nameof(Index));
            }
            Console.WriteLine("Admin Eklemede hata oluştu" + addAdminResult.Message);
            return View(model);
        }


		public async Task<IActionResult> Edit(Guid id)
		{
			var user = await accountService.GetUserAsync(User);
			var result = await _adminService.GetAsync(user.Id);
			if (!result.IsSuccess)
			{
				return RedirectToAction(nameof(Index));
			}
			var adminUpdateVM = _mapper.Map<AdminAdminUpdateVM>(result.Data);

			return View(adminUpdateVM);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(AdminAdminUpdateVM adminAdminUpdateVM)
		{
			var user = await accountService.GetUserAsync(User);
			var result = await _adminService.GetAsync(user.Id);
			adminAdminUpdateVM.Id = result.Data.Id;
			if (!ModelState.IsValid)
			{
				return View(adminAdminUpdateVM);
			}
			var resultdto = await _adminService.EditAsync(_mapper.Map<AdminAdminUpdateDTO>(adminAdminUpdateVM));
			if (!resultdto.IsSuccess)
			{
				return RedirectToAction(nameof(Index));
			}
			return RedirectToAction("Index", "Home", new { area = "Admin" });
		}
	}
}
