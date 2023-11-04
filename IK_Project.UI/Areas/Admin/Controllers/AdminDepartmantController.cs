using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.UI.Areas.CompanyManager.Models.Departmant;
using IK_Project.UI.Areas.CompanyManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.DepartmantVMs;
using Microsoft.AspNetCore.Authorization;
using IK_Project.Application.Services.EmailSenderService;

namespace IK_Project.UI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminDepartmantController : Controller
    {
        private readonly IDepartmantService _departmantService;
        private readonly ICompanyManagerService _cmService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICompanyService _companyService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEMailSenderService _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AdminDepartmantController(IDepartmantService departmantService, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICompanyService companyService, UserManager<IdentityUser> userManager, IEMailSenderService emailSender, SignInManager<IdentityUser> signInManager, ICompanyManagerService cmService)
        {
            ; _departmantService = departmantService;
            _mapper = mapper;
            this._webHostEnvironment = webHostEnvironment;
            _companyService = companyService;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _cmService = cmService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {

                //var DpVm = _mapper.Map<List<AdminDepartmantListVM>>(await _departmantService.GettAllActives());
                return View(/*DpVm*/);

            }
            else
            {
                return View();
            }

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
               

                return View();
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(AdminDepartmantCreateVM adminDepartmantCreateVM)
        {
            var createdDp = _mapper.Map<DepartmantCreateDTO>(adminDepartmantCreateVM);
            await _departmantService.Create(createdDp);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var updateDto = await _departmantService.GetById(id);
            var updateVm = _mapper.Map<AdminDepartmantUpdateVM>(updateDto);
            return View(updateVm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AdminDepartmantUpdateVM departmantUpdateVM)
        {
            var updatedDto = _mapper.Map<DepartmantUpdateDTO>(departmantUpdateVM);
            await _departmantService.Edit(updatedDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _departmantService.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
