using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.ExpenseService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyVM;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace IK_Project.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class ExpenseController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IExpenseService _expenseService;
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;


        IMapper _mapper;


        public ExpenseController(UserManager<IdentityUser> userManager, IExpenseService expenseService, IEmployeeService employeeService, IMapper mapper, IWebHostEnvironment webHostEnvironment, IAccountService accountService )
        {
            _userManager = userManager;
            _expenseService = expenseService;
            _employeeService = employeeService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _accountService = accountService;
        }

        public async Task<IActionResult> GetMyAllExpenses()
        {
            var user = await _accountService.GetUserAsync(User);
            var employee = await _employeeService.GetAsync(user.Id);
            var result = await _expenseService.GetEmployeeExpense(employee.Data.Id);
            var advanceList = _mapper.Map<List<ExpenseListVM>>(result.Data);
            return View(advanceList);
        }


        public async Task<IActionResult> Add()
        {
            var user = await _accountService.GetUserAsync(User);
            var employee = await _employeeService.GetAsync(user.Id);
            if (!employee.IsSuccess)
            {
                return BadRequest();
            }

            ExpenseCreateVM expenseCreateVM = new()
            {
                EmployeeId = employee.Data.Id,

            };

            return View(expenseCreateVM);

        }
        [HttpPost]
        public async Task<IActionResult> Add(ExpenseCreateVM vm)
        {
            if (ModelState.IsValid)
            {            
                try
                {
                    var employee = await _employeeService.GetById(vm.EmployeeId);
                    
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
                vm.ConfirmationStatus = ConfirmationStatus.Pending;
                vm.DateOfReply= DateTime.Now;
                var dto = _mapper.Map<ExpenseCreateDTO>(vm);
                await _expenseService.Create(dto);
                return RedirectToAction("GetMyAllExpenses");
            }
            return View(vm);

        }
        public async Task<IActionResult> Update(Guid id)
        {
            var expense = await _expenseService.GetById(id);
            if (!expense.IsSuccess)
            {
                return BadRequest();
            }
            return View(_mapper.Map<ExpenseUpdateVM>(expense.Data));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ExpenseUpdateVM expenceUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(expenceUpdateVM);
            }
            expenceUpdateVM.DateOfReply = DateTime.Now;
            var updateExpense = _mapper.Map<ExpenseUpdateDTO>(expenceUpdateVM);
            var expense = await _expenseService.Edit(updateExpense);
            return RedirectToAction("GetMyAllExpenses", "Expense", new { area = "Employee" });
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            await _expenseService.Remove(Id);
            return RedirectToAction("GetMyAllExpenses", "Expense", new { area = "Employee" });
        }
    }
}
