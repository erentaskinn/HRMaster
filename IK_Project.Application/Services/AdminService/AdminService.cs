using AutoMapper;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Services.AccountService;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using IK_Project.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.AdminSevice
{
    public class AdminService : IAdminService
    {
		private readonly IAdminRepository _adminRepository;
		private readonly IMapper _mapper;
		private readonly IAccountService _accountService;
		private readonly UserManager<IdentityUser> _userManager;



		public AdminService(IAdminRepository adminRepository, IMapper mapper, IAccountService accountService, UserManager<IdentityUser> userManager)
		{
			_adminRepository = adminRepository;
			_mapper = mapper;
			_accountService = accountService;
			_userManager = userManager;
		}

		public async Task<IDataResult<List<AdminListDTO>>> AllAdminsAsync()
		{
			var admins = await _adminRepository.GetAllAsync();
			if (admins.Count() <= 0)
			{
				return new ErrorDataResult<List<AdminListDTO>>("admin bulunamadı.");

			}
			
			return new SuccessDataResult<List<AdminListDTO>>(_mapper.Map<List<AdminListDTO>>(admins), "admin listeleme başarılı");
			
		}

		public async Task<IDataResult<AdminCreateDTO>> CreateAsync(AdminCreateDTO adminCreateDTO)
		{

			if (await _accountService.AnyAsync(x => x.Email == adminCreateDTO.Email))
			{

				return new ErrorDataResult<AdminCreateDTO>("The admin is already registered.");
			}
			IdentityUser identityUser = new()
			{
				Email = adminCreateDTO.Email,
				EmailConfirmed = true,
				UserName = adminCreateDTO.Email
			};

			DataResult<AdminCreateDTO> result = new ErrorDataResult<AdminCreateDTO>();
			var strategy = await _adminRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _adminRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.CreateUserAsync(identityUser, adminCreateDTO.Password, Roles.Admin);
					if (!identityResult.Succeeded)
					{
						result = new ErrorDataResult<AdminCreateDTO>(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}
					var admin = _mapper.Map<Admin>(adminCreateDTO);
					admin.IdentityId = identityUser.Id;
					await _adminRepository.AddAsync(admin);
					await _adminRepository.SaveChangeAsync();
					result = new SuccessDataResult<AdminCreateDTO>(_mapper.Map<AdminCreateDTO>(admin), "Admin addition is successful.");
					transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorDataResult<AdminCreateDTO>($"Admin addition is unsuccessful.- { ex.Message}");
					transactionScope.Rollback();

				}
				finally
				{
					transactionScope.Dispose();

				}

			});
			return result;


		}

		public async Task<IDataResult<AdminAdminUpdateDTO>> EditAsync(AdminAdminUpdateDTO adminUpdateDTO)
		{
			var admin = await _adminRepository.GetByIdAsync(adminUpdateDTO.Id);
			if (admin == null)
			{
				return new ErrorDataResult<AdminAdminUpdateDTO>("Admin not found.");

			}
			admin.Email = adminUpdateDTO.Email;			
			var identityUser = await _userManager.FindByIdAsync(admin.IdentityId);
			identityUser.Email = adminUpdateDTO.Email;
			identityUser.EmailConfirmed = true;
			identityUser.UserName = adminUpdateDTO.Email;




			DataResult<AdminAdminUpdateDTO> result = new ErrorDataResult<AdminAdminUpdateDTO>();
			var strategy = await _adminRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _adminRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.UpdateUserAsync(identityUser);
					if (!identityResult.Succeeded)
					{
						result = new ErrorDataResult<AdminAdminUpdateDTO>(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}
					var updateAdmin = _mapper.Map(adminUpdateDTO, admin);

					await _adminRepository.UpdateAsync(updateAdmin);
					await _adminRepository.SaveChangeAsync();
					result = new SuccessDataResult<AdminAdminUpdateDTO>(_mapper.Map<AdminAdminUpdateDTO>(updateAdmin), "Admin update successful.");
					transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorDataResult<AdminAdminUpdateDTO>($"Admin update failed.- {ex.Message}");
					transactionScope.Rollback();

				}
				finally
				{
					transactionScope.Dispose();

				}

			});
			return result;

		}

		public async Task<IDataResult<AdminAdminUpdateDTO>> GetAsync(string id)
		{
			var admin = await _adminRepository.GetAsync(x => x.IdentityId == id);
			if (admin == null)
			{
				return new ErrorDataResult<AdminAdminUpdateDTO>("Admin not found.");

			}
			var adminDto = _mapper.Map<AdminAdminUpdateDTO>(admin);

			return new SuccessDataResult<AdminAdminUpdateDTO>(adminDto, "Admin found.");

		}

		public async Task<IDataResult<List<AdminListDTO>>> GetDefaultsAsync(Expression<Func<Admin, bool>> expression)
		{
			var admins = await _adminRepository.GetDefault(expression);
			if (admins == null)
			{
				return new ErrorDataResult<List<AdminListDTO>>("Admins couldn't be listed.");

			}
			var adminDto = _mapper.Map<List<AdminListDTO>>(admins);

			return new SuccessDataResult<List<AdminListDTO>>(adminDto, "Admins listed successfully.");

		
		}

		public async Task<bool> IsAdminExistsAsync(Guid adminId)
		{
			return await _adminRepository.AnyAsync(x => x.Id == adminId);

		}

		public async Task<IResult> RemoveAsync(Guid id)
		{
		
			var admin = await _adminRepository.GetByIdAsync(id);
			if (admin is null)
			{
				return new ErrorResult("Admin not found.");
			}

			var identityUser = await _userManager.FindByIdAsync(admin.IdentityId);

			Result result = new ErrorResult();
			var strategy = await _adminRepository.CreateExecutionStrategy();
			await strategy.ExecuteAsync(async () =>
			{
				using var transactionScope = await _adminRepository.BeginTransactionAsync().ConfigureAwait(false);
				try
				{
					var identityResult = await _accountService.DeleteUserAsync(identityUser.Id);
					if (!identityResult.Succeeded)
					{
						result = new ErrorResult(identityResult.ToString());
						transactionScope.Rollback();
						return;
					}

					await _adminRepository.DeleteAsync(admin);
					await _adminRepository.SaveChangeAsync();
					result = new SuccessResult("Admin deletion successful.");
					transactionScope.Commit();
				}
				catch (Exception ex)
				{
					result = new ErrorResult($"Admin deletion unsuccessful.- {ex.Message}");
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
