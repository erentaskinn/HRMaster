using AutoMapper;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.ExpenseService;
using IK_Project.Application.Services.PermissionService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using IK_Project.UI.Areas.Employee.Models.PermissionVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IK_Project.UI.Areas.Employee.Controllers
{  
        [Area("Employee")]
        [Authorize(Roles = "Employee")]
        public class PermissionController : Controller
        {
            private readonly UserManager<IdentityUser> _userManager;
            private readonly IPermissionService _permissionService;
            private readonly IEmployeeService _employeeService;
            private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;

            IMapper _mapper;


        public PermissionController(UserManager<IdentityUser> userManager, IPermissionService permissionService, IEmployeeService employeeService, IMapper mapper, IWebHostEnvironment webHostEnvironment, IAccountService accountService)
        {
            _userManager = userManager;
            _permissionService = permissionService;
            _employeeService = employeeService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _accountService = accountService;
        }

        public async Task<IActionResult> GetMyAllPermission()
        {
            var user = await _accountService.GetUserAsync(User);
            var employee = await _employeeService.GetAsync(user.Id);
            var result = await _permissionService.GetEmployeePermission(employee.Data.Id);
            var permissionList = _mapper.Map<List<PermissionListVM>>(result.Data);
            return View(permissionList);

        }


        public async Task<IActionResult> Add()
        {
            var user = await _accountService.GetUserAsync(User);
            var employee = await _employeeService.GetAsync(user.Id);
            if (!employee.IsSuccess)
            {
                return BadRequest();
            }

            PermissionCreateVM permissionCreateVM = new()
            {
                EmployeeId = employee.Data.Id,

            };

            return View(permissionCreateVM);
        }


            [HttpPost]
            public async Task<IActionResult> Add(PermissionCreateVM vm)
            {
                if (ModelState.IsValid)
                {
                    
                    try
                    {
                        vm.ConfirmationStatus = ConfirmationStatus.Pending;
                        var dto = _mapper.Map<PermissionCreateDTO>(vm);
                        await _permissionService.Create(dto);
                        return RedirectToAction("GetMyAllPermission");
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
            return View(_mapper.Map<PermissionUpdateVM>(permission.Data));
        }
        [HttpPost]
        public async Task<IActionResult> Update(PermissionUpdateVM permissionUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(permissionUpdateVM);
            }
            var updatePermission = _mapper.Map<PermissionUpdateDTO>(permissionUpdateVM);
            var expense = await _permissionService.Edit(updatePermission);
            return RedirectToAction("GetMyAllPermission", "Permission", new { area = "Employee" });
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            await _permissionService.Remove(Id);
            return RedirectToAction("GetMyAllPermission", "Permission", new { area = "Employee" });
        }
    }
}
