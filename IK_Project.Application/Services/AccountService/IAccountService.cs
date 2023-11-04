using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.AccountService
{
    public interface IAccountService
    {
        Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression);
        Task<IdentityUser?> FindByIdAsync(string identityId);
        Task<IdentityResult> CreateUserAsync(IdentityUser user, string password, Roles role);
        Task<IdentityResult> DeleteUserAsync(string identityId);
        Task<Guid> GetUserIdAsync(string identityId, string role);
		Task<IdentityResult> UpdateUserAsync(IdentityUser user);
		//ık projesinden gelenler..
		Task<SignInResult> Login(LoginDTO loginDTO);
		Task LogOut();
		Task<List<UserListDTO>> GetUsers();
		Task<IdentityUser> GetById(Guid id);

		Task<IdentityUser> GetByUserName(string userName);
		Task<List<string>> GetUserAssignedRoles(IdentityUser appUser);
		Task AddRole(IdentityUser user, string appRoleName);
		Task<IdentityUser> GetByEMailAddress(string eMailAddress);
		Task<IdentityUser> GetUserAsync(ClaimsPrincipal user);
		Task<bool> AddPasswordChangeClaimAsync(IdentityUser user, string claimValue);
		Task<bool> RemovePasswordChangeClaimAsync(IdentityUser user, string claimValue);
		Task<bool> ComponysIsActive(string identityId, string role);







    }
}
