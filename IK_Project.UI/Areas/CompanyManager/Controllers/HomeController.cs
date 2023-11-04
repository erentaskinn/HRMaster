using AutoMapper;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.AdminSevice;
using IK_Project.Application.Services.AdvanceService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.ExpenseService;
using IK_Project.Application.Services.PermissionService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.UI.Areas.Admin.Models.ViewModels;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs;
using IK_Project.UI.Areas.CompanyManager.Models;
using IK_Project.UI.Areas.CompanyManager.Models.AdvanceVM;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyManager;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.CompanyManager.Models.ExpenseVM;
using IK_Project.UI.Areas.CompanyManager.Models.PermissionVM;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using IK_Project.UI.Areas.Employee.Models.PermissionVMs;
using IK_Project.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class HomeController : CompanyManagerBaseController
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
        readonly private IDepartmantService _departmantService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmantManagerService _departmantManagerService;

        public HomeController(IMapper mapper, ICompanyManagerService cmService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IExpenseService expenseService,
        IAdvanceService advanceService,
        IPermissionService permissionService, ICompanyService companyService, IAccountService accountService, IDepartmantService departmantService, IEmployeeService employeeService, IDepartmantManagerService departmantManagerService)

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
            _departmantService = departmantService;
            _employeeService = employeeService;
            _departmantManagerService = departmantManagerService;
        }
        
        public async Task<IActionResult> Index()
        {
            NotificationVM notificationVM = new NotificationVM();
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            var departmantManagers = await _departmantManagerService.AllActiveManagers();
            List<DepartmantManagerListDTO> CompanyDepartmantManagers = new List<DepartmantManagerListDTO>();
            if (departmants.IsSuccess)
            {
                foreach (var departmant in departmants.Data)
                {
                    foreach (var departmanManager in departmantManagers.Data)
                    {
                        if (departmanManager.Id == departmant.DepartmantManagerId)
                        {
                            departmanManager.DepartmantName = departmant.Name;
                            CompanyDepartmantManagers.Add(departmanManager);
                        }
                    }
                }
                var employees = await _employeeService.AllActiveEmployees();
                List<EmployeeListDTO> listEmployee = new List<EmployeeListDTO>();
                if (employees.IsSuccess)
                {
                    foreach (var departmant in departmants.Data)
                    {
                        foreach (var employee in employees.Data)
                        {
                            if (employee.DepartmantId == departmant.Id)
                            {
                                employee.DepartmantName = departmant.Name;
                                listEmployee.Add(employee);
                            }
                        }
                    }
                }
                var advances = await _advanceService.AllAdvances();
                List<AdvanceListDTO> advanceListDTOs = new List<AdvanceListDTO>();
                if (advances.IsSuccess)
                {
                    if (listEmployee != null)
                    {
                        foreach (var employee in listEmployee)
                        {
                            foreach (var advance in advances.Data)
                            {
                                if (employee.Id == advance.EmployeeId)
                                {
                                    advance.EmployeeName = $"{employee.Name} {employee.LastName}";
                                    advanceListDTOs.Add(advance);
                                }

                            }
                        }
                    }
                    if (CompanyDepartmantManagers != null)
                    {
                        foreach (var CompanyDepartmantManager in CompanyDepartmantManagers)
                        {
                            foreach (var advance in advances.Data)
                            {
                                if (CompanyDepartmantManager.Id == advance.DepartmantManagerId)
                                {
                                    advance.EmployeeName = $"{CompanyDepartmantManager.Name} {CompanyDepartmantManager.LastName}";
                                    advanceListDTOs.Add(advance);
                                }
                            }
                        }
                    }
                }


                var advanceListVMs = _mapper.Map<List<CompanyManagerAdvanceListVM>>(advanceListDTOs);
                List<AdvanceListVM> listAdvance = _mapper.Map<List<AdvanceListVM>>(advanceListDTOs);
                var expenses = await _expenseService.AllExpenses();
                List<ExpenseListDTO> expenseListDTOs = new List<ExpenseListDTO>();
                if (listEmployee != null)
                {
                    foreach (var employee in listEmployee)
                    {
                        foreach (var expence in expenses.Data)
                        {
                            if (employee.Id == expence.EmployeeId)
                            {
                                expence.EmployeeName = $"{employee.Name} {employee.LastName}";
                                expenseListDTOs.Add(expence);
                            }

                        }
                    }
                }
                if (CompanyDepartmantManagers != null)
                {
                    foreach (var CompanyDepartmantManager in CompanyDepartmantManagers)
                    {
                        foreach (var expence in expenses.Data)
                        {
                            if (CompanyDepartmantManager.Id == expence.DepartmantManagerId)
                            {
                                expence.EmployeeName = $"{CompanyDepartmantManager.Name} {CompanyDepartmantManager.LastName}";
                                expenseListDTOs.Add(expence);
                            }
                        }
                    }
                }
                var expenseListVMs = _mapper.Map<List<CompanyManagerExpenseListVM>>(expenseListDTOs);
                List<ExpenseListVM> expenseList = _mapper.Map<List<ExpenseListVM>>(expenseListDTOs);

                var permissions = await _permissionService.AllPermissions();
                List<PermissionListDTO> permissionListDTOs = new List<PermissionListDTO>();
                if (listEmployee != null)
                {
                    foreach (var employee in listEmployee)
                    {
                        foreach (var permission in permissions.Data)
                        {
                            if (employee.Id == permission.EmployeeId)
                            {
                                permission.EmployeeName = $"{employee.Name} {employee.LastName}";
                                permissionListDTOs.Add(permission);
                            }

                        }
                    }
                }
                if (CompanyDepartmantManagers != null)
                {
                    foreach (var CompanyDepartmantManager in CompanyDepartmantManagers)
                    {
                        foreach (var permission in permissions.Data)
                        {
                            if (CompanyDepartmantManager.Id == permission.DepartmantManagerId)
                            {
                                permission.EmployeeName = $"{CompanyDepartmantManager.Name} {CompanyDepartmantManager.LastName}";
                                permissionListDTOs.Add(permission);
                            }
                        }
                    }
                }
                var permissionListVMs = _mapper.Map<List<CompanyManagerPermissionListVM>>(permissionListDTOs);
                List<PermissionListVM> permissionList = _mapper.Map<List<PermissionListVM>>(permissionListDTOs);
                notificationVM.Advances = listAdvance;
                notificationVM.Permissions = permissionList;
                notificationVM.Expenses = expenseList;

                return View(notificationVM);
            }
            return View(notificationVM);
        }


        [HttpGet]
        public async Task<IActionResult> ProfileDetails()
        {
            var user = await _accountService.GetUserAsync(User);
            var result = await _cmService.GetAsync(user.Id);
            if (!result.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            var companyManagerCompanyManagerUpdateVM = _mapper.Map<CompanyManagerCompanyManagerUpdateVM>(result.Data);
            return View(companyManagerCompanyManagerUpdateVM);
        }

        [HttpPost]
        public async Task<IActionResult> ProfileDetails(CompanyManagerCompanyManagerUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            //if (Request.Form.Files.Count > 0)
            //{
            //    var uploadedFile = Request.Form.Files.FirstOrDefault(); // İlk dosyayı al

            //    if (uploadedFile != null)
            //    {
            //        var fileContentType = uploadedFile.ContentType;
            //        var allowedContentTypes = new List<string> { "image/jpeg", "image/jpg", "image/png", };

            //        if (!allowedContentTypes.Contains(fileContentType.ToLower())) // Dosya türünü küçük harfe çevirerek kontrol edin
            //        {
            //            ModelState.AddModelError("vm.ProfilePhoto", "Please upload an image file in JPEG, JPG, or PNG format only.");
            //            return View(vm);
            //        }
            //        var maxFileSizeBytes = 4194304; // 4 MB

            //        if (uploadedFile.Length > maxFileSizeBytes)
            //        {
            //            ModelState.AddModelError("ProfilePhoto", "File size exceeds the maximum allowed (4 MB).");
            //            return View(vm);
            //        }
            //string wwwrootDosyaYolu = _webHostEnvironment.WebRootPath;
            //        string dosyaAdi = Path.GetFileNameWithoutExtension(Request.Form.Files[0].FileName);
            //        string dosyaUzantisi = Path.GetExtension(Request.Form.Files[0].FileName);
            //        string tamDosyaAdi = $"{dosyaAdi}_{Guid.NewGuid()}{dosyaUzantisi}";
            //        string yeniDosyaYolu = Path.Combine($"{wwwrootDosyaYolu}/images/{tamDosyaAdi}");
            //        using (var fileStream = new FileStream(yeniDosyaYolu, FileMode.Create))
            //        {
            //            Request.Form.Files[0].CopyTo(fileStream);
            //        }
            //        vm.ProfilePhoto = tamDosyaAdi;
            //    }
            //    else
            //    {
            //        vm.ProfilePhoto = vm.ProfilePhoto;
            //    }
            //}
            var user = await _accountService.GetUserAsync(User);
            var result = await _cmService.GetAsync(user.Id);
            if (!result.IsSuccess)
            {
                TempData["error"] = "Error occured";
                return View(vm);
            }
            else
            {
                vm.Id = result.Data.Id;
                var editDto = _mapper.Map<CompanyManagerUpdateDTO>(vm);
                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    editDto.ProfilePhoto = savedFilePath;

                }
                await _cmService.Edit(editDto);
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
