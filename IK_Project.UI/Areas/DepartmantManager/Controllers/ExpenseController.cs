using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
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
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerAdvanceVM;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerExpenseVMs;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IK_Project.UI.Areas.DepartmantManager.Controllers
{
    public class ExpenseController : BaseController
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


        public ExpenseController(IMapper mapper, ICompanyManagerService cmService, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment, IExpenseService expenseService,
        IAdvanceService advanceService,
        IPermissionService permissionService, ICompanyService companyService, IAccountService accountService, IAdminService adminService, IDepartmantManagerService departmantManagerService, IEmployeeService employeeService, IDepartmantService departmantService)

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
        public async Task<IActionResult> GetAllExpense()
        {
            var user = await _accountService.GetUserAsync(User);
            var DepartmantManager = await _departmantManagerService.GetAsync(user.Id);
            var result = await _departmantService.GetDefaults(x => x.DepartmantManagerId == DepartmantManager.Data.Id);
            var departmant = result.Data.FirstOrDefault();
            var employees = await _employeeService.GetDefaults(x => x.DepartmantId == departmant.Id);
            List<ExpenseListDTO> expenseListDTOs = new List<ExpenseListDTO>();
            if (employees != null)
            {

                foreach (var item in employees.Data)
                {
                    var expense = await _expenseService.GetEmployeeExpense(item.Id);
                    foreach (var data in expense.Data)
                    {
                        data.EmployeeName = $"{item.Name} {item.LastName}";
                        expenseListDTOs.Add(data);

                    }
                }
            }
            var expenseList = _mapper.Map<List<DepartmantManagerExpenseListVM>>(expenseListDTOs);
            return View(expenseList);
        }
        public async Task<IActionResult> Aprove(Guid Id)
        {
            var result = await _expenseService.GetById(Id);
            if (!result.IsSuccess)
            {
                return View();
            }
            result.Data.ConfirmationStatus = Domain.Enums.ConfirmationStatus.Confirm;
            await _expenseService.Edit(result.Data);
            return RedirectToAction("GetAllExpense", "Expense", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Reject(Guid Id)
        {
            var result = await _expenseService.GetById(Id);
            if (!result.IsSuccess)
            {
                return View();
            }
            result.Data.ConfirmationStatus = Domain.Enums.ConfirmationStatus.Reject;
            await _expenseService.Edit(result.Data);
            return RedirectToAction("GetAllExpense", "Expense", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Add()
        {
            var user = await _accountService.GetUserAsync(User);
            var departmantManager = await _departmantManagerService.GetAsync(user.Id);
            if (!departmantManager.IsSuccess)
            {
                return BadRequest();
            }

            DepartmantManagerExpenseCreateVM expenseCreateVM = new()
            {
                DepartmantManagerId = departmantManager.Data.Id,

            };

            return View(expenseCreateVM);

        }
        [HttpPost]
        public async Task<IActionResult> Add(DepartmantManagerExpenseCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var departmantManager = await _departmantManagerService.GetById(vm.DepartmantManagerId);

                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
                vm.ConfirmationStatus = ConfirmationStatus.Pending;
                vm.DateOfReply = DateTime.Now;
                var dto = _mapper.Map<ExpenseCreateDTO>(vm);
                await _expenseService.Create(dto);
                return RedirectToAction("GetExenseDM");
            }
            return View(vm);

        }
        public async Task<IActionResult> Update(Guid id)
        {
            var advance = await _expenseService.GetById(id);
            if (!advance.IsSuccess)
            {
                return BadRequest();
            }
            return View(_mapper.Map<DepartmantManagerExpenseUpdateVm>(advance.Data));
        }
        [HttpPost]
        public async Task<IActionResult> Update(DepartmantManagerExpenseUpdateVm expenseUpdateVm)
        {
            if (!ModelState.IsValid)
            {
                return View(expenseUpdateVm);
            }
            expenseUpdateVm.DateOfReply = DateTime.Now;
            var updateExpense = _mapper.Map<ExpenseUpdateDTO>(expenseUpdateVm);
            var expense = await _expenseService.Edit(updateExpense);
            return RedirectToAction("GetExenseDM", "Expense", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            await _expenseService.Remove(Id);
            return RedirectToAction("GetAdvanceDM", "Advance", new { area = "DepartmantManager" });
        }
        public async Task<IActionResult> GetExenseDM()
        {
            var user = await _accountService.GetUserAsync(User);
            var DepartmantManager = await _departmantManagerService.GetAsync(user.Id);
            var result = await _expenseService.GetDMExpense(DepartmantManager.Data.Id);
            var expenceList = _mapper.Map<List<DepartmantManagerExpenseListVM>>(result.Data);
            return View(expenceList);
        }

    }
}

