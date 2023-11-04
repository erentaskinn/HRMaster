using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.AdvanceService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.Employee.Models;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IK_Project.UI.Areas.Employee.Controllers
{
    [Area("Employee")]
    [Authorize(Roles = "Employee")]
    public class AdvanceController:Controller
    {      
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdvanceService _advanceService;
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAccountService _accountService;
        private readonly  IMapper _mapper;


        public AdvanceController(UserManager<IdentityUser> userManager, IAdvanceService advanceService, IEmployeeService employeeService, IMapper mapper, IWebHostEnvironment webHostEnvironment, IAccountService accountService )
        {
            _userManager = userManager;
            _advanceService = advanceService;
            _employeeService = employeeService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetMyAllAdvances()
        {
            var user = await _accountService.GetUserAsync(User);
            var employee = await _employeeService.GetAsync(user.Id);
            var result = await _advanceService.GetEmployeeAdvance(employee.Data.Id);
            var advanceList=_mapper.Map<List<AdvanceListVM>>(result.Data);
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

            AdvanceCreateVM advanceCreateVM = new()
            {
                EmployeeId = employee.Data.Id,
                
            };

            return View(advanceCreateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdvanceCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var employee = await _employeeService.GetById(vm.EmployeeId);
                    if (vm.Amount>(employee.Data.Salary*3))
                    {
                        TempData["SalaryError"] = "The amount demanded cannot be more than 3 times your salary.";
                        return View(vm);
                    }
                    else
                    {
                        vm.ConfirmationStatus = ConfirmationStatus.Pending;
                        vm.DateOfReply= DateTime.Now;
                        var dto = _mapper.Map<AdvanceCreateDTO>(vm);
                        await _advanceService.Create(dto);
                        return RedirectToAction("GetMyAllAdvances");
                    }


                }
                catch(Exception ex)
                {
                    TempData["Error"] = ex.Message;
                }

            }
            return View(vm);

        }
        public async Task<IActionResult> Update(Guid id)
        {
            var advance=await _advanceService.GetById(id);
            if (!advance.IsSuccess) 
            {
                return BadRequest();
            }
            return View(_mapper.Map<AdvanceUpdateVM>(advance.Data));
        }
        [HttpPost]
        public async Task<IActionResult> Update (AdvanceUpdateVM advanceUpdateVM) 
        { 
            if (!ModelState.IsValid)
            {
                return View(advanceUpdateVM);
            }
            advanceUpdateVM.DateOfReply = DateTime.Now;
            var updateAdvance=_mapper.Map<AdvanceUpdateDTO>(advanceUpdateVM);
            var advance=await _advanceService.Edit(updateAdvance);
            return RedirectToAction("GetMyAllAdvances", "Advance", new { area = "Employee" });
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            await _advanceService.Remove(Id);
            return RedirectToAction("GetMyAllAdvances", "Advance", new { area = "Employee" });
        }

    }



}

