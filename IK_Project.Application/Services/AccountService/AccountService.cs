using AutoMapper;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Domain.Core.Base;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Infrastructure.Repositories.Concreates;
using IK_Project.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAdminRepository _adminRepository;
        SignInManager<IdentityUser> _signInManager;
        private readonly ICompanyManagerRepository _companyManagerRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IDepartmanManagerRepository _departmanManagerRepository;
        private readonly IDepartmantRepository _departmantRepository;
        private readonly IEmployeeRepository _employeeRepository;



        IMapper _mapper;





        public AccountService(UserManager<IdentityUser> userManager, IAdminRepository adminRepository, SignInManager<IdentityUser> signInManager, ICompanyManagerRepository companyManager, ICompanyRepository companyRepository, IDepartmanManagerRepository departmanManagerRepository, IDepartmantRepository departmantRepository, IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _userManager = userManager;
            _adminRepository = adminRepository;
            _signInManager = signInManager;
            _companyManagerRepository = companyManager;
            _companyRepository = companyRepository;
            _departmanManagerRepository = departmanManagerRepository;
            _departmantRepository = departmantRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task AddRole(IdentityUser user, string appRoleName)
        {
            await _userManager.AddToRoleAsync(user, appRoleName);
        }


        public async Task<bool> AnyAsync(Expression<Func<IdentityUser, bool>> expression)
        {
            return await _userManager.Users.AnyAsync(expression);
        }

        public async Task<IdentityResult> CreateUserAsync(IdentityUser user, string password, Roles role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return result;
            }
            return await _userManager.AddToRoleAsync(user, role.ToString());
        }

        public async Task<IdentityResult> DeleteUserAsync(string identityId)
        {
            var user = await _userManager.FindByIdAsync(identityId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError()
                {
                    Code = "kullanıcı bulunamadı",
                    Description = "kullanıcı bulunamadı"

                });
            }
            return await _userManager.DeleteAsync(user);

        }
        public async Task<bool> AddPasswordChangeClaimAsync(IdentityUser user, string claimValue)
        {
            var result = await _userManager.AddClaimAsync(user, new Claim("PasswordChangeRequired", claimValue));
            return result.Succeeded;
        }
        public async Task<bool> RemovePasswordChangeClaimAsync(IdentityUser user, string claimValue)
        {
            // Kullanıcıdan belirli bir claim'i kaldırmak için aşağıdaki kodu kullanabilirsiniz.
            var result = await _userManager.RemoveClaimAsync(user, new Claim("PasswordChangeRequired", claimValue));

            return result.Succeeded;
        }

        public async Task<IdentityUser?> FindByIdAsync(string identityId)
        {
            return await _userManager.FindByIdAsync(identityId);
        }

        public Task<IdentityUser?> GetByEMailAddress(string eMailAddress)
        {
            return _userManager.FindByEmailAsync(eMailAddress);
        }

        public async Task<IdentityUser?> GetById(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<IdentityUser?> GetByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<List<string>> GetUserAssignedRoles(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user) as List<string>;
        }


        public async Task<Guid> GetUserIdAsync(string identityId, string role)
        {
            BaseUser? user = role switch
            {
                "Admin" => await _adminRepository.GetByIdentityId(identityId),
            };
            return user is null ? Guid.NewGuid() : user.Id;
        }

        public async Task<List<UserListDTO>> GetUsers()
        {
            return _mapper.Map<List<UserListDTO>>(await _userManager.Users.ToListAsync());
        }

        public async Task<SignInResult> Login(LoginDTO loginDTO)
        {
            //var result = await _signInManager.PasswordSignInAsync(loginDTO.UserName, loginDTO.Password, false, false);
            var user = await GetByEMailAddress(loginDTO.Email);
            var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, false, false);
            return result;
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(IdentityUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public Task<IdentityUser?> GetUserAsync(ClaimsPrincipal user)
        {
            return (_userManager.GetUserAsync(user));
        }

        public async Task<bool> ComponysIsActive(string identityId, string role)
        {

            if (role == Roles.CompanyManager.ToString())
            {
                var companyManager = await _companyManagerRepository.GetAsync(cm => cm.IdentityId == identityId);
                var company = await _companyRepository.GetAsync(c => c.CompanyManagerId == companyManager.Id);
                if (companyManager == null || !company.IsActive || !companyManager.IsActive)
                {
                    return false;
                }
                return true;
            }
            else if (role == Roles.DepartmantManager.ToString())
            {

                var departmentManager = await _departmanManagerRepository.GetAsync(dm => dm.IdentityId == identityId);
                if (departmentManager == null)
                {
                    return false;
                }

                var department = await _departmantRepository.GetAsync(d => d.DepartmantManagerId == departmentManager.Id);
                if (department == null || !department.Company.IsActive || !departmentManager.IsActive)
                {
                    return false;
                }
                return true;
            }
            else if (role == Roles.Employee.ToString())
            {
                var employee = await _employeeRepository.GetAsync(e => e.IdentityId == identityId);

                if (employee == null)
                {
                    return false;
                }
                var departmentId = employee.DepartmantId;
                var department = await _departmantRepository.GetByIdAsync(departmentId);

                if (department == null || !department.Company.IsActive || !employee.IsActive)
                {
                    return false;
                }

                return true;

            }
            else if (role == Roles.Admin.ToString())
            { 
                return true;
            }
          return false;

        }
    }
}
