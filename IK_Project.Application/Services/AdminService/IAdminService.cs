using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.AdminSevice
{
    public interface IAdminService
    {
		Task<IDataResult<AdminCreateDTO>> CreateAsync(AdminCreateDTO adminCreateDTO);
		Task<IDataResult<AdminAdminUpdateDTO>> EditAsync(AdminAdminUpdateDTO adminUpdateDTO);
        Task<IResult> RemoveAsync(Guid id);

		Task<IDataResult<List<AdminListDTO>>> GetDefaultsAsync(Expression<Func<Admin, bool>> expression);
		Task<IDataResult<List<AdminListDTO>>> AllAdminsAsync();

		Task<IDataResult<AdminAdminUpdateDTO>> GetAsync(string id);
        Task<bool> IsAdminExistsAsync(Guid adminId);



	}
}
