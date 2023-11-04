using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.UI.Areas.CompanyManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyVM;
using Elfie.Serialization;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyManager;
using IK_Project.Application.Services.AccountService;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class CompanyController : CompanyManagerBaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<IdentityUser> _signInManager;
        readonly private ICompanyManagerService _cmService;
        private readonly IAccountService _accountService;


        public CompanyController(ICompanyService companyService, IMapper mapper, IWebHostEnvironment webHostEnvironment, SignInManager<IdentityUser> signInManager, ICompanyManagerService cmService, IAccountService accountService)
        {
            _companyService = companyService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _signInManager = signInManager;
            _cmService = cmService;
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var user = await _accountService.GetUserAsync(User);
            var result = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == result.Data.Id);

            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            var companyManagerCompanyUpdateVM = _mapper.Map<CompanyManagerCompanyUpdateVM>(company.Data);
            return View(companyManagerCompanyUpdateVM);            
        }

        [HttpPost]
        public async Task<IActionResult> Details(CompanyManagerCompanyUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            

            var editDto = _mapper.Map<CompanyUpdateDTO>(vm);
            if (vm.LogoFile != null)
            {
                string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                editDto.Logo = savedFilePath;

            }
            await _companyService.Edit(editDto);
            return RedirectToAction("Index", "Home");
        }
    }
}
