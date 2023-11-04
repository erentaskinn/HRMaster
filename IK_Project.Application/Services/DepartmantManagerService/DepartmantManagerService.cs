using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.DepartmantService;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.PermissionService;
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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace IK_Project.Application.Services.DepartmantManagerService
{
    public class DepartmantManagerService : IDepartmantManagerService
    {
        private readonly IDepartmanManagerRepository _departmanManagerRepository;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEMailSenderService _emailSenderservice;
        private readonly IDepartmantService _departmantService;
        private readonly IEmployeeService _employeeService;
        private readonly IPermissionService _permissionService;
        private readonly IDepartmantRepository _departmantRepository;

        public DepartmantManagerService(IDepartmanManagerRepository departmanManagerRepository, IMapper mapper, IAccountService accountService, UserManager<IdentityUser> userManager, IEMailSenderService emailSenderservice, IDepartmantService departmantService, IEmployeeService employeeService, IPermissionService permissionService, IDepartmantRepository departmantRepository)
        {
            _departmanManagerRepository = departmanManagerRepository;
            _mapper = mapper;
            _accountService = accountService;
            _userManager = userManager;
            _emailSenderservice = emailSenderservice;
            _departmantService = departmantService;
            _employeeService = employeeService;
            _permissionService = permissionService;
            _departmantRepository = departmantRepository;
        }



        public async Task<IDataResult<List<DepartmantManagerListDTO>>> AllManagers()
        {
            var departmantManagers = await _departmanManagerRepository.GetAllAsync();
            if (departmantManagers.Count() <= 0)
            {
                return new ErrorDataResult<List<DepartmantManagerListDTO>>("Departman Manager not found.");

            }
            var departmanManagerListDto = _mapper.Map<List<DepartmantManagerListDTO>>(departmantManagers);
            return new SuccessDataResult<List<DepartmantManagerListDTO>>(departmanManagerListDto, "Departmant Managers listed successfully.");
        }
        public async Task<IDataResult<List<DepartmantManagerListDTO>>> AllActiveManagers()
        {
            var departmantManagers = await _departmanManagerRepository.GetDefault(x => x.IsActive == true);
            if (departmantManagers.Count() <= 0)
            {
                return new ErrorDataResult<List<DepartmantManagerListDTO>>("Departman Manager not found.");

            }
            var departmanManagerListDto = _mapper.Map<List<DepartmantManagerListDTO>>(departmantManagers);
            return new SuccessDataResult<List<DepartmantManagerListDTO>>(departmanManagerListDto, "Departmant Managers listed successfully.");
        }

        public async Task<IDataResult<DepartmantManagerCreateDTO>> Create(DepartmantManagerCreateDTO departmantManagerCreateDTO)
        {
            var departmant = await _departmantRepository.GetByIdAsync(departmantManagerCreateDTO.DepartmantId, false);
            if (await _accountService.AnyAsync(x => x.Email == departmantManagerCreateDTO.Email))
            {
                return new ErrorDataResult<DepartmantManagerCreateDTO>("The departmant mnanager is already registered.");
            }
            IdentityUser identityUser = new()
            {
                Email = departmantManagerCreateDTO.Email,
                EmailConfirmed = true,
                UserName = departmantManagerCreateDTO.Email
            };
            DataResult<DepartmantManagerCreateDTO> result = new ErrorDataResult<DepartmantManagerCreateDTO>();
            var strategy = await _departmanManagerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transactionScope = await _departmanManagerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.CreateUserAsync(identityUser, departmantManagerCreateDTO.Password, Roles.DepartmantManager);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<DepartmantManagerCreateDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var claimAdded = await _accountService.AddPasswordChangeClaimAsync(identityUser, Roles.DepartmantManager.ToString());
                    if (!claimAdded)
                    {
                        result = new ErrorDataResult<DepartmantManagerCreateDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var departmantManager = _mapper.Map<DepartmantManager>(departmantManagerCreateDTO);
                    departmantManager.IdentityId = identityUser.Id;
                    departmantManager.IsActive = true;
                    await _departmanManagerRepository.AddAsync(departmantManager);
                    await _departmanManagerRepository.SaveChangeAsync();
                    departmant.DepartmantManagerId = departmantManager.Id;
                    await _departmantRepository.UpdateAsync(departmant);
                    await _departmantRepository.SaveChangeAsync();
                    result = new SuccessDataResult<DepartmantManagerCreateDTO>(_mapper.Map<DepartmantManagerCreateDTO>(departmantManager), "Departmant manager addition is successful.");
                    string recepientEmail = departmantManagerCreateDTO.Email;
                    string subject = "Welcome to HrMaster Application";
                    string message = $"Your system registration has been made by your Administrator.<br> Your UserName:{identityUser.UserName}<br> Your Temporary Password:{departmantManagerCreateDTO.Password}<br> You need to change it before using.";
                    await _emailSenderservice.SendEmailAsync(recepientEmail, subject, message);
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<DepartmantManagerCreateDTO>($"Departmant manager addition is unsuccessful.- {ex.Message}");
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();

                }

            });
            return result;
        }

        public async Task<IDataResult<DepartmantManagerUpdateDTO>> Edit(DepartmantManagerUpdateDTO departmantManagerUpdateDTO)
        {
            var departmantManager = await _departmanManagerRepository.GetByIdAsync(departmantManagerUpdateDTO.Id);
            if (departmantManager == null)
            {
                return new ErrorDataResult<DepartmantManagerUpdateDTO>("Departmant Manager not found.");
            }
            departmantManager.Email = departmantManagerUpdateDTO.Email;
            var identityUser = await _userManager.FindByIdAsync(departmantManager.IdentityId);
            identityUser.Email = departmantManagerUpdateDTO.Email;
            identityUser.EmailConfirmed = true;
            identityUser.UserName = departmantManagerUpdateDTO.Email;
            DataResult<DepartmantManagerUpdateDTO> result = new ErrorDataResult<DepartmantManagerUpdateDTO>();
            var strategy = await _departmanManagerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transactionScope = await _departmanManagerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.UpdateUserAsync(identityUser);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<DepartmantManagerUpdateDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var updateDepartmantMnager = _mapper.Map(departmantManagerUpdateDTO, departmantManager);
                    await _departmanManagerRepository.UpdateAsync(updateDepartmantMnager);
                    await _departmanManagerRepository.SaveChangeAsync();
                    result = new SuccessDataResult<DepartmantManagerUpdateDTO>(_mapper.Map<DepartmantManagerUpdateDTO>(updateDepartmantMnager), "Departmant Manager update successful.");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<DepartmantManagerUpdateDTO>($"Company Manager update failed.- {ex.Message}");
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }

        
        public async Task<IDataResult<DepartmantManagerUpdateDTO>> GetAsync(string id)
        {
            var departmantManager = await _departmanManagerRepository.GetAsync(x => x.IdentityId == id);
            if (departmantManager == null)
            {
                return new ErrorDataResult<DepartmantManagerUpdateDTO>("Departmant Manager not found.");
            }
            var DepartmantManagerDTO = _mapper.Map<DepartmantManagerUpdateDTO>(departmantManager);
            return new SuccessDataResult<DepartmantManagerUpdateDTO>(DepartmantManagerDTO, "Departmant Manager found.");
        }

        public async Task<IDataResult<DepartmantManagerUpdateDTO>> GetById(Guid id)
        {
            var departmantManager = await _departmanManagerRepository.GetByIdAsync(id);
            if (departmantManager == null)
            {
                return new ErrorDataResult<DepartmantManagerUpdateDTO>("Departmant Manager not found.");
            }
            var departmantManagerDTO = _mapper.Map<DepartmantManagerUpdateDTO>(departmantManager);
            return new SuccessDataResult<DepartmantManagerUpdateDTO>(departmantManagerDTO, "Departmant Manager found.");
        }

        public async Task<IDataResult<List<DepartmantManagerListDTO>>> GetDefaults(Expression<Func<DepartmantManager, bool>> expression)
        {
            var departmantManagers = await _departmanManagerRepository.GetDefault(expression);
            if (departmantManagers == null)
            {
                return new ErrorDataResult<List<DepartmantManagerListDTO>>("Departmant Managers couldn't be listed.");

            }
            var departmantManagersDto = _mapper.Map<List<DepartmantManagerListDTO>>(departmantManagers);

            return new SuccessDataResult<List<DepartmantManagerListDTO>>(departmantManagersDto, "Departmant Managers listed successfully.");
        }

        public async Task<IDataResult<DepartmantDTO>> GetDepartmant(Guid Id)
        {
            var departmant=_departmantService.GetById(Id);
            if (departmant == null)
            {
                return new ErrorDataResult<DepartmantDTO>("Departmant bulunamadı");
            }
            return new SuccessDataResult<DepartmantDTO>("Departmant bulundu");
        }       
        

        public async Task<bool> IsManagerExists(Guid managerId)
        {
            return await _departmanManagerRepository.AnyAsync(x => x.Id == managerId);
        }

        public async Task<IResult> Remove(Guid id)
        {
            var departmantManger = await _departmanManagerRepository.GetByIdAsync(id);
            if (departmantManger is null)
            {
                return new ErrorResult("Departmant Manager not found.");
            }
            var identityUser = await _userManager.FindByNameAsync(departmantManger.IdentityId);
            Result result = new ErrorResult();
            var strategy = await _departmanManagerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transactionScope = await _departmanManagerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.DeleteUserAsync(identityUser.Id);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorResult(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }

                    await _departmanManagerRepository.DeleteAsync(departmantManger);
                    await _departmanManagerRepository.SaveChangeAsync();
                    result = new SuccessResult("Departmant Manager deletion successful.");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorResult($"Departmant Manager deletion unsuccessful.- {ex.Message}");
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
