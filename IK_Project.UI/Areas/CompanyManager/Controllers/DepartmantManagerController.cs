using AutoMapper;
using Humanizer;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.MenuService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyManager;
using IK_Project.UI.Areas.CompanyManager.Models.DepartmanManager;
using IK_Project.UI.Areas.CompanyManager.Models.Departmant;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.DepartmantManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class DepartmantManagerController : CompanyManagerBaseController
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
        private readonly IAccountService _accountService;
        private readonly IDepartmantManagerService _departmantManagerService;

        public DepartmantManagerController(IMapper mapper, ICompanyManagerService cmService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IEmployeeService employeeService, IEMailSenderService emailSender, ICompanyService companyService, IDepartmantService departmantService, IAccountService accountService, IMenuService menuService, IDepartmantManagerService departmantManagerService)
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
        public async Task<IActionResult> Index()
        {
            List<CompanyManagerDepartmantManagerListVM> companyManagerDepartmantManagerListVM = new List<CompanyManagerDepartmantManagerListVM>();
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            var departmantManagers = await _departmantManagerService.AllActiveManagers();
            List<DepartmantManagerListDTO> CompanyDepartmantManagers = new List<DepartmantManagerListDTO>();
            if (departmantManagers.IsSuccess && departmants.IsSuccess)
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
                companyManagerDepartmantManagerListVM = _mapper.Map<List<CompanyManagerDepartmantManagerListVM>>(CompanyDepartmantManagers);
                return View(companyManagerDepartmantManagerListVM);
            }
            return View(companyManagerDepartmantManagerListVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _departmantManagerService.Remove(id);
                return RedirectToAction("Details");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var listDepartmants = (await _departmantService.AllDepartmants()).Data.Where(x => x.CompanyId == company.Data.Id && x.DepartmantManagerId != null).ToList();
            var listNullDepartmants = (await _departmantService.AllDepartmants()).Data.Where(x => x.CompanyId == company.Data.Id && x.DepartmantManagerId == null).ToList(); // şirketin yöneticisi olmayan departmanlarını getirir
            var departmantManagers = await _departmantManagerService.AllActiveManagers();
            var employees = await _employeeService.AllActiveEmployees();
            List<EmployeeListDTO> listEmployee = new List<EmployeeListDTO>();
            foreach (var departmant in listDepartmants)
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
            CompanyManagerDepartmantManagerCreateVM vm = new CompanyManagerDepartmantManagerCreateVM();
            vm.DepartmantDTOs = listNullDepartmants;
            vm.CompanyID = company.Data.Id;
            var package = await _menuService.GetById(company.Data.MenuId);
            var packageEmloyeeRange = package.Data.UserAmount;   // sirketin sahip oldugu paketin kullanici sayisi
            var CompanyDepartmantManagersNumber = listDepartmants.Count();
            var employeeNumber = listEmployee.Count() + 1;//sirkette kac calisan oldugunu gosteriyor
            var total = employeeNumber + CompanyDepartmantManagersNumber;
            if (packageEmloyeeRange > total)
            {
                return View(vm);
            }
            TempData["EmployeeRangeError"] = $"You have reached the user limit. You have 1 manager, {CompanyDepartmantManagersNumber} departmant manager  and {employeeNumber} employees. Your package has a user limit of {packageEmloyeeRange}. Please upgrade your package.";
            return RedirectToAction("Error");

        }
        public IActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CompanyManagerDepartmantManagerCreateVM vm)
        {
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            vm.CompanyID=company.Data.Id;
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
                    var dto = _mapper.Map<DepartmantManagerCreateDTO>(vm);
                    if (vm.LogoFile != null)
                    {
                        string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                        dto.ProfilePhoto = savedFilePath;

                    }
                    var result = await _departmantManagerService.Create(dto);
                    if (result.IsSuccess)
                    {
                        return RedirectToAction("index");
                    }
                    else
                    {
                        TempData["error"] = "Departmant Manager Can Not Create";
                        return RedirectToAction("Error");
                    }
                }
                catch (Exception ex)
                {

                    TempData["error"] = ex.Message;
                }
            }
            var listNullDepartmants = (await _departmantService.AllDepartmants()).Data.Where(x => x.CompanyId == companyManager.Data.CompanyId && x.DepartmantManagerId == null).ToList(); // şirketin yöneticisi olmayan departmanlarını getirir
            vm.DepartmantDTOs = listNullDepartmants;
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.Id == company.Data.Id);
            var departmantManager = await _departmantManagerService.GetById(id);
            var companyManagerDepartmanManagerUpdateVM = _mapper.Map<CompanyManagerDepartmantManagerUpdateVM>(departmantManager.Data);
            companyManagerDepartmanManagerUpdateVM=await Get(id);
            return View(companyManagerDepartmanManagerUpdateVM);
        }
        public async Task<CompanyManagerDepartmantManagerUpdateVM> Get(Guid id)
        {
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var allCompanies = await _companyService.AllCompanies();
            foreach (var company in allCompanies.Data)
            {
                if (company.CompanyManagerId == companyManager.Data.Id)
                {
                    companyManager.Data.CompanyId = company.Id;
                }
            }
            var departmantManagerDTO = await _departmantManagerService.GetById(id);
            CompanyManagerDepartmantManagerUpdateVM departmantManager = _mapper.Map<CompanyManagerDepartmantManagerUpdateVM>(departmantManagerDTO.Data);
            var listdepartmant = (await _departmantService.AllDepartmants()).Data.Where(x => x.CompanyId == companyManager.Data.CompanyId && x.DepartmantManagerId ==null || x.DepartmantManagerId == departmantManager.Id).ToList();
            List<DepartmantListDTO> listDepartmantDTO = _mapper.Map<List<DepartmantListDTO>>(listdepartmant);
            departmantManager.DepartmantDTOs = listDepartmantDTO;
            foreach (var departmant in listDepartmantDTO)
            {
                if (departmant.DepartmantManagerId == departmantManager.Id)
                {
                    departmantManager.DepartmantName = departmant.Name;
                }
            }
            return departmantManager;
        }
        [HttpPost]
        public async Task<IActionResult> Details(CompanyManagerDepartmantManagerUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
          
            try
            {   vm.IsActive= true;
                var editDto = _mapper.Map<DepartmantManagerUpdateDTO>(vm);
                if (vm.LogoFile != null)
                {
                    string savedFilePath = await vm.LogoFile.SaveFileAsync(_webHostEnvironment.WebRootPath, "images");
                    editDto.ProfilePhoto = savedFilePath;

                }
                await _departmantManagerService.Edit(editDto);
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                {
                    TempData["error"] = "Error occured";
                    return View(vm);
                }
            }
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
