using IK_Project.UI.Areas.Admin.Models.ViewModels;
using IK_Project.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using IK_Project.UI.Areas.Admin.Models.ViewModels.UserVMs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Domain.Enums;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using Castle.Core.Smtp;
using System.Web;
using IK_Project.Domain.Entities.Concrete;

namespace IK_Project.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAccountService _accountService;
        private readonly IEMailSenderService _emailSenderService;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAccountService accountService, IEMailSenderService emailSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
            _emailSenderService = emailSenderService;
        }

        public IActionResult Login()
        {
            //return View();
            // return RedirectToAction("Create", "Admin", new { area = "Admin" });
            LoginVM model = new LoginVM();
            return View(model);
        }
        [HttpPost]

        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                Console.WriteLine("Email veya şifre hatalı");
                return View(model);
            }
            var checkPass = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!checkPass.Succeeded)
            {
                Console.WriteLine("Email veya şifre hatalı");
                return View(model);
            }

            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole == null)
            {
                Console.WriteLine("Kullanıcıya air rol bulunamadı");
                return View(model);

            }
            bool IsActive = await _accountService.ComponysIsActive(user.Id, userRole[0]);
            if (!IsActive)
            {
                Console.WriteLine("Kullanıcı aktif değil");
                model.Email = "deactive";
                return View(model);
            }

            var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

            //var passwordChangeClaim = userPrincipal.Claims.FirstOrDefault(c => c.Type == "PasswordChangeRequired" && c.Value == "Employee");
            var passwordChangeClaim = userPrincipal.Claims.FirstOrDefault(c => c.Type == "PasswordChangeRequired");


            if (passwordChangeClaim != null)
            {
                if (passwordChangeClaim.Value == "CompanyManager")
                {
                    return RedirectToAction("ChangePassword", "Account");

                }
                else if (passwordChangeClaim.Value == "Employee")
                {
                    return RedirectToAction("ChangePassword", "Account");

                }
                else if (passwordChangeClaim.Value == "DepartmentManager")
                {
                    return RedirectToAction("ChangePassword", "Account");

                }
            }
            else
            {
                // Şifre değiştirme gerekliliği yok, ana sayfaya yönlendirin.
                return RedirectToAction("Index", "Home", new { Area = userRole[0].ToString() });
            }
            return RedirectToAction("Index", "Home", new { Area = userRole[0].ToString() });
        }
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
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
        public IActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PasswordReset(ResetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {


                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                string encodedToken = WebUtility.UrlEncode(token);
                string resetLink = $"https://localhost:7132/UpdatePassword/{user.Id}/{encodedToken}";

                //var resetLink = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                //string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                //string mailBody = $"<a target=\"_blank\"href=\"http://localhost:5254{Url.Action("UpdatePassword", "Account", new { userId = user.Id, token = HttpUtility.UrlEncode(token) })}\"> Click to update your password </a>";
                string subject = "Update Password";
                await _emailSenderService.SendEmailAsync(user.Email, subject, resetLink);
                ViewBag.State = true;
                ViewBag.Message = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi.";
                return View();

            }

            return View(model);



        }
        [HttpGet("[action]/{userId}/{token}")]
        //[HttpGet("/account/[action]/{userId}/{token}", Name = "UpdatePassword")]

        public IActionResult UpdatePassword(string userId, string token)
        {
            return View();
        }

        [HttpPost("[action]/{userId}/{token}")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel vm, string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), vm.NewPassword);
            if (result.Succeeded)
            {
                ViewBag.State = true;
                await _userManager.UpdateSecurityStampAsync(user);
                return RedirectToAction("Login", "Account");


            }
            else
            {
                ViewBag.State = false;
            }

            return View();
        }

        //Console.WriteLine("Email veya şifre hatalı");
        //return View(model);
        //else
        //{
        //    ViewBag.State = false;
        //}



        //string recepientEmail = model.Email;
        //string subject = "Welcome to HrMaster Application";
        //string message = $"Your system registration has been made by your Administrator.<br> Your UserName:{user.UserName}<br> Your Temporary Password:{user.Password}<br> You need to change it before using.";
        //await _emailSenderService.SendEmailAsync(recepientEmail, subject, message);
        //return View();




    }
}
