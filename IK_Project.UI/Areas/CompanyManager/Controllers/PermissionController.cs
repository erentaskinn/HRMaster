using AutoMapper;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.ExpenseService;
using IK_Project.Application.Services.PermissionService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.CompanyManager.Models;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using IK_Project.UI.Areas.Employee.Models.PermissionVMs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.UI.Areas.CompanyManager.Models.AdvanceVM;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.UI.Areas.CompanyManager.Models.PermissionVM;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class PermissionController : CompanyManagerBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IPermissionService _permissionService;
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly private ICompanyManagerService _cmService;
        private readonly IAccountService _accountService;
        readonly private ICompanyService _companyService;
        readonly private IDepartmantService _departmantService;
        private readonly IDepartmantManagerService _departmantManagerService;

        IMapper _mapper;

        public PermissionController(UserManager<IdentityUser> userManager, IPermissionService permissionService, IEmployeeService employeeService, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICompanyManagerService cmService, IAccountService accountService, ICompanyService companyService, IDepartmantService departmantService, IDepartmantManagerService departmantManagerService)
        {
            _userManager = userManager;
            _permissionService = permissionService;
            _employeeService = employeeService;
            _webHostEnvironment = webHostEnvironment;
            _cmService = cmService;
            _mapper = mapper;
            _accountService = accountService;
            _companyService = companyService;
            _departmantService = departmantService;
            _departmantManagerService = departmantManagerService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CheckPermissionDemands()
        {
            List<CompanyManagerPermissionListVM> companyManagerPermissionListVM = new List<CompanyManagerPermissionListVM>();
            var permissions = await _permissionService.AllPermissions();
            if (!permissions.IsSuccess)
            {
                return View(companyManagerPermissionListVM);
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
                    companyManagerPermissionListVM = _mapper.Map<List<CompanyManagerPermissionListVM>>(permissionListDTOs);
                    return View(companyManagerPermissionListVM);
                }
            }
            return View(companyManagerPermissionListVM);
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
            return RedirectToAction("CheckPermissionDemands");
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
            return RedirectToAction("CheckPermissionDemands");
        }
    }
}
