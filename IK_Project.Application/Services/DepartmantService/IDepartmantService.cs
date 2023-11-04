using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.DepartmantService
{
    public interface IDepartmantService
    {
        Task<IDataResult<DepartmantCreateDTO>> Create(DepartmantCreateDTO departmantDto);
        Task<IDataResult<DepartmantUpdateDTO>> Edit(DepartmantUpdateDTO departmantDto);
        Task<IResult> Remove(Guid id);
        Task<IDataResult<List<DepartmantListDTO>>> GetDefaults(Expression<Func<Departmant, bool>> expression);
        Task<IDataResult<List<DepartmantListDTO>>> AllDepartmants();
        Task<IDataResult<DepartmantDTO>> GetById(Guid id);
    }
}
