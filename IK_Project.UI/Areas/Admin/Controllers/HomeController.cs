using AutoMapper;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Services.AdminSevice;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.UI.Areas.Admin.Models.ViewModels;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.UserVMs;
using IK_Project.UI.Areas.CompanyManager.Controllers;
using IK_Project.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Net;
using System.Web;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyManager;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Domain.Enums;

namespace IK_Project.UI.Areas.Admin.Controllers
{

    public class HomeController : AdminBaseController
    {

        readonly private IMapper _mapper;
        readonly private IAdminService _adminService;
        readonly private ICompanyService _companyService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        IWebHostEnvironment _webHostEnvironment;
        private readonly IEMailSenderService _emailSender;
        private readonly IAccountService _accountService;

        public HomeController(IMapper mapper, IAdminService adminService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IEMailSenderService emailSender, ICompanyService companyService, IAccountService accountService)

        {
            _mapper = mapper;
            _adminService = adminService;
            _signInManager = signInManager;
            _userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
            _companyService = companyService;
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM2 vm)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(vm.Email);
                if (user != null)
                {

                    var response = Request.Form["g-recaptcha-response"];
                    const string secret = "6LchLDAoAAAAALq1q5z5T8HQDgHuJ81udAwKWXca";
                    var client = new WebClient();
                    var reply =
                        client.DownloadString(
                            string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
                    var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

                    if (!captchaResponse.Success)
                        TempData["Message"] = "Lütfen güvenliği doğrulayınız.";
                    else
                    {
                        var _loginDto = _mapper.Map<LoginDTO>(vm);

                        //var result = await _appUserService.Login(_loginDto);
                        //if (result.Succeeded)
                        //{
                        //    var roles = await _userManager.GetRolesAsync(user);
                        //    if (roles.Contains("Admin"))
                        //    {
                        //        return RedirectToAction("Index", "Home", new { area = "Admin" });

                        //    }
                        //    //else if (roles.Contains("CompanyManager"))
                        //    //{
                        //    //    if (user.PasswordChangeRequired)
                        //    //    {

                        //    //        return RedirectToAction("ChangePassword", "Home", new { area = "Admin" });

                        //    //    }
                        //    //    return RedirectToAction("Index", "Home", new { area = "CompanyManager" });



                        //    //}
                        //    else if (roles.Contains("Employee"))
                        //    {
                        //        if (user.PasswordChangeRequired)
                        //        {
                        //            // Kullanıcı şifre değiştirmesi gerektiğinde şifre değiştirme sayfasına yönlendir
                        //            user.PasswordChangeRequired = false;
                        //            await _userManager.UpdateAsync(user);
                        //            return RedirectToAction("ChangePassword", "Home", new { area = "Admin" });

                        //        }
                        //        return RedirectToAction("Index", "Home", new { area = "Employee" });


                        //    }
                        //    else
                        //    {
                        //        TempData["error"] = "Login failed.";
                        //        return View(vm);
                        //    }
                        //}


                    }
                }



                TempData["error"] = "Login failed.";
                return View(vm);
            }
            return View(vm);

        }



        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            //await _appUserService.LogOut();

            return RedirectToAction("Login", "Home");
        }

        [AllowAnonymous]//Bu attribute sayesinde ilgili action methodun Authorize kapsamından çıkmasını istiyoruz. Neden? Çünkü kullanıcı herhangi bir kimlik doğrulamasından yani authentication işleminden geçmeden Register sayfasını görebilmeili ve sisteme register olabilmelidir.
        public IActionResult Register()
        {


            if (User.Identity.IsAuthenticated)//Kullanıcı giriş yapmışsa anasayfaya yönlendirilsin.
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterVM vm)
        {

            if (ModelState.IsValid)
            {
                //var user = _mapper.Map<RegisterDTO>(vm);
                ////var result = await _appUserService.Register(user);

                //if (result.Succeeded)
                //{
                //    return RedirectToAction("Index", "Home", new { area = "" });
                //}
                //foreach (var item in result.Errors)
                //{

                //    ModelState.AddModelError(item.Code, item.Description);
                //    TempData["error"] = "Kayıt Oluşturulurken Bir Hata Meydana Geldi";
                //}
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ProfileDetails()
        {
            var user = await _accountService.GetUserAsync(User);
            var result = await _adminService.GetAsync(user.Id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            var adminadminupdatevm = _mapper.Map<AdminAdminUpdateVM>(result.Data);
            return View(adminadminupdatevm);


        }

        [HttpPost]
        public async Task<IActionResult> ProfileDetails(AdminAdminUpdateVM vm)
        {

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _accountService.GetUserAsync(User);
            var result = await _adminService.GetAsync(user.Id);
            if (!result.IsSuccess)
            {
                TempData["error"] = "Error occured";
                return View(vm);
            }
            else
            {
                vm.Id = result.Data.Id;
                var editDto = _mapper.Map<AdminAdminUpdateDTO>(vm);
                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    editDto.ProfilePhoto = savedFilePath;

                }
                await _adminService.EditAsync(editDto);
                return RedirectToAction("Index");
            }

        }

        [AllowAnonymous]
        public IActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel vm)
        {
            //AppUser user = await _userManager.FindByEmailAsync(vm.Email);
            //if (user != null)
            //{
            //    string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            //    string mailBody = $"<a target=\"_blank\"href=\"http://localhost:5254{Url.Action("UpdatePassword", "Home", new { userId = user.Id, token = HttpUtility.UrlEncode(resetToken) })}\"> Click to update your password </a>";
            //    string subject = "Update Password";
            //    await _emailSender.SendEmailAsync(user.Email, subject, mailBody);
            //    ViewBag.State = true;
            //}

            //else
            //{
            //    ViewBag.State = false;
            //}

            return View();


        }

        //[HttpGet("[action]/{userId}/{token}")]
        //public IActionResult UpdatePassword(string userId, string token)
        //{
        //    return View();
        //}

        //[HttpPost("[action]/{userId}/{token}")]
        //public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel vm, string userId, string token)
        //{
        //    //AppUser user = await _userManager.FindByIdAsync(userId);
        //    //IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), vm.Password);
        //    //if (result.Succeeded)
        //    //{
        //    //    ViewBag.State = true;
        //    //    await _userManager.UpdateSecurityStampAsync(user);
        //    //}
        //    //else
        //    //{
        //    //    ViewBag.State = false;
        //    //}

        //    return View();
        //}

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(UI.Areas.Admin.Models.ViewModels.UserVMs.ChangePasswordVM model)
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