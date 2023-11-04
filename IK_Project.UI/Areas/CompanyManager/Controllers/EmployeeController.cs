using AutoMapper;
using Humanizer;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.MenuService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs;
using IK_Project.UI.Areas.CompanyManager.Models;
using IK_Project.UI.Areas.CompanyManager.Models.Departmant;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.Employee.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using EmployeeUpdateVM = IK_Project.UI.Areas.CompanyManager.Models.Employee.EmployeeUpdateVM;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class EmployeeController : CompanyManagerBaseController
    {
        readonly private IMapper _mapper;
        readonly private ICompanyManagerService _cmService;
        readonly private ICompanyService _companyService;
        readonly private IEmployeeService _employeeService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        IWebHostEnvironment _webHostEnvironment;
        private readonly IMenuService _menuService;
        private readonly IEMailSenderService _emailSender;
        private readonly IDepartmantService _departmantService;
        private readonly IDepartmantManagerService _departmantManagerService;
        private readonly IAccountService _accountService;
        public EmployeeController(IMapper mapper, ICompanyManagerService cmService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IEmployeeService employeeService, IEMailSenderService emailSender, ICompanyService companyService, IDepartmantService departmantService, IAccountService accountService, IMenuService menuService, IDepartmantManagerService departmantManagerService)
        {
            _mapper = mapper;
            _cmService = cmService;
            _signInManager = signInManager;
            _userManager = userManager;
            this._webHostEnvironment = webHostEnvironment;
            _employeeService = employeeService;
            _emailSender = emailSender;
            _companyService = companyService;
            _departmantService = departmantService;
            _accountService = accountService;
            _menuService = menuService;
            _departmantManagerService = departmantManagerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult test()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _employeeService.Remove(id);
                return RedirectToAction("GetAllEmployees");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            List<CompanyManagerEmployeeListVM> companyManagerEmployeeListVM = new List<CompanyManagerEmployeeListVM>();
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            var employees = await _employeeService.AllActiveEmployees();
            List<EmployeeListDTO> listEmployee = new List<EmployeeListDTO>();
            if (employees.IsSuccess && departmants.IsSuccess)
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
                companyManagerEmployeeListVM = _mapper.Map<List<CompanyManagerEmployeeListVM>>(listEmployee);
                return View(companyManagerEmployeeListVM);
            }
            return View(companyManagerEmployeeListVM);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            var employee = await _employeeService.GetById(id);
            var companyManagerEmployeeUpdateVM = _mapper.Map<CompanyManagerEmployeeUpdateVM>(employee.Data);
            companyManagerEmployeeUpdateVM.DepartmantDTOs = departmants.Data;
            return View(companyManagerEmployeeUpdateVM);
        }

        public async Task<CompanyManagerEmployeeUpdateVM> Get(Guid id)
        {
            var employeeDTO = await _employeeService.GetById(id);
            CompanyManagerEmployeeUpdateVM employee = _mapper.Map<CompanyManagerEmployeeUpdateVM>(employeeDTO.Data);
            List<DepartmantListDTO> listDepartmant = _mapper.Map<List<DepartmantListDTO>>(await _departmantService.AllDepartmants()).Where(x => x.CompanyId == employee.CompanyID).ToList();
            employee.DepartmantDTOs = listDepartmant;
            foreach (var departmant in listDepartmant)
            {
                if (departmant.Id == employee.DepartmantId)
                {
                    employee.DepartmantName = departmant.Name;
                }
            }
            return employee;
        }

        [HttpPost]
        public async Task<IActionResult> Details(CompanyManagerEmployeeUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            try
            {
                var editDto = _mapper.Map<EmployeeUpdateDTO>(vm);
                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    editDto.ProfilePhoto = savedFilePath;

                }
                await _employeeService.Edit(editDto);
                return RedirectToAction("GetAllEmployees");
            }
            catch (Exception ex)
            {
                {
                    TempData["error"] = "Error occured";
                    return View(vm);
                }
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CompanyManagerEmployeeCreateVM vm = new CompanyManagerEmployeeCreateVM();
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            var departmantManagers = await _departmantManagerService.AllActiveManagers();
            List<DepartmantManagerListDTO> CompanyDepartmantManagers = new List<DepartmantManagerListDTO>();
            if (!departmants.IsSuccess)
            {
                return View(vm);
            }
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

            
            vm.CompanyID = company.Data.Id;
            vm.DepartmantDTOs = departmants.Data;
            vm.CompanyName = company.Data.CompanyName;
            var companyPackageId = company.Data.MenuId;
            var CompanyDepartmantManagersNumber = CompanyDepartmantManagers.Count();
            var employeeNumber = listEmployee.Count() + 1;//sirkette kac calisan oldugunu gosteriyor
            var package = await _menuService.GetById(company.Data.MenuId);
            var packageEmloyeeRange = package.Data.UserAmount;   // sirketin sahip oldugu paketin kullanici sayisi
            var total = employeeNumber + CompanyDepartmantManagersNumber;
            if (packageEmloyeeRange > total)
            {
                return View(vm);
            }
            TempData["EmployeeRangeError"] = $"You have reached the user limit. You have 1 manager, {CompanyDepartmantManagersNumber} departmant manager  and {employeeNumber} employees. Your package has a user limit of {packageEmloyeeRange}. Please upgrade your package.";
            return RedirectToAction("Index");

        }
        [HttpPost]
        public async Task<IActionResult> Create(CompanyManagerEmployeeCreateVM vm)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    string temporaryPassword = GeneratePassword();
                    vm.Password = temporaryPassword;
                    vm.PasswordChangeRequired = true;
                    if (!string.IsNullOrEmpty(vm.CustomEmail))
                    {
                        vm.Email = vm.CustomEmail;
                    }
                    else
                    {
                        vm.Email = CreateMailAddress(vm.Name, vm.LastName/*, companyUpdateDTO.CompanyName*/);
                    }
                    var dto = _mapper.Map<EmployeeCreateDTO>(vm);
                    if (vm.LogoFile != null)
                    {
                        string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                        dto.ProfilePhoto = savedFilePath;

                    }
                    var result = await _employeeService.Create(dto);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction("GetAllEmployees");
                    }
                    else
                    {
                        TempData["error"] = "Error occured";
                    }
                }
                catch (Exception ex)
                {

                    TempData["error"] = ex.Message;
                }
            }
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            vm.CompanyID = company.Data.Id;
            vm.DepartmantDTOs = departmants.Data;
            return View(vm);
        }

        public static string GeneratePassword()
        {
            var random = new Random();
            string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string digits = "0123456789";
            string specialCharacters = "!*+.";

            string password = new string(Enumerable.Repeat(
                uppercaseLetters + lowercaseLetters + digits + specialCharacters, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            // Şifreye en az bir rakam eklemek için:
            int position = random.Next(0, 8);
            char randomDigit = digits[random.Next(digits.Length)];
            password = password.Substring(0, position) + randomDigit + password.Substring(position + 1);

            return password;
        }
        public static string ConvertTurkishCharactersToEnglishLower(string input)
        {
            string output = input;
            output = output.Replace("ç", "c");
            output = output.Replace("ğ", "g");
            output = output.Replace("ı", "i");
            output = output.Replace("ö", "o");
            output = output.Replace("ş", "s");
            output = output.Replace("ü", "u");
            output = output.Replace("Ç", "c");
            output = output.Replace("Ğ", "g");
            output = output.Replace("I", "i");
            output = output.Replace("İ", "i");
            output = output.Replace("Ö", "o");
            output = output.Replace("Ş", "s");
            output = output.Replace("Ü", "u");
            return output;
        }
        public static string CreateMailAddress(string userName, string userLastName/*, string companyName*/)
        {
            string mailAdress = ConvertTurkishCharactersToEnglishLower((userName + "." + userLastName + "@" /*+ companyName*/ + "bilgeadamboost.com").ToLower());
            return mailAdress;
        }

    }
}
