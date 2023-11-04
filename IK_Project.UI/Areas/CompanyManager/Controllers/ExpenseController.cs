using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.ExpenseService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyVM;
using IK_Project.UI.Areas.CompanyManager.Models;
using IK_Project.UI.Areas.CompanyManager.Models.AdvanceVM;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.CompanyManager.Models.ExpenseVM;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class ExpenseController : CompanyManagerBaseController

    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IExpenseService _expenseService;
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly private ICompanyManagerService _cmService;
        private readonly IAccountService _accountService;
        readonly private ICompanyService _companyService;
        readonly private IDepartmantService _departmantService;
        private readonly IDepartmantManagerService _departmantManagerService;
        IMapper _mapper;


        public ExpenseController(UserManager<IdentityUser> userManager, IExpenseService expenseService, IEmployeeService employeeService, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICompanyManagerService cmService, IDepartmantManagerService departmantManagerService, ICompanyService companyService, IDepartmantService departmantService, IAccountService accountService)
        {
            _userManager = userManager;
            _expenseService = expenseService;
            _employeeService = employeeService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _cmService = cmService;
            _accountService = accountService;
            _departmantManagerService = departmantManagerService;
            _departmantService = departmantService;
            _companyService = companyService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CheckExpenseDemands()
        {
            List<CompanyManagerExpenseListVM> companyManagerExpenseListVM = new List<CompanyManagerExpenseListVM>();
            var expenses = await _expenseService.AllExpenses();
            if (!expenses.IsSuccess)
            {
                return View(companyManagerExpenseListVM);
            }
            var user = await _accountService.GetUserAsync(User);
            var companyManager = await _cmService.GetAsync(user.Id);
            var company = await _companyService.GetCompany(x => x.CompanyManagerId == companyManager.Data.Id);
            var departmants = await _departmantService.GetDefaults(x => x.CompanyId == company.Data.Id);
            var departmantManagers = await _departmantManagerService.AllActiveManagers();
            var employees = await _employeeService.AllActiveEmployees();
            List<DepartmantManagerListDTO> CompanyDepartmantManagers = new List<DepartmantManagerListDTO>();
            if (departmants.Data != null && departmantManagers.Data != null)
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
                List<EmployeeListDTO> listEmployee = new List<EmployeeListDTO>();
                if (employees.Data != null)
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
                    companyManagerExpenseListVM = _mapper.Map<List<CompanyManagerExpenseListVM>>(expenseListDTOs);
                    return View(companyManagerExpenseListVM);
                }
            }
            return View(companyManagerExpenseListVM);
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
            return RedirectToAction("CheckExpenseDemands");
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
            return RedirectToAction("CheckExpenseDemands");
        }
    }
}
