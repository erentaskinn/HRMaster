using AutoMapper;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using IK_Project.Infrastructure.Repositories.Concreates;
using IK_Project.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly ICompanyService _companyService;
		private readonly IDepartmantService _departmantService;
        private readonly IEMailSenderService _emailSenderservice;
        public EmployeeService(IEmployeeRepository employeerRepository, IMapper mapper, IAccountService accountService, UserManager<IdentityUser> userManager, ICompanyService companyService, IDepartmantService departmantService, IEMailSenderService eMailSenderService)
        {
            this._employeeRepository = employeerRepository;
            this._mapper = mapper;
            _accountService = accountService;
            _userManager = userManager;
            _companyService = companyService;
            _departmantService = departmantService;
			_emailSenderservice = eMailSenderService;
        }

        public async Task<IDataResult<List<EmployeeListDTO>>> AllEmployees()
        {
            var departmants = await _employeeRepository.GetAllAsync();
            if (departmants.Count() <= 0)
            {
                return new ErrorDataResult<List<EmployeeListDTO>>("Employee not found.");
            }
            return new SuccessDataResult<List<EmployeeListDTO>>(_mapper.Map<List<EmployeeListDTO>>(departmants), "Employee listed successfully.");
        }
        public async Task<IDataResult<List<EmployeeListDTO>>> AllActiveEmployees()
        {
			var departmants = await _employeeRepository.GetDefault(x => x.IsActive == true);
            if (departmants.Count() <= 0)
            {
                return new ErrorDataResult<List<EmployeeListDTO>>("Employee not found.");
            }
            return new SuccessDataResult<List<EmployeeListDTO>>(_mapper.Map<List<EmployeeListDTO>>(departmants), "Employee listed successfully.");
        }

        public async Task<IDataResult<EmployeeCreateDTO>> Create(EmployeeCreateDTO employeeDto)
        {

			if (await _accountService.AnyAsync(x => x.Email == employeeDto.Email))
			{

				return new ErrorDataResult<EmployeeCreateDTO>("The employee is already registered.");
			}
			IdentityUser identityUser = new()
			{
				Email = employeeDto.Email,
				EmailConfirmed = true,
				UserName = employeeDto.Email
			};
			DataResult<EmployeeCreateDTO> result = new ErrorDataResult<EmployeeCreateDTO>();
			var strategy = await _employeeRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _employeeRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.CreateUserAsync(identityUser, employeeDto.Password, Roles.Employee);
					if (!identityResult.Succeeded)
					{
						result = new ErrorDataResult<EmployeeCreateDTO>(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}
                    var claimAdded = await _accountService.AddPasswordChangeClaimAsync(identityUser, Roles.Employee.ToString());
                    if (!claimAdded)
                    {
                        result = new ErrorDataResult<EmployeeCreateDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var employee = _mapper.Map<Employee>(employeeDto);
					employee.IdentityId = identityUser.Id;
					await _employeeRepository.AddAsync(employee);
					await _employeeRepository.SaveChangeAsync();
					result = new SuccessDataResult<EmployeeCreateDTO>(_mapper.Map<EmployeeCreateDTO>(employee), "Employee addition is successful.");
					transactionScope.Commit();
                    string recepientEmail = employeeDto.Email;
                    string subject = "Welcome to HrMaster Application";
                    string message = $"Your system registration has been made by your Administrator.<br> Your UserName:{identityUser.UserName}<br> Your Temporary Password:{employeeDto.Password}<br> You need to change it before using.";
                    await _emailSenderservice.SendEmailAsync(recepientEmail, subject, message);
                }
				catch (Exception ex)
				{
					result = new ErrorDataResult<EmployeeCreateDTO>($"Employee addition is unsuccessful.- {ex.Message}");
					transactionScope.Rollback();

				}
				finally
				{
					transactionScope.Dispose();

				}

			});
			return result;

        }

        public async Task<IDataResult<EmployeeUpdateDTO>> Edit(EmployeeUpdateDTO employeeDto)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeDto.Id);
            if (employee == null)
            {
                return new ErrorDataResult<EmployeeUpdateDTO>("Employee not found.");
            }
            employeeDto.Email =employee.Email ;
			var identityUser = await _userManager.FindByIdAsync(employee.IdentityId);
			identityUser.Email = employeeDto.Email;
			identityUser.EmailConfirmed = true;
			identityUser.UserName = employeeDto.Email;

			DataResult<EmployeeUpdateDTO> result = new ErrorDataResult<EmployeeUpdateDTO>();
			var strategy = await _employeeRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _employeeRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.UpdateUserAsync(identityUser);
					if (!identityResult.Succeeded)
					{
						result = new ErrorDataResult<EmployeeUpdateDTO>(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}
					var updateemployee = _mapper.Map(employeeDto, employee);

					await _employeeRepository.UpdateAsync(updateemployee);
					await _employeeRepository.SaveChangeAsync();
					result = new SuccessDataResult<EmployeeUpdateDTO>(_mapper.Map<EmployeeUpdateDTO>(updateemployee), "Employee update successful.");
					transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorDataResult<EmployeeUpdateDTO>($"Employee update failed.- {ex.Message}");
					transactionScope.Rollback();

				}
				finally
				{
					transactionScope.Dispose();

				}

			});
			return result;


        }

        public async Task<IDataResult<EmployeeDTO>> GetAsync(string id)
        {
            var employee = await _employeeRepository.GetAsync(x => x.IdentityId == id);
            if (employee == null)
            {
                return new ErrorDataResult<EmployeeDTO>("Employee not found.");

            }
            var employeeDto = _mapper.Map<EmployeeDTO>(employee);
            var departmant =await _departmantService.GetById(employee.DepartmantId);
			var company = await _companyService.GetById(departmant.Data.CompanyId);
			departmant.Data.CompanyId = company.Data.Id;
			employeeDto.CompanyID = company.Data.Id;
            return new SuccessDataResult<EmployeeDTO>(employeeDto, "Employee found.");
        }

        public async Task<IDataResult<EmployeeDTO>> GetById(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return new ErrorDataResult<EmployeeDTO>("Employee not found");
            }
			var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
            return new SuccessDataResult<EmployeeDTO>(employeeDTO, "Employee found");
        }

        public async Task<IDataResult<List<EmployeeListDTO>>> GetDefaults(Expression<Func<Employee, bool>> expression)
        {
            var employees = await _employeeRepository.GetAllAsync(expression);
            if (employees.Count() <= 0)
            {
                return new ErrorDataResult<List<EmployeeListDTO>>("Employee not found");
            }
            return new SuccessDataResult<List<EmployeeListDTO>>(_mapper.Map<List<EmployeeListDTO>>(employees), "Employee listed successfully.");
        }

        public Task<IDataResult<List<EmployeeListDTO>>> GetDefaults(Expression<Func<Menu, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> Remove(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return new ErrorResult("Employee not found");
            }

			var identityUser = await _userManager.FindByNameAsync(employee.IdentityId);
			Result result = new ErrorResult();
			var strategy = await _employeeRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _employeeRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.DeleteUserAsync(identityUser.Id);
					if (!identityResult.Succeeded)
					{
						result = new ErrorResult(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}

					await _employeeRepository.DeleteAsync(employee);
					await _employeeRepository.SaveChangeAsync();
					result = new SuccessResult("Employee deletion successful.");
					transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorResult($"Employee deletion unsuccessful.- {ex.Message}");
					transactionScope.Rollback();

				}
				finally
				{
					transactionScope.Dispose();

				}

			});
			return result;

        }
    }
}
