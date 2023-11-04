using AutoMapper;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
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
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerExpenseVMs;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerPermission;
using IK_Project.UI.Areas.Employee.Models.PermissionVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace IK_Project.UI.Areas.DepartmantManager.Controllers
{
    public class PermissionController : BaseController
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
        private readonly IDepartmantService _departmanService;


        public PermissionController(IMapper mapper, ICompanyManagerService cmService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IExpenseService expenseService,
        IAdvanceService advanceService,
        IPermissionService permissionService, ICompanyService companyService, IAccountService accountService, IAdminService adminService, IDepartmantManagerService departmantManagerService, IEmployeeService employeeService, IDepartmantService departmanService)

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
            _departmanService = departmanService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllPermission()
        {
            var user = await _accountService.GetUserAsync(User);
            var DepartmantManager = await _departmantManagerService.GetAsync(user.Id);
            var result = await _departmanService.GetDefaults(x => x.DepartmantManagerId == DepartmantManager.Data.Id);
            var departmant = result.Data.FirstOrDefault();
            var employees = await _employeeService.GetDefaults(x => x.DepartmantId == departmant.Id);
            List<PermissionListDTO> permissionListDTOs = new List<PermissionListDTO>();
            if (employees != null)
            {
                foreach (var item in employees.Data)
                {
                    var permission = await _permissionService.GetEmployeePermission(item.Id);
                    foreach (var data in permission.Data)
                    {
                        data.EmployeeName = $"{item.Name} {item.LastName}";
                        permissionListDTOs.Add(data);
                    }
                }
            }
            var permissionList = _mapper.Map<List<DepartmanManagerPermissionListVM>>(permissionListDTOs);
            return View(permissionList);
        }
        public async Task<IActionResult> Aprove(Guid Id)
        {
            var result = await _permissionService.GetById(Id);
            if (!result.IsSuccess)
            {
                return View();
            }
            result.Data.ConfirmationStatus = Domain.Enums.ConfirmationStatus.Confirm;
            await _permissionService.Edit(result.Data);
            return RedirectToAction("GetAllPermission", "Permission", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Reject(Guid Id)
        {
            var result = await _permissionService.GetById(Id);
            if (!result.IsSuccess)
            {
                return View();
            }
            result.Data.ConfirmationStatus = Domain.Enums.ConfirmationStatus.Reject;
            await _permissionService.Edit(result.Data);
            return RedirectToAction("GetAllPermission", "Permission", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Add()
        {
            var user = await _accountService.GetUserAsync(User);
            var departmantManager = await _departmantManagerService.GetAsync(user.Id);
            if (!departmantManager.IsSuccess)
            {
                return BadRequest();
            }

            DepartmantManagerPermissionCreateVM permissionCreateVM = new()
            {
                 DepartmantManagerId= departmantManager.Data.Id,

            };

            return View(permissionCreateVM);
        }


        [HttpPost]
        public async Task<IActionResult> Add(DepartmantManagerPermissionCreateVM vm)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                    vm.ConfirmationStatus = ConfirmationStatus.Pending;
                    var dto = _mapper.Map<PermissionCreateDTO>(vm);
                    await _permissionService.Create(dto);
                    return RedirectToAction("GetPermissionDM");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }

            }
            return View(vm);

        }
        public async Task<IActionResult> Update(Guid id)
        {
            var permission = await _permissionService.GetById(id);
            if (!permission.IsSuccess)
            {
                return BadRequest();
            }
            return View(_mapper.Map<DepartmantManagerPermissionUpdateVM>(permission.Data));
        }
        [HttpPost]
        public async Task<IActionResult> Update(DepartmantManagerPermissionUpdateVM permissionUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(permissionUpdateVM);
            }
            var updatePermission = _mapper.Map<PermissionUpdateDTO>(permissionUpdateVM);
            var permission = await _permissionService.Edit(updatePermission);
            return RedirectToAction("GetPermissionDM", "Permission", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            await _permissionService.Remove(Id);
            return RedirectToAction("GetPermissionDM", "Permission", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> GetPermissionDM()
        {
            var user = await _accountService.GetUserAsync(User);
            var DepartmantManager = await _departmantManagerService.GetAsync(user.Id);
            var result = await _permissionService.GetDMPermission(DepartmantManager.Data.Id);
            var permissionList = _mapper.Map<List<DepartmanManagerPermissionListVM>>(result.Data);
            return View(permissionList);
        }
    }
}
