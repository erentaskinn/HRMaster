using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.AppUserService
{
    public interface IAppUserService
    {
        Task<IDataResult<AppUserInformationDTO>> CreateAsync(AppUserInformationDTO appUserInformationDTO);
        Task<IDataResult<List<AppUserListDTO>>> AllAppUserInformation(Expression<Func<AppUser, bool>> expression);
        Task<IDataResult<AppUserUpdateDTO>> GetById(Guid id);
        Task<IDataResult<AppUserUpdateDTO>> Edit(AppUserUpdateDTO appUserDTO);
    }
}
