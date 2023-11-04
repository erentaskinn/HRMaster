using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
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
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerAdvanceVM;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IK_Project.UI.Areas.DepartmantManager.Controllers
{
    public class AdvanceController : BaseController
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
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmantService _departmantService;


        public AdvanceController(IMapper mapper, ICompanyManagerService cmService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IExpenseService expenseService,
        IAdvanceService advanceService,
        IPermissionService permissionService, ICompanyService companyService, IAccountService accountService, IAdminService adminService, IDepartmantManagerService departmantManagerService, IEmployeeService employeeService, IDepartmantService departmantService = null)

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
            _employeeService = employeeService;
            _departmantService = departmantService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllAdvance()
        {
            var user = await _accountService.GetUserAsync(User);
            var DepartmantManager = await _departmantManagerService.GetAsync(user.Id);
            var result = await _departmantService.GetDefaults(x => x.DepartmantManagerId == DepartmantManager.Data.Id);
            var departmant = result.Data.FirstOrDefault();
            var employees = await _employeeService.GetDefaults(x => x.DepartmantId == departmant.Id);
            List<AdvanceListDTO> advanceListDTOs = new List<AdvanceListDTO>();
            if (employees != null)
            {

                foreach (var item in employees.Data)
                {
                    var advance = await _advanceService.GetEmployeeAdvance(item.Id);
                    foreach (var data in advance.Data)
                    {
                        data.EmployeeName = $"{item.Name} {item.LastName}";
                        advanceListDTOs.Add(data);

                    }
                }
            }
            var advanceListVMs = _mapper.Map<List<DepartmantManagerAdvanceListVM>>(advanceListDTOs);
            return View(advanceListVMs);
        }
        public async Task<IActionResult> Aprove(Guid Id)
        {
            var result = await _advanceService.GetById(Id);
            if (!result.IsSuccess)
            {
                return View();
            }
            result.Data.ConfirmationStatus = Domain.Enums.ConfirmationStatus.Confirm;
            await _advanceService.Edit(result.Data);
            return RedirectToAction("GetAllAdvance", "Advance", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Reject(Guid Id)
        {
            var result = await _advanceService.GetById(Id);
            if (!result.IsSuccess)
            {
                return View();
            }
            result.Data.ConfirmationStatus = Domain.Enums.ConfirmationStatus.Reject;
            await _advanceService.Edit(result.Data);
            return RedirectToAction("GetAllAdvance", "Advance", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Add()
        {
            var user = await _accountService.GetUserAsync(User);
            var departmantManager = await _departmantManagerService.GetAsync(user.Id);
            if (!departmantManager.IsSuccess)
            {
                return BadRequest();
            }

            DepartmantManagerAdvanceCreatVm advanceCreateVM = new()
            {
                DepartmantManagerId = departmantManager.Data.Id,

            };

            return View(advanceCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(DepartmantManagerAdvanceCreatVm vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (vm.Amount > 100000)
                    {
                        TempData["SalaryError"] = "The amount demanded cannot be more than 3 times your salary.";
                        return View(vm);
                    }
                    else
                    {
                        var user = await _accountService.GetUserAsync(User);
                        var DepartmantManager = await _departmantManagerService.GetAsync(user.Id);
                        vm.ConfirmationStatus = ConfirmationStatus.Pending;
                        vm.DateOfReply = DateTime.Now;
                        var dto = _mapper.Map<AdvanceCreateDTO>(vm);
                        await _advanceService.Create(dto);
                        return RedirectToAction("GetAdvanceDM");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }
            }
            return View(vm);

        }
        public async Task<IActionResult> Update(Guid id)
        {
            var advance = await _advanceService.GetById(id);
            if (!advance.IsSuccess)
            {
                return BadRequest();
            }
            return View(_mapper.Map<DepartmantManagerAdvanceUpdateVM>(advance.Data));
        }
        [HttpPost]
        public async Task<IActionResult> Update(DepartmantManagerAdvanceUpdateVM advanceUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(advanceUpdateVM);
            }
            var updateAdvance = _mapper.Map<AdvanceUpdateDTO>(advanceUpdateVM);
            updateAdvance.DateOfReply= DateTime.Now;
            var advance = await _advanceService.Edit(updateAdvance);
            return RedirectToAction("GetAdvanceDM", "Advance", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            await _advanceService.Remove(Id);
            return RedirectToAction("GetAdvanceDM", "Advance", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> GetAdvanceDM()
        {
            var user = await _accountService.GetUserAsync(User);
            var DepartmantManager = await _departmantManagerService.GetAsync(user.Id);
            var result = await _advanceService.GetDMAdvance(DepartmantManager.Data.Id);
           var advanceList=_mapper.Map<List<DepartmantManagerAdvanceListVM>>(result.Data);
            return View(advanceList);
        }

    }
}
