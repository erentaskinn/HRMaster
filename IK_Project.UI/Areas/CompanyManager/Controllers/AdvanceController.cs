using AutoMapper;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Services.AdvanceService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.CompanyManager.Models;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerAdvanceVM;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.UI.Areas.CompanyManager.Models.AdvanceVM;
using IK_Project.UI.Areas.Employee.Models;

namespace IK_Project.UI.Areas.CompanyManager.Controllers
{
    public class AdvanceController : CompanyManagerBaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdvanceService _advanceService;
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        readonly private ICompanyManagerService _cmService;
        private readonly IAccountService _accountService;
        private readonly IDepartmantManagerService _departmantManagerService;
        readonly private ICompanyService _companyService;
        readonly private IDepartmantService _departmantService;

        IMapper _mapper;


        public AdvanceController(UserManager<IdentityUser> userManager, IAdvanceService advanceService, IEmployeeService employeeService, IMapper mapper, IWebHostEnvironment webHostEnvironment, ICompanyManagerService cmService, IAccountService accountService, IDepartmantManagerService departmantManagerService, ICompanyService companyService, IDepartmantService departmantService)
        {
            _userManager = userManager;
            _advanceService = advanceService;
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

        public async Task<IActionResult> CheckAdvanceDemands()
        {
            List<CompanyManagerAdvanceListVM> companyManagerAdvanceListVMs = new List<CompanyManagerAdvanceListVM>();
            var advances = await _advanceService.AllAdvances();
            if (!advances.IsSuccess)
            {
                return View(companyManagerAdvanceListVMs);
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
                    List<AdvanceListDTO> advanceListDTOs = new List<AdvanceListDTO>();
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
                    companyManagerAdvanceListVMs = _mapper.Map<List<CompanyManagerAdvanceListVM>>(advanceListDTOs);
                    return View(companyManagerAdvanceListVMs);
                }
            }
            return View(companyManagerAdvanceListVMs);
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
            return RedirectToAction("CheckAdvanceDemands");
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
            return RedirectToAction("CheckAdvanceDemands");
        }

        //[HttpGet]
        //public async Task<IActionResult> AdvanceDetails(Guid id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        AdvanceUpdateVM advance = _mapper.Map<AdvanceUpdateVM>(await _advanceService.GetById(id));

        //        return View(advance);


        //        //var dto = _mapper.Map<CompanyListDTO>(id);
        //        //await _companyService.AllCompanies();
        //        //var company = await _context.Movie.SingleOrDefaultAsync(m => m.ID == id);
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> AdvanceDetails(AdvanceUpdateVM vm)
        //{

        //    var expenseDto = _mapper.Map<AdvanceUpdateDTO>(vm);
        //    await _advanceService.Edit(expenseDto);
        //    return RedirectToAction("CheckAdvanceDemands");
        //}

    }
}
