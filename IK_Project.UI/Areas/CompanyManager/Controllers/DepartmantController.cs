using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs;
using IK_Project.UI.Areas.CompanyManager.Models;
using IK_Project.UI.Areas.CompanyManager.Models.Departmant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class DepartmantController : CompanyManagerBaseController
    {
        private readonly IDepartmantService _departmantService;
        private readonly ICompanyManagerService _cmService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICompanyService _companyService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEMailSenderService _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAccountService _accountService;
        public DepartmantController(IDepartmantService departmantService, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICompanyService companyService, UserManager<IdentityUser> userManager, IEMailSenderService emailSender, SignInManager<IdentityUser> signInManager, ICompanyManagerService cmService, IAccountService accountService)
        {
            _departmantService = departmantService;
            _mapper = mapper;
            this._webHostEnvironment = webHostEnvironment;
            _companyService = companyService;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _cmService = cmService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<CompanyManagerDepartmantListVM> companyManagerDepartmantListVM = new List<CompanyManagerDepartmantListVM>();
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            if (!departmants.IsSuccess)
            {
                return View(companyManagerDepartmantListVM);
            }
            companyManagerDepartmantListVM = _mapper.Map<List<CompanyManagerDepartmantListVM>>(departmants.Data);
            return View(companyManagerDepartmantListVM);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CompanyManagerDepartmantCreateVM createVM = new CompanyManagerDepartmantCreateVM();
            return View(createVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CompanyManagerDepartmantCreateVM departmantCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmantCreateVM);
            }
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            departmantCreateVM.CompanyId = company.Data.Id;
            var departmans = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            if (departmans.IsSuccess)
            {
                foreach (var departmanName in departmans.Data)
                {
                    if (departmanName.Name == departmantCreateVM.Name)
                    {
                        departmantCreateVM.Name = "sameName";
                        return View(departmantCreateVM);
                    }
                }
            }
            var createdDp = _mapper.Map<DepartmantCreateDTO>(departmantCreateVM);
            await _departmantService.Create(createdDp);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var updateDto = await _departmantService.GetById(id);
            var updateVm = _mapper.Map<CompanyManagerDepartmantUpdateVM>(updateDto.Data);
            return View(updateVm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CompanyManagerDepartmantUpdateVM departmantUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(departmantUpdateVM);
            }
            var deparmantUpdatedDTO = _mapper.Map<DepartmantUpdateDTO>(departmantUpdateVM);
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmans = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            deparmantUpdatedDTO.CompanyId = company.Data.Id;
            if (departmans.IsSuccess)
            {
                foreach (var departmanName in departmans.Data)
                {
                    if (departmanName.Name == departmantUpdateVM.Name)
                    {
                        departmantUpdateVM.Name = "sameName";
                        return View(departmantUpdateVM);
                    }
                }
            }
            await _departmantService.Edit(deparmantUpdatedDTO);
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
