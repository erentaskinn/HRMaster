using AutoMapper;
using Humanizer;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.AdminSevice;
using IK_Project.Application.Services.AdvanceService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.Application.Services.ExpenseService;
using IK_Project.Application.Services.PermissionService;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.Admin.Models.ViewModels;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyManager;
using IK_Project.UI.Areas.DepartmantManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace IK_Project.UI.Areas.DepartmantManager.Controllers
{
    
    public class HomeController : BaseController
    {
        readonly private IMapper _mapper;
        readonly private ICompanyManagerService _cmService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        IWebHostEnvironment _webHostEnvironment;
        IExpenseService _expenseService;
        IAdvanceService _advanceService;
        IPermissionService _permissionService;
        private readonly ICompanyService _companyService;
        private readonly IAccountService _accountService;
        private readonly IAdminService _adminService;
        private readonly IDepartmantManagerService _departmantManagerService;
        


        public HomeController(IMapper mapper, ICompanyManagerService cmService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IExpenseService expenseService,
        IAdvanceService advanceService,
        IPermissionService permissionService, ICompanyService companyService, IAccountService accountService, IAdminService adminService, IDepartmantManagerService departmantManagerService)

        {
            _accountService = accountService;
            _mapper = mapper;
            _cmService = cmService;
            _signInManager = signInManager;
            _userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
            _advanceService = advanceService;
            _expenseService = expenseService;
            _permissionService = permissionService;
            _companyService = companyService;
            _adminService = adminService;
            _departmantManagerService = departmantManagerService;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        public async Task< IActionResult> ProfileDetails()
        {
            var user = await _accountService.GetUserAsync(User);
            var result = await _departmantManagerService.GetAsync(user.Id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            var departmantManager = _mapper.Map<DepartmantManagerUpdateVM>(result.Data);
            return View(departmantManager);
        }
        [HttpPost]
        public async Task<IActionResult> ProfileDetails(DepartmantManagerUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _accountService.GetUserAsync(User);
            var result = await _departmantManagerService.GetAsync(user.Id);
          
            if (!result.IsSuccess)
            {
                TempData["error"] = "Error occured";
                return View(vm);
            }
            else
            {
                vm.Id = result.Data.Id;
                
                var editDto = _mapper.Map<DepartmantManagerUpdateDTO>(vm);
                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    editDto.ProfilePhoto = savedFilePath;

                }
                await _departmantManagerService.Edit(editDto);
                return RedirectToAction("Index");
            }
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UI.Models.ChangePasswordVM model)
        {
            if (ModelState.IsValid)
            {
                var response = Request.Form["g-recaptcha-response"];
                const string secret = "6LchLDAoAAAAALq1q5z5T8HQDgHuJ81udAwKWXca";
                var client = new WebClient();
                var reply =
                    client.DownloadString(
                        string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
                var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                if (!captchaResponse.Success)
                {
                    TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
                    return View(model);
                }
                else
                {
                    //return RedirectToAction("Index", "Home", new { Area = userRole[0].ToString() });

                    var user = await _userManager.GetUserAsync(User);
                    if (user != null)
                    {
                        var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                        if (changePasswordResult.Succeeded)
                        {

                            var roles = await _userManager.GetRolesAsync(user);

                            if (roles.Contains("CompanyManager"))
                            {
                                await _accountService.RemovePasswordChangeClaimAsync(user, Roles.CompanyManager.ToString());

                                return RedirectToAction("Index", "Home", new { area = "CompanyManager" });

                            }
                            else if (roles.Contains("Employee"))
                            {
                                await _accountService.RemovePasswordChangeClaimAsync(user, Roles.Employee.ToString());

                                return RedirectToAction("Index", "Home", new { area = "Employee" });

                            }
                            else if (roles.Contains("DepartmantManager"))
                            {
                                await _accountService.RemovePasswordChangeClaimAsync(user, Roles.DepartmantManager.ToString());
                                return RedirectToAction("Index", "Home", new { area = "DepartmantManager" });

                            }
                            else
                            {
                                TempData["error"] = "Login failed.";
                                return View(model);
                            }
                        }
                        else
                        {


                            foreach (var error in changePasswordResult.Errors)
                            {
                                if (error.Code == "PasswordRequiresDigit" || error.Code == "PasswordRequiresUpper" || error.Code == "PasswordRequiresNonAlphanumeric" || error.Code == "PasswordTooShort")
                                {
                                    // Eğer hata şifre gereksinimleriyle ilgiliyse, bu durumda yeni şifre gereksinimlerini karşılamadığını belirtin.
                                    ModelState.AddModelError("NewPassword", "Your password must contain at least one uppercase letter, one special character, one digit, and be at least 8 characters long.");
                                }
                                else if (error.Description == "Incorrect password.")
                                {
                                    // Eğer hata "Incorrect password" ise, bu hatayı sadece "OldPassword" alanına ekleyin.
                                    ModelState.AddModelError("OldPassword", "Your old password is incorrect");
                                }
                                else
                                {
                                    // Diğer hataları ModelState'e ekleyin
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                            }

                        }
                    }
                }
            }

            return View(model);
        }

    }
}
