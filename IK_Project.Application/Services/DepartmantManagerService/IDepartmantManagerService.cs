using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.DepartmantManagerService
{
    public interface IDepartmantManagerService
    {
        Task<IDataResult<DepartmantManagerCreateDTO>> Create(DepartmantManagerCreateDTO departmantManagerCreateDTO);
        Task<IDataResult<DepartmantManagerUpdateDTO>> Edit(DepartmantManagerUpdateDTO departmantManagerUpdateDTO);
        Task<IResult> Remove(Guid id);

        Task<IDataResult<List<DepartmantManagerListDTO>>> GetDefaults(Expression<Func<DepartmantManager, bool>> expression);
        Task<IDataResult<List<DepartmantManagerListDTO>>> AllManagers();
        Task<IDataResult<List<DepartmantManagerListDTO>>> AllActiveManagers();
        Task<IDataResult<DepartmantManagerUpdateDTO>> GetById(Guid id);
        Task<bool> IsManagerExists(Guid managerId);

        Task<IDataResult<DepartmantManagerUpdateDTO>> GetAsync(string id);
        Task<IDataResult<DepartmantDTO>> GetDepartmant(Guid Id);
       
    }
}
