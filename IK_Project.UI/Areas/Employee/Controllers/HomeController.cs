using AutoMapper;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Domain.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.UI.Areas.CompanyManager.Models;
using IK_Project.UI.Areas.Employee.Models;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Services.DepartmantService;
using Microsoft.Identity.Client;
using IK_Project.Application.Services.AccountService;
using Humanizer;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.Admin.Models.ViewModels;
using Newtonsoft.Json;
using System.Net;

namespace IK_Project.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        IWebHostEnvironment _webHostEnvironment;
        readonly private ICompanyService _companyService;
        private readonly IDepartmantService _departmantService;
        private readonly IAccountService _accountService;

        public HomeController(IMapper mapper, IEmployeeService employeeService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, ICompanyService companyService, IAccountService accountService)

        {
            _mapper = mapper;
            _employeeService = employeeService;
            _signInManager = signInManager;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _companyService = companyService;
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            //if (_signInManager.IsSignedIn(HttpContext.User))
            //{
            //  var employeeList=  await _employeeService.GetDefaults(x => x.Name == HttpContext.User.Identity.Name);
            //    var employee=employeeList.Data.FirstOrDefault();
            //    return View(employee);
            //}
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProfileDetails()
        {


            var user = await _accountService.GetUserAsync(User);
            var employeeDto = await _employeeService.GetAsync(user.Id);

            var employee = _mapper.Map<EmployeeVM>(employeeDto.Data);

            return View(employee);


        }

        [HttpPost]
        public async Task<IActionResult> ProfileDetails(EmployeeVM vm)
        {


            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _accountService.GetUserAsync(User);
            var result = await _employeeService.GetAsync(user.Id);
            if (!result.IsSuccess)
            {
                TempData["error"] = "Error occured";
                return View(vm);
            }
            else
            {
                vm.Id = result.Data.Id;
                var editDto = _mapper.Map<EmployeeUpdateDTO>(vm);
                editDto.DepartmantId = result.Data.DepartmantId;

                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    editDto.ProfilePhoto = savedFilePath;

                }
                await _employeeService.Edit(editDto);
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
