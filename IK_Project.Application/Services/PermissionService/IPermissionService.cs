using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.PermissionService
{
    public interface IPermissionService
    {
		Task<IResult> Create(PermissionCreateDTO permissionCreateDTO);
		Task<IResult> Edit(PermissionUpdateDTO permissionUpdateDTO);
		Task<IResult> Remove(Guid id);

		Task<IDataResult<List<PermissionListDTO>>> GetDefaults(Expression<Func<Permission, bool>> expression);
       Task<IDataResult<List<PermissionListDTO>>> AllPermissions();
        Task<IDataResult<PermissionUpdateDTO>> GetById(Guid id);
        Task<IDataResult<List<PermissionListDTO>>> GetDMPermission(Guid Id);
        Task<IDataResult<List<PermissionListDTO>>> GetEmployeePermission(Guid Id);
    }
}
