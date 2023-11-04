using AutoMapper;
using Castle.Core.Smtp;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Application.Services.CompanyService;
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
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.CompanyManagerService
{
    public class CompanyManagerService : ICompanyManagerService
    {
		private readonly ICompanyManagerRepository _companyManagerRepository;
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IEMailSenderService _emailSenderservice;
		private readonly ICompanyRepository _companyRepository;
        public CompanyManagerService(ICompanyManagerRepository companyManagerRepository, IMapper mapper, UserManager<IdentityUser> userManager, IAccountService accountService, IEMailSenderService emailSenderservice, ICompanyRepository companyRepository)
        {
            _companyManagerRepository = companyManagerRepository;
            this._mapper = mapper;
            _userManager = userManager;
            _accountService = accountService;
            _emailSenderservice = emailSenderservice;
            _companyRepository = companyRepository;
        }

        public async Task<IDataResult<List<CompanyManagerListDTO>>> AllManagers()
		{
			var companyManagers= await _companyManagerRepository.GetAllAsync();
			if (companyManagers.Count()<0)
			{
				return new ErrorDataResult<List<CompanyManagerListDTO>>("CompanyManager not found.");
			}
			var companyManagerListDto = _mapper.Map<List<CompanyManagerListDTO>>(companyManagers);
			return new SuccessDataResult<List<CompanyManagerListDTO>>(companyManagerListDto, "Company Managers listed successfully.");
		}
		public async Task<IDataResult<CompanyManagerCreateDTO>> Create(CompanyManagerCreateDTO companyManagerCreateDTO)
		{
			var company = await _companyRepository.GetByIdAsync(companyManagerCreateDTO.CompanyID);

			if (company.CompanyManagerId!=null) 
			{
                return new ErrorDataResult<CompanyManagerCreateDTO>("The company manager for this company is already registered.");

            }
            if (await _accountService.AnyAsync(x => x.Email == companyManagerCreateDTO.Email))
			{
				return new ErrorDataResult<CompanyManagerCreateDTO>("The company manager is already registered.");
			}
			IdentityUser identityUser = new()
			{
				Email = companyManagerCreateDTO.Email,
				EmailConfirmed = true,
				UserName = companyManagerCreateDTO.Email
			};
			DataResult<CompanyManagerCreateDTO> result = new ErrorDataResult<CompanyManagerCreateDTO>();
			var strategy = await _companyManagerRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _companyManagerRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.CreateUserAsync(identityUser, companyManagerCreateDTO.Password, Roles.CompanyManager);
					if (!identityResult.Succeeded)
					{
						result = new ErrorDataResult<CompanyManagerCreateDTO>(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}
                    var claimAdded = await _accountService.AddPasswordChangeClaimAsync(identityUser, Roles.CompanyManager.ToString());
                    if (!claimAdded)
                    {
                        result = new ErrorDataResult<CompanyManagerCreateDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }

                    var companyManager = _mapper.Map<CompanyManager>(companyManagerCreateDTO);
					companyManager.IdentityId = identityUser.Id;
                    await _companyManagerRepository.AddAsync(companyManager);
					await _companyManagerRepository.SaveChangeAsync();
                    company.CompanyManagerId = companyManager.Id;
					await _companyRepository.UpdateAsync(company);
					await _companyRepository.SaveChangeAsync();
					result = new SuccessDataResult<CompanyManagerCreateDTO>(_mapper.Map<CompanyManagerCreateDTO>(companyManager), "Company manager addition is successful.");
                    string recepientEmail = companyManagerCreateDTO.Email;
                    string subject = "Welcome to HrMaster Application";
                    string message = $"Your system registration has been made by your Administrator.<br> Your UserName:{identityUser.UserName}<br> Your Temporary Password:{companyManagerCreateDTO.Password}<br> You need to change it before using.";                   
                    await _emailSenderservice.SendEmailAsync(recepientEmail, subject, message);
                    transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorDataResult<CompanyManagerCreateDTO>($"Company manager addition is unsuccessful.- { ex.Message}");
					transactionScope.Rollback();
				}
				finally
				{
					transactionScope.Dispose();
				}
			});
			return result;
		}
        public async Task<IDataResult<CompanyManagerUpdateDTO>> GetAsync(string id)
        {
            var CompanyManager = await _companyManagerRepository.GetAsync(x => x.IdentityId == id);
            if (CompanyManager == null)
            {
                return new ErrorDataResult<CompanyManagerUpdateDTO>("Company Manager not found.");
            }
            var CompanyManagerDTO = _mapper.Map<CompanyManagerUpdateDTO>(CompanyManager);
            return new SuccessDataResult<CompanyManagerUpdateDTO>(CompanyManagerDTO, "Company Manager found.");
        }
        public async Task<IDataResult<CompanyManagerUpdateDTO>> Edit(CompanyManagerUpdateDTO companyManagerUpdateDTO)

        {
            var companyManager = await _companyManagerRepository.GetByIdAsync(companyManagerUpdateDTO.Id);
            if (companyManager == null)
            {
                return new ErrorDataResult<CompanyManagerUpdateDTO>("CompanyManager not found.");
            }
            companyManager.Email = companyManagerUpdateDTO.Email;
            var identityUser = await _userManager.FindByIdAsync(companyManager.IdentityId);
            identityUser.Email = companyManagerUpdateDTO.Email;
            identityUser.EmailConfirmed = true;
            identityUser.UserName = companyManagerUpdateDTO.Email;
            DataResult<CompanyManagerUpdateDTO> result = new ErrorDataResult<CompanyManagerUpdateDTO>();
            var strategy = await _companyManagerRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transactionScope = await _companyManagerRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var identityResult = await _accountService.UpdateUserAsync(identityUser);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<CompanyManagerUpdateDTO>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var updateCompanyMnager = _mapper.Map(companyManagerUpdateDTO, companyManager);
                    await _companyManagerRepository.UpdateAsync(updateCompanyMnager);
                    await _companyManagerRepository.SaveChangeAsync();
                    result = new SuccessDataResult<CompanyManagerUpdateDTO>(_mapper.Map<CompanyManagerUpdateDTO>(updateCompanyMnager), "Company Manager update successful.");
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<CompanyManagerUpdateDTO>($"Company Manager update failed.- {ex.Message}");
                    transactionScope.Rollback();
                }
                finally
                {
                    transactionScope.Dispose();
                }
            });
            return result;
        }
        public async Task<IDataResult<CompanyManagerUpdateDTO>> GetById(Guid id)
		{
			var companyManager = await _companyManagerRepository.GetByIdAsync(id);
			if (companyManager == null)
			{
				return new ErrorDataResult<CompanyManagerUpdateDTO>("Company Manager not found.");
			}
			var companyManagerDTO = _mapper.Map<CompanyManagerUpdateDTO>(companyManager);
			return new SuccessDataResult<CompanyManagerUpdateDTO>(companyManagerDTO, "Company Manager found.");
		}
		public  async Task<IDataResult<List<CompanyManagerListDTO>>> GetDefaults(Expression<Func<CompanyManager, bool>> expression)
		{
			var companyManagers = await _companyManagerRepository.GetDefault(expression);
			if (companyManagers == null)
			{
				return new ErrorDataResult<List<CompanyManagerListDTO>>("Company Managers couldn't be listed.");
			}
			var companyManagersDto = _mapper.Map<List<CompanyManagerListDTO>>(companyManagers);
			return new SuccessDataResult<List<CompanyManagerListDTO>>(companyManagersDto, "Company Managers listed successfully.");
		}
		public async Task<bool> IsManagerExists(Guid managerId)
		{
			return await _companyManagerRepository.AnyAsync(x => x.Id==managerId);
		}
		public async Task<IResult> Remove(Guid id)
		{
			var companyManager = await _companyManagerRepository.GetByIdAsync(id);
			if (companyManager is null)
			{
				return new ErrorResult("Company Manager not found.");
			}
			var identityUser = await _userManager.FindByIdAsync(companyManager.IdentityId);
			Result result = new ErrorResult();
			var strategy = await _companyManagerRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _companyManagerRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.DeleteUserAsync(identityUser.Id);
					if (!identityResult.Succeeded)
					{
						result = new ErrorResult(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}
				var company = await _companyRepository.GetByIdAsyncNull(companyManager.CompanyId);
					if (company != null)
					{
						company.CompanyManagerId = null;
						company.CompanyManager.Name = null;
						await _companyRepository.UpdateAsync(company);
						await _companyRepository.SaveChangeAsync();

					}
					companyManager.CompanyId = null;
					await _companyManagerRepository.DeleteAsync(companyManager);

					await _companyManagerRepository.SaveChangeAsync();
					result = new SuccessResult("Company Manager deletion successful.");
					transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorResult($"Company Manager deletion unsuccessful.- {ex.Message}");
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
